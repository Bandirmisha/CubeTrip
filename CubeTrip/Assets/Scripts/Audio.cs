using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip[] Music_clip;
    public AudioClip[] UI_clip;
    public AudioSource MusicSourse;
    SceneManage sceneManage;
    void Start()
    {
        sceneManage = GetComponent<SceneManage>();
        MusicSourse = GetComponent<AudioSource>();
        MusicSourse.clip = Music_clip[1];
    }
    void Update()
    {
        MusicSourse.clip = sceneManage.is_game ? Music_clip[0] : Music_clip[1];
    }
}
