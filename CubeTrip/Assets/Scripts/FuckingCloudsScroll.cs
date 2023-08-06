using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuckingCloudsScroll : MonoBehaviour
{
    public float scroll_speed = 0.1f;
    [SerializeField] RectTransform Quad;
    [SerializeField] float endPos;
    [SerializeField] Vector3 pos = new Vector3(0, 0f, 5000);

    void Update()
    {
        Quad.Translate(Vector3.right * scroll_speed * Time.deltaTime);
        if (Quad.anchoredPosition.x >= endPos)
        {
            Quad.anchoredPosition = pos;// + new Vector3(0, Quads[id = id == 0 ? 1 : 0].anchoredPosition.y, 0);
        }
    }
}
