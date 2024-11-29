using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HVPUnityBase.Base.DesignPattern;
using System;
using ExitGames.Client.Photon;
using GameNamespace;
using System.Reflection;
using Photon.Realtime;

public class GameController : MonoBehaviour, IOnEventCallback
{
    [Header("Attribute")]
    public float coinInMacth = 0;
    public bool startMatch = false, statusClient = false, statusMaster = false;
    public float createCoinPerSecond;
    public float amountHeroInSite;
    public List<float> choiceHeroID;
    public List<GameObject> heroInMatch;
    public Vector3 posBarraclLeft, posBarraclRight;
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
        Application.targetFrameRate = 144;
        choiceHeroID = new List<float>();
        photonView = this.gameObject.GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!startMatch) return;
        coinInMacth += createCoinPerSecond * Time.deltaTime;
    }
    public void SetUpMatch()
    {
        createCoinPerSecond = UData.Instance.barrackAttribute.amountCoinCreatePerSecond;
        amountHeroInSite = UData.Instance.barrackAttribute.amountHero;
        CreateBarrack(UData.Instance.dataLevelBarrack.level);
    }
    public void StartMatch()
    {
        SetUpMatch();
        startMatch = true;
    }
    public void CreateHero(int index)
    {
        GameObject hero = PhotonNetwork.Instantiate("Prefabs/Heroes/Hero" + index.ToString(), Vector3.zero, Quaternion.identity, 0);

        PhotonView photonView = hero.transform.Find("Body").GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            tagHero = "Right";
            hero.transform.position = posCreateHeroRight;
            photonView.RPC("EnterDataToHero", RpcTarget.All, UData.Instance.heroAttributes[index - 1].baseHP, UData.Instance.heroAttributes[index - 1].baseDamage, UData.Instance.heroAttributes[index - 1].speed, UData.Instance.heroAttributes[index - 1].baseTimeEffect, UData.Instance.heroAttributes[index - 1].baseCoolDownAttack, tagHero, posCreateHeroRight);
        }
        else
        {
            tagHero = "Left";
            hero.transform.position = posCreateHeroLeft;
            photonView.RPC("EnterDataToHero", RpcTarget.All, UData.Instance.heroAttributes[index - 1].baseHP, UData.Instance.heroAttributes[index - 1].baseDamage, UData.Instance.heroAttributes[index - 1].speed, UData.Instance.heroAttributes[index - 1].baseTimeEffect, UData.Instance.heroAttributes[index - 1].baseCoolDownAttack, tagHero, posCreateHeroLeft);
        }
    }
    public void CreateBarrack(int index)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject barrack = PhotonNetwork.Instantiate("Prefabs/Barrack/BarrackLV" + index.ToString(), posBarraclRight, Quaternion.identity, 0);
            PhotonView photonView = barrack.transform.Find("Body").GetComponent<PhotonView>();
            TagBarrack = "BarrackRight";
            Vector3 localScaleBarrack = new Vector3(barrack.transform.localScale.x * -1, barrack.transform.localScale.y, barrack.transform.localScale.z);
            barrack.transform.position = posBarraclRight;
            photonView.RPC("EnterDataToBarrack", RpcTarget.All, UData.Instance.barrackAttribute.baseHP, UData.Instance.barrackAttribute.amountHero, UData.Instance.barrackAttribute.amountHero, TagBarrack, localScaleBarrack);
        }
        else
        {
            GameObject barrack = PhotonNetwork.Instantiate("Prefabs/Barrack/BarrackLV" + index.ToString(), posBarraclLeft, Quaternion.identity, 0);
            PhotonView photonView = barrack.transform.Find("Body").GetComponent<PhotonView>();
            TagBarrack = "BarrackLeft";
            barrack.transform.position = posBarraclLeft;
            photonView.RPC("EnterDataToBarrack", RpcTarget.All, UData.Instance.barrackAttribute.baseHP, UData.Instance.barrackAttribute.amountHero, UData.Instance.barrackAttribute.amountHero, TagBarrack, barrack.transform.localScale);
        }
    }
    public void OnEvent(EventData photonEvent)
    {
        Debug.Log($"OnEvent {photonEvent.Code}");
        byte eventCode = photonEvent.Code;


        if (eventCode == 1)//mess
        {
            string even = (string)photonEvent.CustomData;
            even = (string)photonEvent.CustomData;
            Debug.Log($"Received message: {even}");
        }
        else if (eventCode == 2)//ready
        {
            string even = (string)photonEvent.CustomData;
            if (even == "Master")
            {
                if (statusMaster) statusMaster = false;
                else statusMaster = true;
            }
            else
            {
                if (statusClient) statusClient = false;
                else statusClient = true;
            }
            if (statusMaster && statusClient) StartMatch();
        }
        else if (eventCode == 3)//EndMacth
        {
            string even = (string)photonEvent.CustomData;
            if (even == "Client" && PhotonNetwork.IsMasterClient || even == "Master" && !PhotonNetwork.IsMasterClient)
            {
                OnEndMatchWin();
            }
            else
            {
                OnEndMatchLost();
            }
        }
    }
    public void OnEndMatchWin()
    {

    }
    public void OnEndMatchLost()
    {

    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
public class GAMECTL : SingletonMonoBehaviour<GameController> { }