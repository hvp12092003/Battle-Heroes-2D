using GameNamespace;
using HVPUnityBase.Base.DesignPattern;
using System;
using TMPro;
using UnityEngine;

public class TextArenaManager : MonoBehaviour
{
    public TextMeshProUGUI nameP1, nameP2, lvBarrackP1, lvBarrackP2, roomName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Observer.Instance.AddObserver("UpdateTextArenaP1", UpdateTextP1);
        Observer.Instance.AddObserver("UpdateTextArenaP2", UpdateTextP2);
        Observer.Instance.AddObserver("UpdateTextArenaRoomName", UpdateRoomName);
        Observer.Instance.AddObserver("ResetRoomName", ResetRoomName);
        Observer.Instance.AddObserver("ResetNameP2", ResetNameP2);
    }

    private void ResetRoomName(object data)
    {
        roomName.text = "";
    }
    private void ResetNameP2(object data)
    {
        nameP2.text = "";
    }


    public void UpdateTextP1(object data)
    {
        Debug.Log("UpdateTextArenaP1");
        nameP1.text = UData.Instance.userData.nickName;
        lvBarrackP1.text = "Lv: " + UData.Instance.dataLevelBarrack.level.ToString();
    }
    public void UpdateTextP2(object data)
    {
        InformationOnJoinedRoom dataTaken = (InformationOnJoinedRoom)data;
        Debug.Log("UpdateTextArenaP2");
        nameP2.text = dataTaken.nickName;
        lvBarrackP2.text = "Lv: " + dataTaken.levelBarrack.ToString();
    }
    public void UpdateRoomName(object data)
    {
        roomName.text = data.ToString();
    }
    void OnDestroy()
    {
        Observer.Instance.RemoveObserver("UpdateTextArenaP1", UpdateTextP1);
        Observer.Instance.RemoveObserver("UpdateTextArenaP2", UpdateTextP2);
        Observer.Instance.RemoveObserver("UpdateTextArenaRoomName", UpdateRoomName);
        Observer.Instance.RemoveObserver("ResetRoomName", ResetRoomName);
        Observer.Instance.RemoveObserver("ResetNameP2", ResetNameP2);
    }
}
