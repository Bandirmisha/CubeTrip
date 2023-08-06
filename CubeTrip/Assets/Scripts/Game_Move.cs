using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Move : MonoBehaviour
{

  /*
  Это основной скрипт, является компонентом нашего персонажа.
  Он регистрирует поворот системы и отдает указания скрипту Move.
  Когда триггер, находящийся на границе каждого блока задевает персонажа, скрипт получает 
  объект блока, сохраняя в переменную Block_tmp. 
  В случае необходимости отдает команду Move'у, чтобы начать двигать систему в другом направлении.
  */
  Menu menu;
  public GameObject bg1;
  [SerializeField] GameObject score_txt;
  [SerializeField] Move Move_blockLast;
  SceneManage sceneManage;
  //Префаб блока земли (Ground)
  public GameObject Ground_Block;
  //Последний блок
  public GameObject Block_lst;
  //Массив ловушек
  public List<GameObject> Traps;
  //public GameObject trap;
  //Блок, на котором находится персонаж
  GameObject Block_tmp;
  GameObject lst_Ground;
  Jump jump;
  //Количество очков
  public int score;
  public int best;
  //public int score_pres = 0;
  //Скорость движения системы
  public double speed;
  //Переменная, которая используется в скрипте Move для контролируемого рандома при смене направения
  public int chance;
  //Костыль юзается для смены направления, работает - НЕ ТРОГАЙ БЛЯТЬ 
  public bool twist = true;
  //Направление движения системы 
  public bool dir = true;
  //Херня для контроля рандома при генерации ловушек
  public bool trap_inst;
  //Переменная, необходимая для разворота куба при повороте 
  //(есть теория, что это фиктивная переменная, но мне похуй, я панк)
  public bool rot = true;
  //Счетчик времени
  double value = 6;
  Camera cam;
  [SerializeField] InitPlayServices init;
  void Start()
  {
    Move_blockLast = Block_lst.GetComponent<Move>();
    menu = GameObject.Find("UI").GetComponent<Menu>();
    jump = GetComponent<Jump>();
    sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();
    cam = GameObject.Find("Camera").GetComponent<Camera>();
    cam.orthographicSize = 5;

  }
  void Update()
  {
    if (sceneManage.is_game)
    {
      //Херзнает сколько отнимем каждый кадр, спустя некоторое время значение переменной обнулится и сработвет условие 
      value -= Time.deltaTime * speed * 6;
      if (value < 0)
      {
        if (speed < 6) speed += 0.03f;
        score++;

        value = 5;
        //Создаем новый блок
        if (Block_lst.transform.position.x >= -10 && Block_lst.transform.position.z <= 10)
          Move_blockLast.Inst();
      }
      if (cam.orthographicSize < 7)
      {
        cam.orthographicSize += 0.001f;
      }
      Block_lst.transform.position = Vector3.Lerp(Block_lst.transform.position, new Vector3(Block_lst.transform.position.x, 0, Block_lst.transform.position.z), (float)(0.2 * speed));
      score_txt.GetComponent<Text>().text = score.ToString();
    }

  }
  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Ground" && lst_Ground != other.gameObject)
    {
      Block_tmp = other.gameObject;
      lst_Ground = Block_tmp;
    }
  }
  public int theBest()
  {
    best = score > best ? best = score : best;
    PlayerPrefs.SetInt("best", best);
    init.GetBestRecord(best);
    return best;
  }
}

