using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using HVPUnityBase.Base.DesignPattern;
public class EventController : MonoBehaviour
{
    public TMP_InputField mess;
    public void Start()
    {
        //SendBasicEvent();
    }
    public void SendMessage()
    {
        byte eventCode = 1;
        string message = mess.text;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Tùy chọn gửi sự kiện
        PhotonNetwork.RaiseEvent(eventCode, message, raiseEventOptions, SendOptions.SendReliable); // message là kdl gui di / nhớ đúng kiểu trả về khi nghe
    }
    public void SendInformationToOther()
    {
        Debug.Log("SendInformationToOther");
        byte eventCode = 3;
        string jsonData = JsonUtility.ToJson(UData.Instance.inforOnJoinedRoom);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(eventCode, jsonData, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SendBasicEvent()
    {
        byte eventCode = 0; // Mã sự kiện (phải duy nhất)
        string message = "Hello from " + PhotonNetwork.NickName; // Dữ liệu gửi đi

        // Cấu hình gửi
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All // Gửi tới tất cả (bao gồm cả người gửi)
        };

        // Gửi sự kiện
        PhotonNetwork.RaiseEvent(eventCode, message, raiseEventOptions, SendOptions.SendReliable);
        Debug.Log("Event sent: " + message);
    }
}
public class EVENTCTL : SingletonMonoBehaviour<EventController> { }
