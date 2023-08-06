using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonetkaRotation : MonoBehaviour
{
  [SerializeField] float speed = 1f;
  SceneManage sceneManage;
  Game_Move gm;
  [SerializeField] GameObject mesh;
  [SerializeField] Material particleMat;

  void Start()
  {
    sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
    gm = GameObject.Find("Main_Player").GetComponent<Game_Move>();
    

    if (sceneManage.selected_theme_id >= 9 && sceneManage.selected_theme_id <= 12)
    {
      mesh.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
      mesh.GetComponent<Renderer>().material.SetColor("_EmissionColor", sceneManage.monCol[sceneManage.selected_theme_id - 9]);
      particleMat.EnableKeyword("_EMISSION");
      particleMat.SetColor("_EmissionColor", sceneManage.monCol[sceneManage.selected_theme_id - 9]);
    }
    else
    {
      mesh.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
      particleMat.DisableKeyword("_EMISSION");
    }   
  }
  void Update()
  {
    mesh.transform.Rotate(0, 360 * Time.deltaTime * speed, 0);
  }
  void OnTriggerEnter(Collider collider)
  {
    GetComponent<AudioSource>().Play();
    sceneManage.money_bal+=1;
    transform.Find("Particle_Monetka").gameObject.GetComponent<ParticleSystem>().Play();
    transform.Find("default").gameObject.SetActive(false);
    StartCoroutine("Timer");
  }
  IEnumerator Timer()
  {
    yield return new WaitForSeconds(0.4f);
    Destroy(gameObject);
  }
}
