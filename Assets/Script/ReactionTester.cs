using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionTester : MonoBehaviour
{
    public GameObject redSignal;
    public GameObject blueSignal;
    public GameObject resultPanel;
    public Text resultText;
    float waitTime;
    float timer = 0f;
    float reactionTime = 0f;
    float reactionTimeResult = 0f;
    // Start is called before the first frame update
    void Start()
    {
        redSignal.SetActive(true);
        blueSignal.SetActive(false);
        resultText.text = "";
        timer = 0f;
        waitTime = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            redSignal.SetActive(false);
            blueSignal.SetActive(true);
            reactionTime += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
            resultPanel.SetActive(true);
            reactionTimeResult = reactionTime;
            resultText.text = "反応時間: " + reactionTimeResult.ToString("f3") + " 秒";
        }
    }
}
