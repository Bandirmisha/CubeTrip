using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
  Game_Move gm;
  void Start() { gm = GameObject.FindGameObjectWithTag("Player").GetComponent<Game_Move>(); }
  void OnCollisionEnter(Collision collision)
  {
    gm.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 600);
    Destroy(gm.gameObject.transform.Find("Particle_Fall"));
    gm.gameObject.GetComponent<Animator>().enabled = false;
    gm.gameObject.GetComponent<Collider>().enabled = false;
    gameObject.transform.parent.GetComponent<Move>().speedoff = true;
  }
}
