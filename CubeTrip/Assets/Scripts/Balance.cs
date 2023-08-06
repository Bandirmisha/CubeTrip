using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balance : MonoBehaviour
{
    SceneManage sceneManage;
    void Start()
    {
        sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
    }
    void Update()
    {
        GetComponent<Text>().text = sceneManage.money_bal.ToString();
    }
}
