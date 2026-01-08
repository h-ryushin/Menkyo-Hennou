using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReactionTester : MonoBehaviour
{
    public PanelManager panelManager; 
    
    public GameObject redSignal;
    public GameObject blueSignal;
    public GameObject resultPanel; 
    public GameObject otetukiPanel; // PanelManagerが制御するため使用しない
    public Text resultText;        // PanelManagerが制御するため使用しない
    
    float waitTime;
    float timer = 0f;
    float reactionTime = 0f;
    float reactionTimeResult = 0f;
    
    public float lastReactionTime = 0f; 

    void OnEnable()
    {
        redSignal.SetActive(true);
        blueSignal.SetActive(false);
        timer = 0f;
        reactionTime = 0f;
        lastReactionTime = 0f;
        waitTime = Random.Range(2f, 5f);
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= waitTime)
        {
            // 赤信号終了、青信号開始
            redSignal.SetActive(false);
            blueSignal.SetActive(true);
            reactionTime += Time.deltaTime; 
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                // 成功時の処理
                reactionTimeResult = reactionTime;
                lastReactionTime = reactionTimeResult; 
                
                redSignal.SetActive(false);
                blueSignal.SetActive(false);
                this.gameObject.SetActive(false);
                
                // PanelManagerに成功を通知
                if (panelManager != null) panelManager.HandleSuccess(reactionTimeResult);
            }
        }
        else if (timer < waitTime && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            // お手つき時の処理
            lastReactionTime = -1f; // お手つきを記録
            
            redSignal.SetActive(false);
            blueSignal.SetActive(false);
            this.gameObject.SetActive(false);
            
            // PanelManagerにお手つきを通知
            if (panelManager != null) panelManager.HandleOtetuki();
        }
    }
}