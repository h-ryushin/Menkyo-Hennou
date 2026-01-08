using System.Collections.Generic;
using System.Linq;

// UIやUnityに依存しない純粋なデータ管理クラス
public class ExperimentModel
{
    private List<float> finalResults = new List<float>();

    public int ValidTrialCount => finalResults.Count;

    // 有効な反応時間をリストに追加
    public void AddReactionTime(float time)
    {
        finalResults.Add(time);
    }

    // 格納された反応時間の平均を計算
    public float CalculateAverage()
    {
        if (finalResults.Count == 0) return 0f;
        return finalResults.Average();
    }
}