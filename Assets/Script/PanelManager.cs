using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class PanelManager : MonoBehaviour
{
    // === インスペクタで割り当てるパネル (計16要素を想定) ===
    [Header("切り替えるパネルを順番に入れる (計16要素)")]
    public GameObject[] panels; 
    
    [Header("計測スクリプトと結果Text")]
    public ReactionTester reactionTester; 
    public CountdownPanel countdownPanel; 
    public Text finalResultText; 
    public Text currentResultText; // 練習結果、本番各回の結果表示用
    public GameObject otetukiPanel; // お手つき時に表示するパネル
    
    // === プライベート変数 ===
    private int currentIndex = 0;
    private List<float> finalResults = new List<float>(); // 本番3回分の結果を格納
    private int trialCount = 0; 
    private const int MAX_TRIAL_COUNT = 3;

    void Start()
    {
        // 最初に全てオフにして、最初のパネルだけオン
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
        
        // 連携設定
        if (reactionTester != null)
        {
            reactionTester.panelManager = this; 
        }
        if (countdownPanel != null)
        {
            countdownPanel.panelManager = this;
        }
    }

    // === 主な遷移関数 (各ボタン、CountdownPanelから呼ばれる) ===
    public void NextPanel()
    {
        // 1. 今のパネルを非表示
        panels[currentIndex].SetActive(false);

        // 2. 結果パネルからの遷移の場合、結果の処理を行う (HandleTrialEndへ)
        HandleResult(currentIndex); 
        Debug.Log(currentIndex);

        // 3. 次のパネルへ
        currentIndex++;

        // 4. 範囲チェックとパネル表示
        if (currentIndex < panels.Length)
        {
            panels[currentIndex].SetActive(true);
        }
    }

    private void HandleResult(int index)
    {
        // ResultPanel (Index 4, 8, 11, 14) から NextPanel() が呼ばれたときの処理
        
        // 最終結果の計算と表示 (本番3回目結果パネルからの遷移: index 14)
        if (index == 14)
        {
            CalculateAndDisplayAverage();
        }
    }
    public void HandleSuccess(float reactionTime)
    {
        // ReactionTesterのパネルを非表示
        panels[currentIndex].SetActive(false); 
        
        // 結果処理
        currentIndex++; // 結果パネルへインデックスを進める
        HandleTrialEnd(reactionTime, false);
    }
    
    // ★★★ ReactionTesterがお手つき時に呼ぶ関数 ★★★
    public void HandleOtetuki()
    {
        // ReactionTesterのパネルを非表示
        panels[currentIndex].SetActive(false); 
        
        // お手つきパネルを表示
        if (otetukiPanel != null) otetukiPanel.SetActive(true);
        
        // お手つきパネル表示後に一定時間待って次の遷移へ
        StartCoroutine(ProceedAfterOtetuki());
    }

    private IEnumerator ProceedAfterOtetuki()
    {
        // 3秒間お手つきパネルを表示（必要に応じて時間を調整してください）
        yield return new WaitForSeconds(3f); 
        
        // お手つきパネルを非表示
        if (otetukiPanel != null) otetukiPanel.SetActive(false);
        
        // 結果処理（お手つきを記録）
        HandleTrialEnd(-1f, true);
    }
    
    // ★★★ 試行終了後の共通処理 ★★★
    private void HandleTrialEnd(float result, bool isOtetuki)
    {
        int index = currentIndex; // 結果パネルのインデックス (4, 8, 11, 14)

        // 練習後の処理 (Index 4)
        if (index == 4) 
        {
            currentResultText.text = isOtetuki ? "お手つきでした。\n準備ができたら本番へ進んでください。" : 
                                                $"練習結果: {result:f3}秒\n準備ができたら本番へ進んでください。";
        }
        // 本番後の処理 (Index 8, 11, 14)
        else if (index == 8 || index == 11 || index == 14)
        {
            if (!isOtetuki)
            {
                finalResults.Add(result);
            }
            trialCount++; 

            // 各回結果をTextに表示
            currentResultText.text = isOtetuki ? $"本番 {trialCount}回目はお手つきでした。" : 
                                                $"本番 {trialCount}回目: {result:f3}秒";
        }
        
        // 最終結果へ直行するかチェック
        if (trialCount == MAX_TRIAL_COUNT)
        {
            CalculateAndDisplayAverage();
            currentIndex = panels.Length - 1; // 最終パネル(Index 15)へ直行
            panels[currentIndex].SetActive(true); // 最終パネル表示
            return;
        }

        // 次の結果パネルを表示
        panels[currentIndex].SetActive(true);
    }

    private void CalculateAndDisplayAverage()
    {
        if (finalResults.Count == 0 || finalResultText == null)
        {
            finalResultText.text = "計測された有効な反応時間がありません。";
            return;
        }

        float average = finalResults.Average();
        
        finalResultText.text = $"本番 {finalResults.Count}回の平均反応時間:\n{average:f3} 秒";
        Debug.Log($"【最終平均】: {average:f3}秒");
    }
}