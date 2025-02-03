using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class MatchSetup : MonoBehaviour
{
    public MoveCamByScrollbarr moveCamByScrollbarr;

    public Vector3 posBarraclLeft, posBarraclRight;
    private string tagHero, TagBarrack;
    public TextMeshProUGUI[] textCoinHero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* temp = Resources.Load<GameObject>("Prefabs/HP_Barrack_UI/HP_Barrack_Red");*/
        CreateBarrack(UData.Instance.dataLevelBarrack.level);



        //GAMECTL.Instance.heroIDMine = GAMECTL.Instance.id_Hero_Choise.ToList();
        GAMECTL.Instance.createCoinPerSecond = UData.Instance.barrackAttribute.amountCoinCreatePerSecond;
        GAMECTL.Instance.amountHeroInSite = UData.Instance.barrackAttribute.amountHero;



        if (PhotonNetwork.IsMasterClient) return;
        moveCamByScrollbarr = this.GetComponent<MoveCamByScrollbarr>();
        moveCamByScrollbarr.scrollbar.value = 1;
        moveCamByScrollbarr.Move();


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreateBarrack(int index)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject barrack = PhotonNetwork.Instantiate("Prefabs/Barrack/BarrackLV" + index.ToString(), posBarraclLeft, Quaternion.identity, 0);
            PhotonView photonView = barrack.transform.Find("Body").GetComponent<PhotonView>();
            barrack.transform.position = posBarraclLeft;
            TagBarrack = "BarrackLeft";
            photonView.RPC("EnterDataToBarrack", RpcTarget.All, UData.Instance.barrackAttribute.baseHP, UData.Instance.barrackAttribute.amountHero, UData.Instance.barrackAttribute.amountHero, TagBarrack, barrack.transform.localScale);
        }
        else
        {
            GameObject barrack = PhotonNetwork.Instantiate("Prefabs/Barrack/BarrackLV" + index.ToString(), posBarraclRight, Quaternion.identity, 0);
            PhotonView photonView = barrack.transform.Find("Body").GetComponent<PhotonView>();
            Vector3 localScaleBarrack = new Vector3(barrack.transform.localScale.x * -1, barrack.transform.localScale.y, barrack.transform.localScale.z);
            TagBarrack = "BarrackRight";
            barrack.transform.position = posBarraclRight;
            photonView.RPC("EnterDataToBarrack", RpcTarget.All, UData.Instance.barrackAttribute.baseHP, UData.Instance.barrackAttribute.amountHero, UData.Instance.barrackAttribute.amountHero, TagBarrack, localScaleBarrack);
        }
    }
}
