using ExitGames.Client.Photon;
using GameNamespace;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class ButtonUIInMatch : MonoBehaviour
{
    public TMP_InputField mess;
    string titleInRoom;
    public void SendMessage()
    {
        byte eventCode = 1;
        string message = mess.text;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Tùy chọn gửi sự kiện
        PhotonNetwork.RaiseEvent(eventCode, message, raiseEventOptions, SendOptions.SendReliable); // message là kdl gui di / nhớ đúng kiểu trả về khi nghe
    }
    public void ButtonReady()
    {
        Debug.Log("ButtonReady");
        if (PhotonNetwork.IsMasterClient) titleInRoom = "Master";
        else titleInRoom = "Client";
        byte evencode = 2;
        //  StatusMatch statusMatch = StatusMatch.Ready;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(evencode, titleInRoom, raiseEventOptions, SendOptions.SendReliable);
    }
    public void ButtonUnReady()
    {
        Debug.Log("ButtonUnReady");
        if (PhotonNetwork.IsMasterClient) titleInRoom = "Master";
        else titleInRoom = "Client";
        byte evencode = 2;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(evencode, titleInRoom, raiseEventOptions, SendOptions.SendReliable);
    }
}
