using UnityEngine;
using UnityEngine.Advertisements;
public class ADS : MonoBehaviour, IUnityAdsListener
{

  public void OnUnityAdsDidError(string message)
  {
    ruletka.gameObject.SetActive(false);
    ruletka.gameObject.SetActive(true);
  }
  public void OnUnityAdsDidStart(string placementId) { }
  public void OnUnityAdsReady(string placementId) { }
  public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
  {
    switch (showResult)
    {
      case ShowResult.Failed:
        ruletka.but.SetActive(true);
        break;
      case ShowResult.Skipped:
        ruletka.but.SetActive(true);
        break;
      case ShowResult.Finished:
        Debug.Log("All Done");
        RewardVideoReady(placementId);
        break;
    }
  }
  string gKey = "3540825";
  string[] placements = { "Banner", "video", "ruletkaReward" };
  [SerializeField] bool isTest;
  [SerializeField] Menu menu;
  [SerializeField] Ruletka ruletka;
  [SerializeField] Audio audioo;


  void Start()
  {
    Advertisement.AddListener(this);
    Advertisement.Initialize(gKey, isTest);

  }
  public void ShowVideo()
  {
    if (!Advertisement.IsReady(placements[1]))
      return;
    Advertisement.Show(placements[1]);
  }

  public void ShowRewardVideo()
  {
    if (!Advertisement.IsReady(placements[2]))
      return;
    Advertisement.Show(placements[2]);
  }
  public void RewardVideoReady(string name)
  {
    if (name == placements[2])
    {
      menu = GameObject.Find("UI").GetComponent<Menu>();
      ruletka = GameObject.Find("ruletka").GetComponent<Ruletka>();
      menu.SoundPlay(audioo.UI_clip[4]);
      ruletka.but.SetActive(false);
      ruletka.ads_tmp = true;
      ruletka.spin = true;
    }
  }
}