using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryTestmanager : MonoBehaviour
{
    public GameObject[] memoryTestPanelArray;
    public GameObject anserPanel;
    float timer = 30;
    public Text timerText;
    int currentPanelIndex = 0;
    public Button leftButton;
    public Button rightButton;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log($"Left={leftButton}, Right={rightButton}");
        leftButton.onClick.AddListener(GoLeftPanel);
        rightButton.onClick.AddListener(GoRightPanel);
        memoryTestPanelArray[currentPanelIndex].SetActive(true);
        leftButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("f0");
        if (timer <= 0)
        {
            anserPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }
        // else
        // {

        // }
        if (currentPanelIndex == 3)
        {
            rightButton.gameObject.SetActive(false);
        }
        if (currentPanelIndex == 0)
        {
            leftButton.gameObject.SetActive(false);
        }

    }
    void GoLeftPanel()
    {
        Debug.Log($"左ボタン押された！ 現在: {currentPanelIndex}");
        if(currentPanelIndex != 3)
        {
            rightButton.gameObject.SetActive(true);
        }
        memoryTestPanelArray[currentPanelIndex].SetActive(false);
        currentPanelIndex--;
        memoryTestPanelArray[currentPanelIndex].SetActive(true);
    }
    void GoRightPanel()
    {
        Debug.Log($"右ボタン押された！ 現在: {currentPanelIndex}");
        if(currentPanelIndex != 0)
        {
            leftButton.gameObject.SetActive(true);
        }
        memoryTestPanelArray[currentPanelIndex].SetActive(false);
        currentPanelIndex++;
        memoryTestPanelArray[currentPanelIndex].SetActive(true);
    }
}
