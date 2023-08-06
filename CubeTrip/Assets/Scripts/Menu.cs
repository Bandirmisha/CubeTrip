using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
  [SerializeField] SceneManage sceneManage;
  [SerializeField] Game_Move gm;

  public GameObject main; // Основной канвас со всем интерфейсом
  public GameObject shop; // Канвас магазина
                          //Открытый магазин 0 - кубы, 1 - темы
  public bool shop_type;
  [SerializeField] GameObject replay; //Канвас, вызывающийся после поражения
  [SerializeField] GameObject quests; //Канвас списка заданий
  [SerializeField] GameObject pause; //Канвас паузы
  public GameObject lightScene; // Свет сцены
  [SerializeField] GameObject lightUI; // Свет для магазина и списка заданий
  [SerializeField] GameObject lightPres; //Свет для освещения анимации монетки для темных тем
  [SerializeField] GameObject Shop_Cubes; //Спрайт, определяющий состояние кнопки магазина кубов
  [SerializeField] GameObject Shop_Themes; //Спрайт, определяющий состояние кнопки магазина тем
  [SerializeField] GameObject Swipe_Cubes; //Свайп кубов
  [SerializeField] GameObject Swipe_Themes; // Свайп тем
  [SerializeField] GameObject guide; //Окно туториала
  [SerializeField] GameObject pres_img;  //Спрайт подарка, анимация которого проигрывается после нажатия кнопки подарка
  [SerializeField] GameObject pres_but; //Кнопка подарка
  [SerializeField] GameObject dead; //СММЭЭЭЭРТЬ. ахем, экран смерти 
  [SerializeField] GameObject inGame;
  [SerializeField] GameObject Best;
  [SerializeField] GameObject Record;
  [SerializeField] GameObject present;
  [SerializeField] GameObject TimeForReturn;
  [SerializeField] GameObject ruletka;
  [SerializeField] GameObject ruletka_but;
  [SerializeField] Ruletka r;
  [SerializeField] GameObject TimeToPresent;
  [SerializeField] NowTime now;
  [SerializeField] Sprite[] s = new Sprite[4]; //Массив спрайтов звука
  [SerializeField] GameObject sound; //Кнопка звука
  [SerializeField] ADS ads;
  [SerializeField] Audio audioo;
  [SerializeField] RTimer rTimer;
  [SerializeField] AudioSource soundSourse;
  public Animator notices;
  private bool snd = true; //Проверка состояния кнопки звука
  SpriteState spriteState = new SpriteState();
  System.TimeSpan timer;
  double tmp_speed;
  public GameObject PAUSE;
   public GameObject resumeImg;

  int ScoreCountInTotal;
  public int CountGames;
  Quests questsMan;
  [SerializeField] Scroll scroll;
  void Awake()
  {
    //PlayerPrefs.DeleteAll();
    questsMan = GetComponent<Quests>();
    soundSourse = GetComponent<AudioSource>();
    spriteState = sound.GetComponent<Button>().spriteState;
    sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
    gm = GameObject.Find("Main_Player").GetComponent<Game_Move>();

    if (PlayerPrefs.HasKey("money"))
      sceneManage.money_bal = PlayerPrefs.GetInt("money");
    else
    {
      sceneManage.money_bal = 0;
      PlayerPrefs.SetInt("money", sceneManage.money_bal);
    }
    if (PlayerPrefs.HasKey("best"))
      gm.best = PlayerPrefs.GetInt("best");
    else
    {
      gm.best = 0;
      PlayerPrefs.SetInt("best", gm.best);
    }
    if (PlayerPrefs.HasKey("pres_bool"))
    {
      if (PlayerPrefs.GetInt("pres_bool") == 0)
      {
        present.SetActive(true);
        PlayerPrefs.SetInt("pres_bool", 1);
        sceneManage.money_bal += 100;
        PlayerPrefs.SetInt("money", sceneManage.money_bal);
      }
      else if (PlayerPrefs.GetInt("pres_bool") == 1)
        present.SetActive(false);
    }
    if (sceneManage.selected_theme_id >= 9 && sceneManage.selected_theme_id <= 12)
    {
      lightScene.SetActive(false);
      lightUI.SetActive(false);
      lightPres.SetActive(true);
      sceneManage.main_Material.EnableKeyword("_EMISSION");
      sceneManage.main_Material.SetColor("_EmissionColor", sceneManage.playerCol[sceneManage.selected_theme_id - 9]);
    }
    else
    {
      lightScene.SetActive(true);
      lightUI.SetActive(false);
      lightPres.SetActive(false);
      sceneManage.main_Material.DisableKeyword("_EMISSION");
    }
    if (AudioListener.volume == 1)
      sound.GetComponent<Image>().sprite = s[0];
    else
      sound.GetComponent<Image>().sprite = s[2];

    if (PlayerPrefs.HasKey("CountGames"))
      CountGames = PlayerPrefs.GetInt("CountGames");
    else
      CountGames = 0;

    if (PlayerPrefs.HasKey("snd"))
    {
      if (PlayerPrefs.GetFloat("snd") == 1)
      {
        AudioListener.volume = 1;
        sound.GetComponent<Image>().sprite = s[0];
      }
      else
      {
        AudioListener.volume = 0;
        sound.GetComponent<Image>().sprite = s[2];
      }
    }
    if (PlayerPrefs.HasKey("Reward"))
    {
      var tmp = PlayerPrefs.GetString("Reward");
      for (int i = 0; i != tmp.Length; i++)
      {
        if (tmp[i] == '0') scroll.savedQuests[i] = false;
        else scroll.savedQuests[i] = true;
      }
    }
    else
    {
      for (int i = 0; i != scroll.quests.Count; i++)
        scroll.savedQuests[i] = false;
    }
    if (PlayerPrefs.HasKey("ScoreCountInTotal"))
    {
      ScoreCountInTotal = PlayerPrefs.GetInt("ScoreCountInTotal");
    }
    else
    {
      ScoreCountInTotal = 0;
      PlayerPrefs.SetInt("ScoreCountInTotal", ScoreCountInTotal);
    }
    if (PlayerPrefs.HasKey("JumpCountInOneGame"))
      Jump.JumpCountInOneGame = PlayerPrefs.GetInt("JumpCountInOneGame");
    else
      Jump.JumpCountInOneGame = 0;

    if (PlayerPrefs.HasKey("JumpCountEver"))
      Jump.JumpCountEver = PlayerPrefs.GetInt("JumpCountEver");
    else
      Jump.JumpCountEver = 0;
    InitMissions();
  }
  void InitMissions()
  {
    for (int i = 0; i != scroll.quests.Count; i++)
    {
      var quest_i = scroll.quests[i].GetComponent<Quest>();
      if (scroll.savedQuests[scroll.quests.FindIndex(j => j == quest_i.gameObject)])
        quest_i.complited = true;
      switch (quest_i.id)
      {
        case 1:
          scroll.quests[i].transform.Find("Slider").GetComponent<Slider>().value =
          CountGames / quest_i.value;

          if (CountGames >= quest_i.value && !quest_i.complited)
            quest_i.QuestComplited();
          break;
        case 2:
          scroll.quests[i].transform.Find("Slider").GetComponent<Slider>().value =
          Jump.JumpCountEver / quest_i.value;

          if (Jump.JumpCountEver >= quest_i.value && !quest_i.complited)
            quest_i.QuestComplited();
          break;
        case 3:
          scroll.quests[i].transform.Find("Slider").GetComponent<Slider>().value =
          Jump.JumpCountInOneGame / quest_i.value;

          if (Jump.JumpCountInOneGame >= quest_i.value && !quest_i.complited)
            quest_i.QuestComplited();
          break;
        case 4:
          scroll.quests[i].transform.Find("Slider").GetComponent<Slider>().value =
          gm.best / quest_i.value;

          if (gm.best >= quest_i.value && !quest_i.complited)
            quest_i.QuestComplited();
          break;
        case 5:
          scroll.quests[i].transform.Find("Slider").GetComponent<Slider>().value =
          ScoreCountInTotal / quest_i.value;

          if (ScoreCountInTotal >= quest_i.value && !quest_i.complited)
            quest_i.QuestComplited();
          break;
        case 6:
          scroll.quests[i].transform.Find("Slider").GetComponent<Slider>().value =
          sceneManage.money_bal / quest_i.value;

          if (sceneManage.money_bal >= quest_i.value && !quest_i.complited)
            quest_i.QuestComplited();
          break;
      }
    }
  }
  public void SoundPlay(AudioClip _clip)
  {
    soundSourse = GetComponent<AudioSource>();
    soundSourse.clip = _clip;
    soundSourse.Play();
  }
  public void Sound()
  {
    notices.SetFloat("notice", 2f);

    AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
    SoundPlay(audioo.UI_clip[1]);
    snd = snd ? false : true;
    if (AudioListener.volume == 1)
      sound.GetComponent<Image>().sprite = s[0];
    else
      sound.GetComponent<Image>().sprite = s[2];
    PlayerPrefs.SetFloat("snd", AudioListener.volume);
  }
  void Update()
  {
    //Изменение спрайта кнопки звука
    if (AudioListener.volume == 1)
      spriteState.pressedSprite = s[1];
    else
      spriteState.pressedSprite = s[3];

    sound.GetComponent<Button>().spriteState = spriteState;
    //Нажатие на экран в определенной области приводит к запуску игры
    if (Input.GetMouseButtonDown(0) && main.activeSelf == true && ruletka.activeSelf == false && guide.activeSelf == false && Input.mousePosition.x >= Screen.width * 0.21f && Input.mousePosition.x <= Screen.width * 0.74f &&
        Input.mousePosition.y >= Screen.height * 0.173f && Input.mousePosition.y <= Screen.height * 0.865f && !present.activeSelf)
      sceneManage.GoToGame();

    if (pres_img.GetComponent<pres_img>().pres == true)
      present.SetActive(false);
  }
  public void GuideOpen() { SoundPlay(audioo.UI_clip[1]); guide.SetActive(true); }

  public void GuideClose() { SoundPlay(audioo.UI_clip[1]); guide.SetActive(false); }

  //конпка рулетки
  public void RulBut()
  {
    if (r.time)
    {
      SoundPlay(audioo.UI_clip[1]);
      ruletka.SetActive(true);
      //ads.ShowRewardVideo();
    }
  }

  // Открытие Магазина
  public void ShopBut()
  {
    SoundPlay(audioo.UI_clip[1]);
    main.SetActive(false);
    lightScene.SetActive(false);
    lightUI.SetActive(true);
    shop.SetActive(true);
    sceneManage.main_Material.DisableKeyword("_EMISSION");
  }
  // Открытие Списка заданий
  public void QuestsBut()
  {
    SoundPlay(audioo.UI_clip[1]);
    main.SetActive(false);
    lightScene.SetActive(false);
    lightUI.SetActive(true);
    quests.SetActive(true);

  }
  // Фукция кнопки назад. Универсальна для магазина и списка заданий
  public void BackToMain()
  {
    SoundPlay(audioo.UI_clip[1]);
    sceneManage.GoToMenu();
    main.SetActive(true);

    if (sceneManage.selected_theme_id >= 9 && sceneManage.selected_theme_id <= 12)
    {
      lightScene.SetActive(false);
      lightUI.SetActive(false);
      sceneManage.main_Material.EnableKeyword("_EMISSION");
      sceneManage.main_Material.SetColor("_EmissionColor", sceneManage.playerCol[sceneManage.selected_theme_id - 9]);
    }
    else
    {
      lightScene.SetActive(true);
      lightUI.SetActive(false);
      lightPres.SetActive(false);
    }

    quests.SetActive(false);
    shop.SetActive(false);
    ruletka.SetActive(false);
  }
  public void PresActive() { present.SetActive(true); TimeToPresent.SetActive(false); }
  // Кнопка подарка
  public void Present()
  {
    SoundPlay(audioo.UI_clip[1]);
    pres_img.SetActive(true);
    pres_but.SetActive(false);
    pres_img.GetComponent<Animation>().Play("Present");
    now.nowtime = System.TimeSpan.Parse("00:02:00");
    sceneManage.money_bal += 25;
    PlayerPrefs.SetInt("money", sceneManage.money_bal);
    StartCoroutine("WaitForPres");
    TimeToPresent.SetActive(true);
  }
  // Активировать магазин кубов
  public void ChangeCubes()
  {
    SoundPlay(audioo.UI_clip[1]);
    Shop_Cubes.SetActive(true);
    Shop_Themes.SetActive(false);
    shop_type = true;
  }
  // Активировать магазин тем
  public void ChangeThemes()
  {
    SoundPlay(audioo.UI_clip[1]);
    Shop_Cubes.SetActive(false);
    Shop_Themes.SetActive(true);
    shop_type = false;
  }
  // Переиграть
  public void Replay()
  {
    main.SetActive(true);
    replay.SetActive(false);
    PlayerPrefs.SetInt("money", sceneManage.money_bal);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    sceneManage.is_game = false;

  }
  //Пауза игры
  public void Pause()
  {
    Time.timeScale = 0;
    tmp_speed = gm.speed;
    gm.speed = 0;
    pause.SetActive(true);
  }
  // Возобновление игры
  public void Resume()
  {
    StartCoroutine("Timer");
    Time.timeScale = 0.001f;
    pause.SetActive(false);
  }
  public void BackToMenu()
  {
    Time.timeScale = 1;
    PlayerPrefs.SetInt("money", sceneManage.money_bal);
    PlayerPrefs.SetString("data", now.nowtime.ToString());
    sceneManage.is_game = false;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //Dead();
  }
  public void Dead()
  {
    dead.SetActive(true);
    PAUSE.SetActive(false);
    inGame.SetActive(false);
    gm.speed = 0;
    gm.gameObject.GetComponent<Jump>().jump = false;
    gm.gameObject.GetComponent<Jump>().animator.SetBool("is_jump", false);
    gm.GetComponent<AudioSource>().Play();
    PlayerPrefs.SetInt("money", sceneManage.money_bal);
    PlayerPrefs.SetString("data", now.nowtime.ToString());
    sceneManage.is_game = false;
    Record.GetComponent<Text>().text = gm.score.ToString();
    Best.GetComponent<Text>().text = gm.theBest().ToString();

    CountGames++;
    ScoreCountInTotal += gm.score;
    PlayerPrefs.SetInt("ScoreCountInTotal", ScoreCountInTotal);
    PlayerPrefs.SetInt("CountGames", CountGames);
    PlayerPrefs.SetInt("JumpCountEver", Jump.JumpCountEver);
    PlayerPrefs.SetInt("JumpCountInOneGame", Jump.JumpCountInOneGame);

    if (CountGames % 5 == 0)
    {
      ads.ShowVideo();
    }
    /*
        questsMan.QuestsUpdate(
          CountGames,
          Jump.JumpCountEver,
          Jump.JumpCountInOneGame,
          gm.best,
          ScoreCountInTotal,
          sceneManage.money_bal
        );
        */
    for (int i = 0; i != scroll.quests.Count; i++)
    {
      var quest_i = scroll.quests[i].GetComponent<Quest>();
      switch (quest_i.id)
      {
        case 1:
          if (CountGames >= quest_i.value && !scroll.savedQuests[i])
          {
            notices.SetFloat("notice", 0.5f);
            notices.SetBool("noticeDone", false);

            Debug.Log("Games: " + CountGames + i + " " + quest_i.complited);
          }
          break;
        case 2:
          if (Jump.JumpCountEver >= quest_i.value && !scroll.savedQuests[i])
          {
            notices.SetFloat("notice", 0.5f);
            notices.SetBool("noticeDone", false);

            Debug.Log("JumpEv: " + Jump.JumpCountEver + i + " " + quest_i.complited);
          }
          break;
        case 3:
          if (Jump.JumpCountInOneGame >= quest_i.value && !scroll.savedQuests[i])
          {
            notices.SetFloat("notice", 0.5f);
            notices.SetBool("noticeDone", false);

            Debug.Log("JumpIn: " + Jump.JumpCountInOneGame + i + " " + quest_i.complited);
          }
          break;
        case 4:
          if (gm.best >= quest_i.value && !scroll.savedQuests[i])
          {
            notices.SetFloat("notice", 0.5f);
            notices.SetBool("noticeDone", false);

            Debug.Log("Best: " + gm.best + i + " " + quest_i.complited);
          }
          break;
        case 5:
          if (ScoreCountInTotal >= quest_i.value && !scroll.savedQuests[i])
          {
            notices.SetFloat("notice", 0.5f);
            notices.SetBool("noticeDone", false);

            Debug.Log("Score: " + ScoreCountInTotal + " " + i + " " + quest_i.complited);
          }
          break;
        case 6:
          if (sceneManage.money_bal >= quest_i.value && !scroll.savedQuests[i])
          {
            notices.SetFloat("notice", 0.5f);
            notices.SetBool("noticeDone", false);

            Debug.Log("Money: " + sceneManage.money_bal + i + " " + quest_i.complited);
          }
          break;
      }
    }
  }
  IEnumerator Timer()
  {
    TimeForReturn.SetActive(true);
    resumeImg.SetActive(true);
    var time = TimeForReturn.GetComponent<Text>();
    time.text = "3";
    yield return new WaitForSeconds(0.001f);
    time.text = "2";
    yield return new WaitForSeconds(0.001f);
    time.text = "1";
    yield return new WaitForSeconds(0.001f);
    Time.timeScale = 1;
    gm.speed = tmp_speed;
    TimeForReturn.SetActive(false);
    resumeImg.SetActive(false);
  }
  IEnumerator WaitForPres()
  {
    yield return new WaitForSeconds(1.2f);
    SoundPlay(audioo.UI_clip[2]);
    yield return new WaitForSeconds(2.25f);
    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}

