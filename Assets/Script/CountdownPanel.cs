using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownPanel : MonoBehaviour
{
    // ★★★ 連携に必要なPanelManagerへの参照を追加 ★★★
    public PanelManager panelManager; 
    
    // [Header("次に表示するパネル")] // NextPanelは使用しません
    // public GameObject nextPanel;  // PanelManagerが制御するため、この行は削除またはコメントアウト推奨

    [Header("カウントを表示するText")]
    public Text countdownText;

    [Header("カウント開始値（例:3）")]
    public int startCount = 3;

    void OnEnable()
    {
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

        // ★★★ カウント終了後、PanelManagerに次のパネルへの遷移を指示 ★★★
        if (panelManager != null) panelManager.NextPanel();
        
        // 自分を非表示にする
        gameObject.SetActive(false);
    }
}