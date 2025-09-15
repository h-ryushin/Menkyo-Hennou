using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour

{
    public string sceneName; // シーン名を指定するための変数
    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
