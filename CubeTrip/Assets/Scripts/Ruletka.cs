using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ruletka : MonoBehaviour
{
  // Приз, От какой точки, До какой точки, Имя лампы
  float[,] coordinates = {
     {0, 342.8f, 360, 0}, //+1 spin
     {1, 17.7f, 72.44f, -1f}, //+50 gold
     {2, 72.44f, 107.1f, 1}, //+cube
     {3, 107.1f, 161.95f, -1f}, //+25 gold
     {4, 161.95f, 197.15f, 3}, //?
     {1, 197.15f, 252.4f, -1f}, //+50 gold
     {5, 252.4f, 287.7f, 2}, //+theme
     {3, 287.7f, 342.8f, -1f}, //+25 gold
     };
  float min;
  [SerializeField] float speed = 0;
  [SerializeField] float acceleration;
  [SerializeField] Sprite lamp_on;
  [SerializeField] Sprite lamp_off;
  [SerializeField] Swipe_Cube swipe_Cube;
  [SerializeField] Swipe_Theme swipe_Theme;
  [SerializeField] Audio audioo;
  [SerializeField] Menu menu;
  [SerializeField] RTimer rTimer;
  public GameObject but;
  public GameObject back;
  [SerializeField] GameObject notice_Cube;
  [SerializeField] GameObject notice_Theme;
  ADS ads;
  public bool spin;
  bool f;
  bool goToNearest;
  public bool ads_tmp;
  SceneManage sceneManage;
  bool[] cubes;
  bool[] themes;
  public bool time;
  bool spin2;
  bool timerCostil;
  void Start()
  {
    sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
    ads = sceneManage.gameObject.GetComponent<ADS>();
    transform.Find("spin_img").Rotate(Vector3.forward * Random.Range(0, 180));
    ads_tmp = false;

  }
  public void PressButton()
  {
    if (!ads_tmp)
    {
      menu.SoundPlay(audioo.UI_clip[1]);
      time = false;
      rTimer.nowtime = System.TimeSpan.Parse("00:02:00");
      rTimer.block.SetActive(true);
      ads.ShowRewardVideo();
      back.SetActive(false);

      for (int i = 0; i <= 3; i++)
        transform.Find("spin_img").Find(i.ToString()).gameObject.GetComponent<Image>().sprite = lamp_off;
    }

  }

  void Update()
  {
    if (spin)
    {
      if (speed <= 25 && !f)
      {
        speed += acceleration * Time.deltaTime;
      }
      else if ((speed >= 25 || f) && speed >= 0)
      {
        speed -= acceleration * Time.deltaTime;
        f = true;
      }
      if (speed < 0)
      {
        speed = 0;
        spin = false;
        f = false;
        var zrot = transform.Find("spin_img").rotation.eulerAngles.z;
        print(zrot);
        if (zrot <= 17.7f)
        {
          Win(0);
          return;
        }
        for (int i = 0; i != 8; i++)
        {

          if (zrot >= coordinates[i, 1] && zrot <= coordinates[i, 2])
          {
            Debug.Log(coordinates[i, 0]);
            min = coordinates[i, 3];
            if (min >= 0)
              transform.Find("spin_img").Find(min.ToString()).gameObject.GetComponent<Image>().sprite = lamp_on;
            Win(i);
          }
        }
        goToNearest = true;
      }
      transform.Find("spin_img").Rotate(Vector3.forward * speed);
    }
  }
  void Win(int i)
  {
    if (spin2)
    {
      Debug.Log("Spin2");
      menu.SoundPlay(audioo.UI_clip[4]);
      spin2 = false;
      f = false;
      spin = true;
      ads_tmp = true;
      back.SetActive(false);
    }
    //menu.SoundPlay(audioo.UI_clip[5]);
    back.SetActive(true);
    bool r = false;
    switch (coordinates[i, 0])
    {
      case 0:
        Debug.Log("+SPIN");
        menu.SoundPlay(audioo.UI_clip[5]);
        menu.SoundPlay(audioo.UI_clip[4]);
        f = false;
        spin = true;
        ads_tmp = true;
        spin2 = true;
        timerCostil = true;
        back.SetActive(false);
        break;
      case 1:
        Debug.Log("+50");
        menu.notices.gameObject.transform.Find("notice_Coins").Find("Text").GetComponent<Text>().text = "50";
        menu.SoundPlay(audioo.UI_clip[2]);
        menu.notices.SetFloat("notice", 2f);
        menu.notices.SetBool("noticeDone", false);

        sceneManage.money_bal += 50;
        PlayerPrefs.SetInt("money", sceneManage.money_bal);
        StartCoroutine("TimerOff");
        break;
      case 2:
        Debug.Log("+CUBE");
        menu.SoundPlay(audioo.UI_clip[5]);
        //Тут мы получаем массив купленных кубов
        if (PlayerPrefs.HasKey("by_cube"))
        {
          string tmp = PlayerPrefs.GetString("by_cube");
          cubes = new bool[tmp.Length];
          for (int c = 0; c != tmp.Length; c++)
          {
            if (tmp[c] == '1' || c == 0)
              cubes[c] = true;
            else
              cubes[c] = false;
          }
        }
        else
        {
          cubes = new bool[swipe_Cube.bought_cube.Length];
          for (int ji = 0; ji != swipe_Cube.bought_cube.Length; ji++)
          {
            cubes[ji] = false;
          }
          cubes[0] = true;
          PlayerPrefs.SetString("by_cube", cubes.ToString());
        }

        print(cubes);
        //Проверка на то, что у пользователя есть некупленные кубы
        for (int j = 0; j != cubes.Length; j++)
        {
          if (!cubes[j])
          {
            r = true;
            RngCube();
            break;
          }
        }
        if (!r)
        {
          but.SetActive(false);
          menu.SoundPlay(audioo.UI_clip[4]);
          ads_tmp = true;
          spin = true;
        }
        break;
      case 3:
        Debug.Log("+25");
        menu.notices.gameObject.transform.Find("notice_Coins").Find("Text").GetComponent<Text>().text = "25";
        menu.SoundPlay(audioo.UI_clip[2]);
        menu.notices.SetFloat("notice", 2f);
        menu.notices.SetBool("noticeDone", false);
        sceneManage.money_bal += 25;
        PlayerPrefs.SetInt("money", sceneManage.money_bal);
        StartCoroutine("TimerOff");
        break;
      case 4:
        Debug.Log("WIN");
        menu.SoundPlay(audioo.UI_clip[5]);
        menu.SoundPlay(audioo.UI_clip[4]);
        Win(Random.Range(0, 6));
        break;
      case 5:
        Debug.Log("+THEME");
        menu.SoundPlay(audioo.UI_clip[5]);
        //Тут мы получаем массив купленных тем
        if (PlayerPrefs.HasKey("by_theme"))
        {
          string tmp = PlayerPrefs.GetString("by_theme");
          themes = new bool[tmp.Length];
          for (int c = 0; c != tmp.Length; c++)
          {
            if (tmp[c] == '1' || c == 0)
              themes[c] = true;
            else
              themes[c] = false;
          }

        }
        else
        {
          themes = new bool[swipe_Theme.bought_theme.Length];
          for (int ji = 0; ji != swipe_Theme.bought_theme.Length; ji++)
          {
            themes[ji] = false;
          }
          themes[0] = true;
          PlayerPrefs.SetString("by_theme", themes.ToString());
        }
        //Проверка на то, что у пользователя есть некупленные темы
        for (int j = 0; j != themes.Length; j++)
        {
          if (!themes[j])
          {
            r = true;
            RngTheme();
            break;
          }
        }
        if (!r)
        {
          but.SetActive(false);
          menu.SoundPlay(audioo.UI_clip[4]);
          ads_tmp = true;
          spin = true;
        }
        break;
      default:
        break;
    }

    ads_tmp = false;
  }
  void RngCube()
  {
    int ran = Random.Range(0, cubes.Length);
    print(ran);
    if (!cubes[ran])
    {
      menu.notices.SetFloat("notice", 1.5f);
      menu.notices.SetBool("noticeDone", false);

      cubes[ran] = true;
      string by = "";

      for (int i = 0; i != cubes.Length; i++)
      {
        if (cubes[i]) by += '1';
        else by += '0';
      }
      PlayerPrefs.SetString("by_cube", by);
      PlayerPrefs.SetInt("New_Cube", ran);

      print(by);
      StartCoroutine("TimerOff");

    }
    else RngCube();
  }

  void RngTheme()
  {
    int ran = Random.Range(0, themes.Length);
    print(ran);
    if (!themes[ran])
    {
      menu.notices.SetFloat("notice", 1f);
      menu.notices.SetBool("noticeDone", false);

      themes[ran] = true;
      string by = "";

      for (int i = 0; i != themes.Length; i++)
      {
        if (themes[i]) by += '1';
        else by += '0';
      }
      PlayerPrefs.SetString("by_theme", by);
            PlayerPrefs.SetInt("New_Theme", ran);

            print(by);
      StartCoroutine("TimerOff");

    }
    else RngTheme();
  }
  IEnumerator TimerOff()
  {
    if (timerCostil || spin2)
    {
      Debug.Log("TimerCostil");
      menu.SoundPlay(audioo.UI_clip[4]);
      timerCostil = false;
    }
    else
    {
      Debug.Log("Timer Start");
      yield return new WaitForSeconds(2);
      Debug.Log("Timer End");
      gameObject.SetActive(false);
      but.SetActive(true);
    }
  }
}
