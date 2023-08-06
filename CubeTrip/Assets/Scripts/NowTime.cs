using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NowTime : MonoBehaviour
{
  string data;
  public System.TimeSpan nowtime;
  SceneManage sceneManage;
  Menu menu;
  string zero_min;
  string zero_sec;
  [SerializeField]GameObject text;
  void Start()
  {
    sceneManage = GetComponent<SceneManage>();
    menu = GameObject.Find("UI").GetComponent<Menu>();
    if (PlayerPrefs.HasKey("data"))
    {
      data = PlayerPrefs.GetString("data");
      nowtime = System.TimeSpan.Parse(data);
    }
    else
    {
      nowtime = System.TimeSpan.Parse("00:05:00");
      PlayerPrefs.SetString("data", nowtime.ToString());
    }
  }
  void Update()
  {
          //nowtime = System.TimeSpan.Parse("00:00:01");

    if (nowtime.Minutes <= 0 && nowtime.Seconds <= 0 && !sceneManage.is_game)
      menu.PresActive();
    nowtime -= System.TimeSpan.FromSeconds(1 * Time.deltaTime);
    PlayerPrefs.SetString("data", nowtime.ToString());
    if(nowtime.Minutes <= 9) zero_min = "0";
    else zero_min = "";
    if(nowtime.Seconds <= 9) zero_sec = "0";
    else zero_sec = "";
    if(nowtime.Seconds >= 0)
    text.GetComponent<Text>().text = (zero_min + nowtime.Minutes.ToString() + ":" + zero_sec + nowtime.Seconds.ToString());
  }
}
