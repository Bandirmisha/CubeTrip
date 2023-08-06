using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
  /*
  О, мой самый любимый скрипт, я с ним дня 4 ебался (а то и больше)
  По названию понятно за что он отвечет
  */
  public Animator animator;
  Rigidbody rb;
  public Game_Move gm;
  //На земле ли персонаж?
  public bool is_ground;
  //В прыжке ли персонаж?
  public bool jump;
  SceneManage sceneManage;

  public static int JumpCountInOneGame;
  public static int JumpCountEver;

  void Start()
  {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
    sceneManage = GameObject.Find("Scene_Manage").GetComponent<SceneManage>();

    JumpCountInOneGame = 0;
  }

  void FixedUpdate()
  {
    if (sceneManage.is_game)
    {
      //Тут мы устанавливаем скорость проигрывания анимации прыжка, в зависимотсти от
      //скорости движения системы
      animator.SetFloat("speed", (float)((gm.speed / 4)));

      RaycastHit hit;
      //Выпускаем луч вниз, если засек землю, то он на земле, иначе в прыжке
      if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
      {
        is_ground = true;
      }
      else
      {
        is_ground = false;
        //jump = jump ? true : false;
      }
      //Костыльная хуйня, нужна, чтобы запустить прыжок
      if (Input.GetMouseButton(0) && sceneManage.is_game &&
         (Input.mousePosition.y / Screen.height) < 0.87f)
      {
        jump = true;
      }
      if (Input.GetMouseButtonDown(0) && sceneManage.is_game &&
   (Input.mousePosition.y / Screen.height) < 0.87f)
      {
        JumpCountEver++;
        JumpCountInOneGame++;
      }

      //В зависимости от направления движения системы будет проигрываться разная анимашка прыжка
      if (jump)
      {
        animator.SetBool("is_jump", true);
        if (gm.rot)
          animator.SetFloat("jump", 0);
        else
          animator.SetFloat("jump", 0.5f);
      }
      //То же самое и тут
      if (!Input.GetMouseButton(0) && is_ground)
      {
        animator.SetBool("is_jump", false);
        if (gm.rot)
          animator.SetFloat("Idle", 0);

        else
          animator.SetFloat("Idle", 1);
        jump = false;
      }
    }
  }
}



