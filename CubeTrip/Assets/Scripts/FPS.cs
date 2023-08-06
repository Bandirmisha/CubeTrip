using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
    void Awake()
    {
       // Application.targetFrameRate = 300;
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    void OnGUI() { GUILayout.Label("" + fps.ToString("f2")); }

    void Update()
    {
        frames++;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
