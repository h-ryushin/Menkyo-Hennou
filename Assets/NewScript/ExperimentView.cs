using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExperimentView : MonoBehaviour, IExperimentView
{
    [Header("切り替えるパネル (計16要素を想定)")]
    public GameObject[] panels; 
    public Text finalResultText; 
    public Text currentResultText; 
    public GameObject otetukiPanel; 
    
    // Presenterからの指示によって、どのパネルを表示するか決める辞書などを用意しても良い
    
    private ExperimentPresenter presenter;

    // IExperimentViewのイベントを公開
    public event Action OnNextButtonClicked;
    public event Action OnStepCompleted;
    public event Action<float, bool> OnReactionTestFinished; 

    void Start()
    {
        // === MVP 初期化 ===
        ExperimentModel model = new ExperimentModel();
        presenter = new ExperimentPresenter(this, model);
        
        // 最初のパネルだけオン
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    // === IExperimentView 実装 (Presenterからの指示) ===
    
    public void ActivatePanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
        }
        else
        {
            Debug.LogWarning($"パネルインデックス {index} は範囲外です。");
        }
    }

    public void DeactivatePanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(false);
        }
    }

    public void UpdateCurrentResultText(string text)
    {
        if (currentResultText) currentResultText.text = text;
    }

    public void UpdateFinalResultText(string text)
    {
        if (finalResultText) finalResultText.text = text;
    }

    public void SetOtetukiPanelActive(bool isActive)
    {
        if (otetukiPanel) 
        {
            otetukiPanel.SetActive(isActive);
            if (isActive)
            {
                StartCoroutine(ProceedAfterOtetuki());
            }
        }
    }
    
    private IEnumerator ProceedAfterOtetuki()
    {
        yield return new WaitForSeconds(3f); 
        otetukiPanel.SetActive(false);
        
        // お手つきパネル表示後、次のステップへ進むイベントをPresenterに通知
        OnNextButtonClicked?.Invoke();
    }

    // === UIからの入力 (外部からの呼び出し) ===

    // UIボタンのOnClickイベントから呼ばれる
    public void OnClickNextButton()
    {
        OnNextButtonClicked?.Invoke();
    }
    
    // ReactionViewやCountdownViewが完了したときに呼ばれる
    public void NotifyStepCompleted()
    {
        OnStepCompleted?.Invoke();
    }
    
    // ReactionViewが結果を出したときに呼ばれる
    public void NotifyReactionResult(float time, bool isOtetuki)
    {
        OnReactionTestFinished?.Invoke(time, isOtetuki);
    }
}