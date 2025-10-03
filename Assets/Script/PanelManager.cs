using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("切り替えるパネルを順番に入れる")]
    public GameObject[] panels;
    private int currentIndex = 0;

    void Start()
    {
        // 最初に全部オフにして一番目だけオン
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
    }

    public void NextPanel()
    {
        // 今のパネルを非表示
        panels[currentIndex].SetActive(false);

        // 次のパネルへ
        currentIndex++;

        // 範囲チェック（最後を超えたら終了）
        if (currentIndex < panels.Length)
        {
            panels[currentIndex].SetActive(true);
        }
    }
}