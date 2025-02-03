using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNamespace;
using HVPUnityBase.Base.DesignPattern;

public class EventListener : MonoBehaviour, IOnEventCallback
{
    // Đăng ký nhận sự kiện khi script được bật
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    // Hủy đăng ký khi script bị tắt
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        int eventCode = photonEvent.Code;
        Debug.Log(photonEvent.Code);

        switch (eventCode)
        {
            case 0:
                string receivedMessage = (string)photonEvent.CustomData; // Lấy dữ liệu gửi qua sự kiện
                Debug.Log("Received event: " + receivedMessage);
                break;
            case 1: // mess
                string even = (string)photonEvent.CustomData;

                even = (string)photonEvent.CustomData;
                Debug.Log($"Received message: {even}");
                break;
            case 2:// ready
                even = (string)photonEvent.CustomData;
                Debug.Log(even);
                if (even == "Master")
                {
                    if (GAMECTL.Instance.statusMaster)
                    {
                        GAMECTL.Instance.statusMaster = false;
                    }
                    else
                    {
                        GAMECTL.Instance.statusMaster = true;
                    }

                    if (PhotonNetwork.IsMasterClient) Observer.Instance.Notify("UpdateStatusP1", GAMECTL.Instance.statusMaster);
                    else Observer.Instance.Notify("UpdateStatusP2", GAMECTL.Instance.statusMaster);
                }
                else
                {
                    if (GAMECTL.Instance.statusClient)
                    {
                        GAMECTL.Instance.statusClient = false;
                    }
                    else
                    {
                        GAMECTL.Instance.statusClient = true;
                    }

                    if (!PhotonNetwork.IsMasterClient)
                    {
                        Observer.Instance.Notify("UpdateStatusP1", GAMECTL.Instance.statusClient);
                    }
                    else
                    {
                        Debug.Log("ClientP2");
                        Observer.Instance.Notify("UpdateStatusP2", GAMECTL.Instance.statusClient);
                    }
                }
                if (GAMECTL.Instance.statusMaster && GAMECTL.Instance.statusClient) GAMECTL.Instance.StartMatch();
                break;
            case 3:// take data on joined room
                Debug.Log("take data on joined room");
                object data = photonEvent.CustomData; // Nhận dữ liệu từ sự kiện

                if (data is string jsonData)
                {
                    // Deserialize JSON thành đối tượng
                    InformationOnJoinedRoom info = JsonUtility.FromJson<GameNamespace.InformationOnJoinedRoom>(jsonData);
                    Debug.Log($"Received Data levelBarrack: {info.levelBarrack},heroes: {info.nickName},heroes: {info.heroes[4]}");
                    // Update Text
                    Observer.Instance.Notify("UpdateTextArenaP2", info);
                    Observer.Instance.Notify("UpdateImageBarrackP2", info.levelBarrack);
                    //Update Icon Heroes
                    Observer.Instance.Notify("UpdateUIHeroesP2", info.heroes);
                }
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }

}
