using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pres_img : MonoBehaviour
{
    SceneManage sceneManage;
    Game_Move gm;
    public bool pres;

    void Start()
    {
        sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
        gm = GameObject.Find("Main_Player").GetComponent<Game_Move>();
    }
}
