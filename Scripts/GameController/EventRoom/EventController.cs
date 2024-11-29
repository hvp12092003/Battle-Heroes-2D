using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
public class EventController : MonoBehaviour
{
    public TMP_InputField mess;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SendMessage()
    {
        byte eventCode = 1; 
        string message  = mess.text;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Tùy chọn gửi sự kiện
        PhotonNetwork.RaiseEvent(eventCode, message, raiseEventOptions, SendOptions.SendReliable); // message là kdl gui di / nhớ đúng kiểu trả về khi nghe
    }
    public void CreateHero()
    {
        PhotonNetwork.Instantiate("Prefabs/Heroes/Hero1"  , this.transform.position, Quaternion.identity, 0);
    }
}
