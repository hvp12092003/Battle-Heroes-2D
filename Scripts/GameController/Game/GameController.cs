using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using HVPUnityBase.Base.DesignPattern;

public class GameController : MonoBehaviour
{
    [Header("Attribute")]
    public float coinInMacth = 0, coinForMath;
    public bool startMatch = false, statusClient = false, statusMaster = false, canCreateHero = true;
    public float createCoinPerSecond;
    public float amountHeroInSite;
    public List<int> iD_Heroes;
    public List<bool> posHeroID;
    public Transform[] objPosHeroID;
    public int playerInRoom = 1;
    //public List<GameObject> heroInMatch;
    public Vector3 posCreateHeroLeft, posCreateHeroRight;
    [Header("==========")]

    private PhotonView photonView;
    private string tagHero, TagBarrack;

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    // Start is called before the first frame update
    private void Start()
    {
        iD_Heroes = new List<int>();
        posHeroID = new List<bool> { false, false, false, false, false };
        photonView = this.gameObject.GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!startMatch) return;
        coinForMath += createCoinPerSecond * Time.deltaTime * (1 + UData.Instance.dataLevelBarrack.level / 10);
        coinInMacth = (int)coinForMath;
    }

    public void StartMatch()
    {
        startMatch = true;
        PhotonNetwork.LoadLevel("Map1");
    }
    public void CreateHero(int index)
    {
        if (coinInMacth < UData.Instance.heroAttributes[index - 1].coin) return;

        GameObject hero = PhotonNetwork.Instantiate("Prefabs/Heroes/Hero" + index.ToString(), Vector3.zero, Quaternion.identity, 0);

        PhotonView photonView = hero.transform.Find("Body").GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            coinInMacth -= UData.Instance.heroAttributes[index - 1].coin;
            tagHero = "Left";
            hero.transform.position = posCreateHeroRight;
            photonView.RPC("EnterDataToHero", RpcTarget.All, UData.Instance.heroAttributes[index - 1].baseHP, UData.Instance.heroAttributes[index - 1].baseDamage, UData.Instance.heroAttributes[index - 1].speed, UData.Instance.heroAttributes[index - 1].baseTimeEffect, UData.Instance.heroAttributes[index - 1].baseCoolDownAttack, tagHero, posCreateHeroRight);
        }
        else
        {
            tagHero = "Right";
            hero.transform.position = posCreateHeroLeft;
            photonView.RPC("EnterDataToHero", RpcTarget.All, UData.Instance.heroAttributes[index - 1].baseHP, UData.Instance.heroAttributes[index - 1].baseDamage, UData.Instance.heroAttributes[index - 1].speed, UData.Instance.heroAttributes[index - 1].baseTimeEffect, UData.Instance.heroAttributes[index - 1].baseCoolDownAttack, tagHero, posCreateHeroLeft);
        }
    }
    public void SaveResult()
    {
        Debug.Log("Handle and Save Data");
    }
}
public class GAMECTL : SingletonMonoBehaviour<GameController> { }