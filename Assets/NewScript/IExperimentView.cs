using System;
using UnityEngine;

// PresenterがView（UI）に指示を出すためのインターフェース
public interface IExperimentView
{
    // === Presenter -> View への指示 ===
    
    void ActivatePanel(int index);
    void DeactivatePanel(int index);
    void UpdateCurrentResultText(string text);
    void UpdateFinalResultText(string text);
    void SetOtetukiPanelActive(bool isActive);

    // === View -> Presenter への通知 (イベント) ===

    // Viewのボタンが押されたときにPresenterに通知する
    event Action OnNextButtonClicked;
    
    // ReactionViewやCountdownViewが完了したときにPresenterに通知する
    event Action OnStepCompleted; 
    
    // ReactionViewから結果を通知する（具体的なデータが必要なため、Action<T>を使う）
    event Action<float, bool> OnReactionTestFinished; // <反応時間, お手つきフラグ>
}