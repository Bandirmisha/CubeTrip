using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedInstantiate : MonoBehaviour
{
  static GameObject[] plts = new GameObject[30];
  static int lst = 0;
  [SerializeField] Game_Move gm;
  void Start()
  {
    for (int i = 0; i != 30; i++)
      plts[i] = Instantiate(gm.Ground_Block, new Vector3(0, -30, 0), Quaternion.identity);
  }
  public static GameObject F_Instantiate(Vector3 pos)
  {
    if (lst == 30) lst = 0;
    if (plts[lst].transform.Find("default Variant(Clone)"))
      Destroy(plts[lst].transform.Find("default Variant(Clone)").gameObject);
    if (plts[lst].transform.Find("Monetka Variant(Clone)"))
      Destroy(plts[lst].transform.Find("Monetka Variant(Clone)").gameObject);
    plts[lst].transform.position = pos;
    return plts[lst++];
  }
}
