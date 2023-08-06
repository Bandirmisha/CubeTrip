using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NewObjInShop : MonoBehaviour
{
    public int id;
    [SerializeField] GameObject textNew;
    [SerializeField] Vector3 rot;
    [SerializeField] Vector3 scale;
    GameObject newnew;
    void OnEnable()
    {

        if (PlayerPrefs.HasKey("New_Theme") && gameObject.tag == "Theme" &&
         id == PlayerPrefs.GetInt("New_Theme") &&
         newnew == null)
        {
            newnew = Instantiate(textNew, transform.position, Quaternion.identity);
            newnew.transform.localScale = scale;
            newnew.transform.rotation = Quaternion.Euler(rot);
            newnew.transform.parent = gameObject.transform;
            newnew.SetActive(true);
        }

        if (PlayerPrefs.HasKey("New_Cube") && gameObject.tag == "Cube" &&
        id == PlayerPrefs.GetInt("New_Cube") &&
         newnew == null)
        {
            newnew = Instantiate(textNew, transform.position, Quaternion.identity);
            newnew.transform.localScale = scale;
            newnew.transform.rotation = Quaternion.Euler(rot);
            newnew.transform.parent = gameObject.transform;
            newnew.SetActive(true);

            newnew.transform.localPosition = new Vector3(3, -1, -3);
        }
    }
}
