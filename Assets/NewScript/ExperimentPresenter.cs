using System;
using UnityEngine;
using System.Collections;

public class ExperimentPresenter
{
    private readonly IExperimentView _view;
    private readonly ExperimentModel _model;
    
    private int currentIndex = 0;
    private int trialCount = 0; 
    private const int MAX_TRIAL_COUNT = 3;
    
    // パネル構成のインデックスを固定（実際のシーンに合わせて修正してください）
    // 例: 0:スタート, 1:説明, 2:練習Countdown, 3:練習Test, 4:練習Result, 5:本番Countdown, ...
    private readonly int[] TEST_PANEL_INDICES = { 3, 7, 10, 13 }; // TesterパネルのIndex
    private readonly int[] RESULT_PANEL_INDICES = { 4, 8, 11, 14 }; // ResultパネルのIndex

    public ExperimentPresenter(IExperimentView view, ExperimentModel model)
    {
        _view = view;
        _model = model;

        // Viewからのイベントを購読
        _view.OnNextButtonClicked += HandleNextStep;
        _view.OnReactionTestFinished += HandleTrialEnd;
        _view.OnStepCompleted += HandleNextStep; // CountdownView完了時など

        _view.ActivatePanel(currentIndex); // 最初のパネルを表示
    }

    // Viewのボタンクリックやステップ完了時に呼ばれる
    private void HandleNextStep()
    {
        if (currentIndex >= 0 && currentIndex < RESULT_PANEL_INDICES.Length)
        {
            _view.DeactivatePanel(currentIndex);
        }

        currentIndex++;

        if (currentIndex < 16) // パネル総数に合わせて調整
        {
            _view.ActivatePanel(currentIndex);
        }
        
        // カウントダウンパネルなど、自動で次のステップに進むパネルの場合、
        // 処理が完了したらそのViewから OnStepCompleted イベントが発火します。
    }

    // ReactionTesterが結果を返したときに呼ばれる
    private void HandleTrialEnd(float resultTime, bool isOtetuki)
    {
        // 1. ReactionTesterのパネルを非表示にし、次の結果パネルのインデックスへジャンプ
        int resultPanelIndex = GetNextResultPanelIndex(currentIndex);
        _view.DeactivatePanel(currentIndex);
        currentIndex = resultPanelIndex;

        // 2. 結果テキストとデータ処理
        string resultText;
        bool isPractice = Array.IndexOf(TEST_PANEL_INDICES, currentIndex) == 0; // 練習試行か判定

        if (isOtetuki)
        {
            resultText = (isPractice ? "練習" : $"本番 {trialCount + 1}回目") + "はお手つきでした。";
            _view.SetOtetukiPanelActive(true); // お手つきパネル表示
            // ※ お手つきパネルの非表示とNextStepへの遷移は、View側（Coroutine）で行うのが自然

            // ここではお手つきパネルを表示し、Viewに後続処理を委ねる
        }
        else
        {
            resultText = (isPractice ? "練習結果" : $"本番 {trialCount + 1}回目") + $": {resultTime:f3}秒";
            
            // 本番試行の場合のみデータをModelに追加
            if (!isPractice)
            {
                _model.AddReactionTime(resultTime);
                trialCount++;
            }
        }
        
        _view.UpdateCurrentResultText(resultText);
        
        // 3. 最終試行チェック
        if (trialCount == MAX_TRIAL_COUNT)
        {
            CalculateAndDisplayFinalResult();
            currentIndex = 15; // 最終パネル(Index 15)へ
        }

        // 4. 結果パネルを表示
        _view.ActivatePanel(currentIndex);
    }
    
    // 最終結果の計算と表示を指示
    private void CalculateAndDisplayFinalResult()
    {
        float average = _model.CalculateAverage();
        string finalDisplay = $"本番 {_model.ValidTrialCount}回の平均反応時間:\n{average:f3} 秒";
        
        _view.UpdateFinalResultText(finalDisplay);
    }
    
    // ReactionTesterのパネルインデックスから、結果パネルのインデックスを計算
    private int GetNextResultPanelIndex(int currentTesterIndex)
    {
        int testerIndexInArray = Array.IndexOf(TEST_PANEL_INDICES, currentTesterIndex);
        if (testerIndexInArray != -1 && testerIndexInArray < RESULT_PANEL_INDICES.Length)
        {
            return RESULT_PANEL_INDICES[testerIndexInArray];
        }
        return currentTesterIndex + 1; // 安全のためのフォールバック
    }
}