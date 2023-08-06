using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    SceneManage sceneManage;
    Menu menu;
    Game_Move gm;
    [SerializeField] Material particleMat;
    void Start()
    {
        sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
        menu = GameObject.Find("UI").GetComponent<Menu>();
        gm = GameObject.Find("Main_Player").GetComponent<Game_Move>();
        GetComponent<Renderer>().material = sceneManage.select_theme.GetComponent<Theme>().spike_mat;
        transform.Find("default").GetComponent<Renderer>().material = sceneManage.select_theme.GetComponent<Theme>().spike_mat;

        if (sceneManage.selected_theme_id >= 9 && sceneManage.selected_theme_id <= 12)
        {
         particleMat.EnableKeyword("_EMISSION");
         particleMat.SetColor("_EmissionColor", sceneManage.playerCol[sceneManage.selected_theme_id - 9]);
        }
        else
         particleMat.DisableKeyword("_EMISSION");
         
    }
    void OnTriggerEnter(Collider collider)
    {
        transform.Find("Particle_Death").gameObject.GetComponent<ParticleSystem>().Play();
        menu.Dead();
        gm.GetComponent<Collider>().enabled = false;
    }
}
