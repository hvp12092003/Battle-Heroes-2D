using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonStatus : MonoBehaviour
{
    public string photonStatus;
    public TextMeshProUGUI textStatus;

    // Update is called once per frame
    void Update()
    {
        this.photonStatus = PhotonNetwork.NetworkClientState.ToString();
        this.textStatus.text = photonStatus;
        Debug.Log(this.photonStatus);
    }
    //Authenticating
    //ConnectingToMasterServer
    //OnConnectedToMaster
    //JoiningLobby
    //JoinedLobby
}
