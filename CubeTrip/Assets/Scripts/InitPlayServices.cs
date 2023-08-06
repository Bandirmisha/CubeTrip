using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class InitPlayServices : MonoBehaviour
{
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
 
        if(!Social.localUser.authenticated) SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate((bool success) =>{});

    }
    public void GetBestRecord(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_score, (bool success) => {});
    }

    public void ShowBestRecord()
    {
        Social.ShowLeaderboardUI();
    }
    public void OnApplicationQuit()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
}
