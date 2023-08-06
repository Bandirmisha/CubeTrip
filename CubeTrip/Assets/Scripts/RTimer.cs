using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTimer : MonoBehaviour
{
  string data;
  public System.TimeSpan nowtime;
  public bool time;
  private Text txt;
  [SerializeField] SceneManage sceneManage;
  string zero_min;
  string zero_sec;
  [SerializeField] GameObject text;
  [SerializeField] Ruletka ruletka;
  public GameObject block;
  void Start()
  {
    if (ruletka.time)
      block.SetActive(false);
    else
      block.SetActive(true);

    txt = GetComponent<Text>();
    if (PlayerPrefs.HasKey("Rdata"))
    {
      data = PlayerPrefs.GetString("Rdata");
      nowtime = System.TimeSpan.Parse(data);
    }
    else
    {
      ruletka.time = true;
      nowtime = System.TimeSpan.Parse("00:00:00");
      PlayerPrefs.SetString("Rdata", nowtime.ToString());
    }
  }
  void Update()
  {
   // nowtime = System.TimeSpan.Parse("00:00:00");
    if (nowtime.Minutes <= 0 && nowtime.Seconds <= 0 && !sceneManage.is_game)
    {
      ruletka.time = true;
      text.GetComponent<Text>().text = " ";
      block.SetActive(false);
      //nowtime = System.TimeSpan.Parse("00:00:00")
    }
    nowtime -= System.TimeSpan.FromSeconds(1 * Time.deltaTime);
    PlayerPrefs.SetString("Rdata", nowtime.ToString());
    if (nowtime.Minutes <= 9) zero_min = "0";
    else zero_min = "";
    if (nowtime.Seconds <= 9) zero_sec = "0";
    else zero_sec = "";
    if (nowtime.Seconds >= 0)
      text.GetComponent<Text>().text = (zero_min + nowtime.Minutes.ToString() + ":" + zero_sec + nowtime.Seconds.ToString());
  }
}
