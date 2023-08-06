using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
  public List<GameObject> quests;
  [SerializeField] int id;
  //Скорость скорллинга
  [SerializeField] float snapSpeed;
  //RectTransform объекта Scroll 
  [SerializeField] RectTransform ScrollobjRect;
  [SerializeField] ScrollRect scrollRect;
  [SerializeField] GameObject border1;
  [SerializeField] GameObject border2;

  Vector2 scrollVector;
  [SerializeField] string tmp;

  public bool[] savedQuests = new bool[25];
  bool is_scrolling;
  public void SaveQuest(GameObject quest)
  {
    int idSaved = quests.FindIndex(i => i == quest);
    savedQuests[idSaved] = true;
    string save = "";
    for (int i = 0; i != savedQuests.Length; i++)
    {
      if (savedQuests[i] == false) save += '0';
      else save += '1';
    }
    PlayerPrefs.SetString("Reward", save);
  }

}

