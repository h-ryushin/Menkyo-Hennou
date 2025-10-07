using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownPanelManeger : MonoBehaviour
{
    int testCount = 1;
    public GameObject nextpanel;
    public GameObject currentpanel;
    public GameObject finalpanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartTest()
    {
        if (testCount == 3)
        {
            currentpanel.SetActive(false);
            finalpanel.SetActive(true);
        }
        else
        {
            testCount++;
            currentpanel.SetActive(false);
            nextpanel.SetActive(true);
        }

    }
}
