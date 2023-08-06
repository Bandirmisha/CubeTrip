using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
  [Range(1, 6)]
  public int id;
  public float value;
  public int reward;
  public bool complited;

  [SerializeField] SceneManage sceneManage;
  [SerializeField] Scroll scroll;
  [SerializeField] Menu menu;
  [SerializeField] Audio audioo;
  [SerializeField] GameObject Text;
  void Start()
  {
    sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
    menu = GameObject.Find("UI").GetComponent<Menu>();
    scroll = GameObject.Find("Scroll").GetComponent<Scroll>();

    
    transform.Find("Slider").Find("count").GetComponent<Text>().text = reward.ToString();

    if (complited)
    {
      Text.GetComponent<Text>().color = Color.white;
      gameObject.transform.Find("Slider").gameObject.SetActive(false);
      gameObject.transform.Find("completed").gameObject.SetActive(true);
      gameObject.transform.Find("GET REWARD").gameObject.SetActive(false);
    }
  }
  public void QuestComplited()
  {
    gameObject.transform.Find("Slider").gameObject.SetActive(false);
    gameObject.transform.Find("GET REWARD").gameObject.SetActive(true);
  }
  public void GetReward()
  {
    menu.SoundPlay(audioo.UI_clip[1]);
    menu.SoundPlay(audioo.UI_clip[3]);
    menu.notices.gameObject.transform.Find("notice_Coins").Find("Text").GetComponent<Text>().text = reward.ToString();
    menu.notices.SetFloat("notice", 2f);
    menu.notices.SetBool("noticeDone", false);

    gameObject.transform.Find("GET REWARD").gameObject.SetActive(false);
    Text.GetComponent<Text>().color = Color.white;
    
    sceneManage.money_bal += reward;
    gameObject.transform.Find("completed").gameObject.SetActive(true);

    scroll.SaveQuest(gameObject);
  }
}
