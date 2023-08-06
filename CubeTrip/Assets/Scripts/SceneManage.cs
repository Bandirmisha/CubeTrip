using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
  //Все скины кубов, из этих массивов будем загружать инфу
  public List<GameObject> all_Cubes;
  //Все темы
  public List<GameObject> all_Themes;
  //Мэш игрока, тобишь скин куба
  public Mesh main_Mesh; //s
                         //Материал для мэша
  public Material main_Material; //s
  public GameObject select_theme; //s
                                  //Номер выбранного скина 
  public int selected_cube_id; //s
                               //Номер выбранной темы
  public int selected_theme_id; //s
                                //Баланс денег
  public int money_bal; //s
                        //Играем ли мы или находимся в меню
  public bool is_game;
  GameObject balance;
  //Ссылка на пользовательский инт
  public GameObject ui;
  [SerializeField] Game_Move gm;
  //Скины
  GameObject[] blocks;
  Jump jump;
  bool timeoff;
  public Color[] playerCol;
  public Color[] monCol;
  [SerializeField] Menu menu;
  Audio _audio;

  [SerializeField] GameObject govno;
  [SerializeField] GameObject bg;
  void Awake()
  {
    Application.targetFrameRate = 60;
    jump = GameObject.Find("Main_Player").GetComponent<Jump>();
    ui = GameObject.Find("UI");
    _audio = GetComponent<Audio>();

    LoadInf();
    InstSPole();
    Costil();
    
    if (selected_theme_id >= 9 && selected_theme_id <= 12)
    {
      menu.lightScene.SetActive(false);
      main_Material.EnableKeyword("_EMISSION");
      main_Material.SetColor("_EmissionColor", playerCol[selected_theme_id - 9]);
    }
    else  main_Material.DisableKeyword("_EMISSION");
     
  }
  void LoadInf()
  {
    if (PlayerPrefs.HasKey("selected_theme_id"))
    {
      selected_theme_id = PlayerPrefs.GetInt("selected_theme_id");
      select_theme = all_Themes[selected_theme_id];
    }
    else
    {
      selected_theme_id = 0;
      select_theme = all_Themes[selected_theme_id];
      PlayerPrefs.SetInt("selected_theme_id", selected_theme_id);
    }
      Debug.Log(select_theme);
      Debug.Log(gm);
    if (PlayerPrefs.HasKey("selected_cube_id"))
    {
      selected_cube_id = PlayerPrefs.GetInt("selected_cube_id");
      main_Mesh = all_Cubes[selected_cube_id].GetComponent<MeshFilter>().mesh;
      main_Material = all_Cubes[selected_cube_id].GetComponent<Renderer>().material;
      
    }
    else
    {
      selected_cube_id = 0;
      main_Mesh = all_Cubes[selected_cube_id].GetComponent<MeshFilter>().mesh;
      main_Material = all_Cubes[selected_cube_id].GetComponent<Renderer>().material;
      PlayerPrefs.SetInt("selected_cube_id", selected_cube_id);
    }

  }

  public void Costil()
  {
    //Вот я считаю, что за такое нужно пиздить, но я ТАК ЗАЕБАЛСЯ это кодить, что мне можно
    if (selected_theme_id == 5)
    {
      govno.SetActive(true);
      bg.SetActive(false);
    }
    else
    {
      govno.SetActive(false);
      bg.SetActive(true);
    }

  }
  public void GoToMenu()
  {
    //_audio.MusicSourse.clip = _audio.Music_clip[1];
    //_audio.MusicSourse.Play();
    PlayerPrefs.SetInt("money", money_bal);
    LoadInf();
    InstSPole();
    Costil();
    is_game = false;
  }
  void Update()
  {
    if (timeoff)
    {
      ui.transform.Find("InGame").gameObject.SetActive(true);
      ui.transform.Find("main").gameObject.SetActive(false);
      timeoff = false;
    }

  }
  public void GoToGame()
  {
    is_game = true;
    gm.speed = 2.45;
    gm.twist = false;
    gm.dir = true;
    gm.rot = true;
    _audio.MusicSourse.clip = _audio.Music_clip[0];
    _audio.MusicSourse.Play();
    StartCoroutine("Timer");
  }
  //Создание первых блоков
  void InstSPole()
  {
    gm.gameObject.GetComponent<MeshFilter>().mesh = main_Mesh;
    gm.gameObject.GetComponent<Renderer>().material = main_Material;
    gm.Ground_Block.transform.Find("mesh").gameObject.GetComponent<Renderer>().material = select_theme.GetComponent<Theme>().plt_mat;
    gm.bg1.GetComponent<Image>().sprite = select_theme.GetComponent<Theme>().bg_mat;
    gm.gameObject.transform.Find("Particle_Fall").gameObject.GetComponent<Renderer>().material = select_theme.GetComponent<Theme>().plt_mat;

    GameObject[] bl = GameObject.FindGameObjectsWithTag("Block");
    if (bl.Length > 0)
    {
      for (int i = 0; i != bl.Length; i++)
        bl[i].transform.Find("mesh").gameObject.GetComponent<Renderer>().material = select_theme.GetComponent<Theme>().plt_mat;
    }
    else
    {
      for (int i = 0; i != 7; i++)
        Instantiate(gm.Ground_Block, new Vector3(0, 0, i), Quaternion.identity);
      blocks = GameObject.FindGameObjectsWithTag("Block");
      gm.Block_lst = blocks[blocks.Length - 1];
    }
  }
  void OnApplicationPause(bool pause)
  {
    if (pause && is_game && menu.PAUSE.activeSelf)
      menu.Pause();
  }
  IEnumerator Timer()
  {
    ui.transform.Find("main").gameObject.GetComponent<Animation>().Play("MainOFF");
    yield return new WaitForSeconds(0.25f);
    timeoff = true;
  }
}
