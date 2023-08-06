using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quests : MonoBehaviour
{
  /*Для каждого задания нужно ввести определенный счетчик, который будем получать напрямую из
  соответствующего скрипта:
  1) Сыграть n раз - счетчик будет обновляться при смерти и сохранять свое значение
  2) Прыгнуть n раз
  3) Прыгнуть n раз в одной игре
  4) Набрать n очков в одной игре
  5) Поставить рекорд в n очков
  6) Накопить n монет
  */

  [SerializeField] Scroll scroll;
  [SerializeField] List<GameObject> questsSlidersList;
  void Start()
  {
    for (int i = 0; i != scroll.quests.Count; i++)
    {
      questsSlidersList.Add(scroll.quests[i].transform.Find("Slider").gameObject);
    }
  }
  //При окончании игры будет вызываться этот метод
  public void QuestsUpdate(
      int ConutGames,
      int JumpCount,
      int JumpCountInOneGame,
      int ScoreCountInOneGame,
      int ScoreCountInTotal,
      int MoneyCount
  )
  {
    //Debug.Log(questsSlidersList[0].GetComponent<Slider>());
    
    switch (ConutGames)
    {
      case 1:

        break;
      case 10:

        break;
      case 25:

        break;
      case 100:

        break;
      case 250:

        break;

    }
    switch (JumpCount)
    {
      case 1:

        break;
      case 500:

        break;
      case 1000:

        break;
      case 2000:

        break;
    }
    switch (JumpCountInOneGame)
    {
      case 25:

        break;
      case 50:

        break;
      case 100:

        break;
      case 200:

        break;
    }
    switch (ScoreCountInOneGame)
    {
      case 50:

        break;
      case 100:

        break;
      case 200:

        break;
      case 300:

        break;
    }
    switch (ScoreCountInTotal)
    {
      case 1000:

        break;
      case 2500:

        break;
      case 5000:

        break;
      case 10000:

        break;
    }
    switch (MoneyCount)
    {
      case 10:

        break;
      case 150:

        break;
      case 500:

        break;
    }
  }
}
