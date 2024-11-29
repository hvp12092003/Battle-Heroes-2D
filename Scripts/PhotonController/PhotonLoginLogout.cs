using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class PhotonLoginLogout : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputUserName;
    string nameUser;
    // Start is called before the first frame update
    void Start()
    {
        nameUser ="hvp";
        Login(); 
        
    }

    public void Login()
    {
        //string name = inputUserName.text;
        Debug.Log(this.nameUser);

        PhotonNetwork.LocalPlayer.NickName = nameUser;
        PhotonNetwork.ConnectUsingSettings();
    }
    public void Logout()
    {
        
        PhotonNetwork.Disconnect();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }
}
