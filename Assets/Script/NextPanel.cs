using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPanel : MonoBehaviour
{
    public GameObject nextpanel;
    // Start is called before the first frame update
    public void Next()
    {
        this.gameObject.SetActive(false);
        nextpanel.SetActive(true);
    }
}
