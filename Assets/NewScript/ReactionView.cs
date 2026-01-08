using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

// PanelManagerではなくExperimentViewに通知する
public class ReactionView : MonoBehaviour
{
    // ★ Inspectorで ExperimentView をアタッチする
    public ExperimentView experimentView; 
    
    public GameObject redSignal;
    public GameObject blueSignal;
    
    float waitTime;
    float timer = 0f;
    float reactionTime = 0f;
    
    void OnEnable()
    {
        // 試行開始時のリセットと初期設定はViewで行う
        redSignal.SetActive(true);
        blueSignal.SetActive(false);
        timer = 0f;
        reactionTime = 0f;
        waitTime = UnityEngine.Random.Range(2f, 5f);
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        bool inputReceived = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        
        if (timer >= waitTime)
        {
            // 青信号フェーズ
            if (!blueSignal.activeSelf) // 青信号を初めて表示
            {
                redSignal.SetActive(false);
                blueSignal.SetActive(true);
                timer = 0f; // ★ Reaction Time計測開始のためタイマーリセット
            }
            
            reactionTime += Time.deltaTime; 
            
            if (inputReceived)
            {
                // 成功時の処理
                this.gameObject.SetActive(false);
                // Viewはロジックを実行せず、結果をPresenterに通知
                experimentView.NotifyReactionResult(reactionTime, false);
            }
        }
        else if (timer < waitTime && inputReceived)
        {
            // お手つき時の処理
            this.gameObject.SetActive(false);
            // Viewはロジックを実行せず、お手つきをPresenterに通知
            experimentView.NotifyReactionResult(-1f, true);
        }
    }
}