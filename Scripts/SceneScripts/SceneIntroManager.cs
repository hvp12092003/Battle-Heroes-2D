using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HVPUnityBase.Base.DesignPattern;
using System.Linq;
public class SceneIntroManager : MonoBehaviour
{
    public Image imageLoading2;

    public GameObject[] imageTextLOADING;
    public float count, count2 = 0.3f, process;
    public int count3;
    public GameObject sceneLoading;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Update()
    {
        count -= Time.deltaTime;
        count2 -= Time.deltaTime;
        if (count <= 0)
        {
            if (count2 <= 0)
            {
                count2 = 0.1f;
                HandlerAnimationTextLOADING();
            }
        }
        if (imageLoading2.fillAmount < process)
        {
            imageLoading2.fillAmount += Time.deltaTime * 2;
            if (imageLoading2.fillAmount == 1) HandlerEndLoading();
        }

        switch (PhotonNetwork.NetworkClientState.ToString())
        {
            case "Authenticating":
                process = 0.2f;
                break;
            case "ConnectingToMasterServer":
                process = 0.4f;
                break;
            case "OnConnectedToMaster":
                process = 0.6f;
                break;
            case "JoiningLobby":
                process = 0.8f;
                break;
            case "JoinedLobby":
                process = 1f;
                break;
        }
    }
    public void HandlerAnimationTextLOADING()
    {
        Sequence sequence = DOTween.Sequence();

        float originalY = imageTextLOADING[count3].transform.position.y;

        sequence.Append(imageTextLOADING[count3].transform.DOMoveY(originalY + 0.5f, 0.2f))
                .Append(imageTextLOADING[count3].transform.DOMoveY(originalY, 0.2f))
                .SetLoops(1, LoopType.Restart);
        count3++;
        if (count3 == imageTextLOADING.Length)
        {
            count = 2;
            count3 = 0;
        }
    }
    public void HandlerEndLoading()
    {
        sceneLoading.SetActive(false);
        Debug.Log("HandlerEndLoading");
        Observer.Instance.Notify("TurnOnShop");
        Observer.Instance.Notify("TurnOnMainMenu");
        Observer.Instance.Notify("UpdateTextArenaP1");
        Observer.Instance.Notify("UpdateImageBarrackP1");
        GAMECTL.Instance.iD_Heroes = UData.Instance.userData.heroes.ToList();
        Debug.Log(GAMECTL.Instance.iD_Heroes[2]);
        Observer.Instance.Notify("UpdateUIHeroesP1");
        Observer.Instance.Notify("UpdateUIHeroesP1");
    }
}
