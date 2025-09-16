using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownPanel : MonoBehaviour
{
    [Header("次に表示するパネル")]
    public GameObject nextPanel;

    [Header("カウントを表示するText")]
    public Text countdownText;

    [Header("カウント開始値（例:3）")]
    public int startCount = 3;

    void OnEnable()
    {
        // このパネルが SetActive(true) になった瞬間にカウント開始
        StartCountdown();
    }

    public void StartCountdown()
    {
        StartCoroutine(DoCountdown());
    }

    private IEnumerator DoCountdown()
    {
        int count = startCount;

        // 3, 2, 1 と表示
        while (count > 0)
        {
            if (countdownText) countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        // 表示を消す
        if (countdownText) countdownText.text = "";

        // 次のパネルを有効化して、自分を非表示にする
        if (nextPanel) nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}