using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
  [SerializeField] Game_Move gm;
  Ray forward;
  Ray left;
  Ray back;
  Ray right;
  Vector3 find;
  RaycastHit hit_save;
  float tmp_speed;
  bool cet;

  void Start()
  {
    find = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
    forward = new Ray(find, Vector3.forward);
    left = new Ray(find, Vector3.left);
    back = new Ray(find, Vector3.back);
    right = new Ray(find, Vector3.right);
  }
  void Update()
  {

    RaycastHit hit;
    Ray ray;
    Ray rayback;
    ray = gm.dir ? forward : left;
    rayback = gm.dir ? back : right;
    Debug.DrawRay(ray.origin, ray.direction * 20, Color.green, 0.01f, true);
    if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Ground")
    {
      if (gm.dir)
      {
        if (Mathf.Abs(hit.transform.position.z) <= gm.speed / 50)
        {
          tmp_speed = (float)gm.speed;
          gm.speed = Mathf.Abs(hit.transform.position.z * 25);
          hit.collider.gameObject.SetActive(false);
          StartCoroutine("stop");
        }
      }
      else
      {
        if (Mathf.Abs(hit.transform.position.x) <= gm.speed / 50)
        {
          tmp_speed = (float)gm.speed;
          gm.speed = Mathf.Abs(hit.transform.position.x * 25);
          hit.collider.gameObject.SetActive(false);
          StartCoroutine("stop");
        }
      }
    }
    else if (Physics.Raycast(rayback, out hit) && hit.collider.tag == "Ground")
    {
      if (gm.dir)
      {
        tmp_speed = (float)gm.speed;
        gm.speed = -Mathf.Abs(hit.transform.position.z * 25);
        hit.collider.gameObject.SetActive(false);
        StartCoroutine("stop");
        print(hit.collider.gameObject.transform.position.z);
      }
      else
      {
        tmp_speed = (float)gm.speed;
        gm.speed = -Mathf.Abs(hit.transform.position.x * 25);
        hit.collider.gameObject.SetActive(false);
        StartCoroutine("stop");
        print(hit.collider.gameObject.transform.position.x);
      }
    }
  }
  IEnumerator stop()
  {
    yield return new WaitForEndOfFrame();
    gm.speed = tmp_speed;
    if (gm.dir)
    {
      gm.dir = false;
      gm.rot = false;
      transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    else
    {

      gm.dir = true;
      gm.rot = true;
      transform.rotation = new Quaternion(0, -90, 0, 90);
    }
    yield return 0;
  }
}
