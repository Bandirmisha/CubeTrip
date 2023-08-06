using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
  /*
  Итак, что це за зверь такой? Этот скрипт является компонентом для каждого блока.
  Он отвечает за то, чтобы создавать новые блоки, делается это ф-цией Inst.
  Так же, он двигает всю систему, а точнее каждый блок в одном направлении.
  Направление меняется тогда, когда появляется поворот, это замечает персонаж из главного скрипта
  */

  //Ссылка на оосновной скрипт
  Game_Move gm;
  //Координаты, на которых заспавнится новый блок
  Vector3 tmp;
  //true - движение прямо, false - движение вбок
  [SerializeField] GameObject money;
  public bool speedoff;
  Menu menu;
  float off = 1;
  void Start()
  {
    menu = GameObject.Find("UI").GetComponent<Menu>();
    gm = GameObject.FindGameObjectWithTag("Player").GetComponent<Game_Move>();
  }
  void Update()
  {
    //if dir - движения прямо, if !dir - движение налево
    if (gm.dir)
      transform.Translate(Vector3.back * (float)gm.speed * Time.fixedDeltaTime);
    else
    if (!gm.dir)
      transform.Translate(Vector3.right * (float)gm.speed * Time.fixedDeltaTime);

    if (speedoff)
    {
      if (gm.speed >= 0)
      {
        gm.speed -= 0.1f * gm.speed;
      }
      if (gm.speed <= 0.1)
      {
        gm.speed = 0;
        speedoff = false;
        menu.Dead();
      }
    }
  }
  //Предназначена для контролируемого ранодома, чтобы уменьшить количество резких поворотов
  int Direct(bool is_up, int chance)
  {
    if (is_up)
      gm.chance = chance;
    else
      gm.chance -= 1;
    return gm.chance;
  }

  //В процессе разработки, так что не смотри сюда
  void Objects_Gen(Vector3 pos, bool f)
  {
    GameObject tmp_lst;
    pos = new Vector3(pos.x, pos.y + 0.5f, pos.z);
    if (gm.trap_inst && f)
    {
      switch (Random.Range(0, Random.Range(2, 5)))
      {
        case 0:
          gm.trap_inst = false;
          Instantiate(gm.Traps[0], pos, Quaternion.identity).transform.SetParent(gm.Block_lst.transform);
          break;
        case 1:
          gm.trap_inst = false;
          tmp_lst = gm.Block_lst;
          Vector3 f_pos = new Vector3(gm.Block_lst.transform.position.x, gm.Block_lst.transform.position.y, gm.Block_lst.transform.position.z);
          gm.Block_lst = Instantiate(gm.Traps[1], f_pos, Quaternion.identity);
          if (gm.twist)
          {
            gm.Block_lst.transform.Find("mesh").Rotate(0, -90, 0);
            gm.Block_lst.transform.Find("mesh").position += new Vector3(-0.13f, 0, -0.13f);
          }
          off = 1.1f;
          tmp_lst.transform.position = new Vector3(0, -30, 0);
          Inst();
          off = 1;
          break;
      }
    }
    else
    {
      gm.trap_inst = true;
      if (Random.Range(0, 5) == 0)
        Instantiate(money, pos, Quaternion.identity).transform.SetParent(gm.Block_lst.transform);
    }
  }
  /*
  Выглядит страшно из-за того, что в процессе разработке у меня менялись идеи, как следует
  сделать поворот, но по факту тут все постороенно так:
  1) Есть единый вариант, когда у нас происходит поворот, чем дольше его не было, тем больше 
  шанс на его появления (см. ф-цию Direct)
  2) Если поворачиваем, то смотрим, куда шла система в предыдущий раз, и меняем ее направление
  3) В противном случае просто создаем блок вслед за предыдущем
  */
  public GameObject Inst()
  {
    var bl_Transform = gm.Block_lst.GetComponent<Transform>();
    switch (Random.Range(0, Direct(false, 18)))
    {
      case 0:
        Direct(true, 18);
        if (gm.twist)
        {
          gm.twist = false;
          tmp = new Vector3(bl_Transform.position.x - off, bl_Transform.position.y - 1, bl_Transform.position.z);
          gm.Block_lst = FixedInstantiate.F_Instantiate(tmp);
          bl_Transform = gm.Block_lst.GetComponent<Transform>();
          bl_Transform.Find("mesh").rotation = Quaternion.Euler(0, -90, 0);
          bl_Transform.Find("mesh").Find("Cube").gameObject.SetActive(true);
        }
        else if (!gm.twist)
        {
          gm.twist = true;
          tmp = new Vector3(bl_Transform.position.x, bl_Transform.position.y - 1, bl_Transform.position.z + off);
          gm.Block_lst = FixedInstantiate.F_Instantiate(tmp);
          bl_Transform = gm.Block_lst.GetComponent<Transform>();
          bl_Transform.Find("mesh").rotation = Quaternion.Euler(0, 0, 0);
          bl_Transform.Find("mesh").Find("Cube").gameObject.SetActive(true);

        }
        Objects_Gen(tmp, false);

        break;
      default:
        Direct(false, 18);
        if (gm.twist)
        {
          tmp = new Vector3(bl_Transform.position.x - off, bl_Transform.position.y - 1, bl_Transform.position.z);
          gm.Block_lst = FixedInstantiate.F_Instantiate(tmp);
          bl_Transform = gm.Block_lst.GetComponent<Transform>();
          bl_Transform.Find("mesh").rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
          tmp = new Vector3(bl_Transform.position.x, bl_Transform.position.y - 1, bl_Transform.position.z + off);
          gm.Block_lst = FixedInstantiate.F_Instantiate(tmp);
          bl_Transform = gm.Block_lst.GetComponent<Transform>();
          bl_Transform.Find("mesh").rotation = Quaternion.Euler(0, 0, 0);

        }
        Objects_Gen(tmp, true);
        break;
    }
    return null;
  }
}
