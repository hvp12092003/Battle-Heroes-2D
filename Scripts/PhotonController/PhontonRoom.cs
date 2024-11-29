using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhontonRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputRoomName;
    public Transform roomContent;
    public GameObject roomContentPrefab;
    public UIRoomProfile roomPrefab;
    public List<RoomInfo> updateRooms;
    public List<RoomProfile> rooms = new List<RoomProfile>();

    // Start is called before the first frame update
    void Start()
    {
    }
    public void CreateRoom()
    {
        string name = inputRoomName.text;
        Debug.Log(transform.name + ": Create Room" + name);
        PhotonNetwork.CreateRoom(name);
        //  SceneManager.CreateScene(name);
    }
    public void JoinRoom()
    {
        string name = inputRoomName.text;
        Debug.Log(transform.name + ": Join Room" + name);
        PhotonNetwork.JoinRoom(name);
    }

    public void LeftRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MenuLogin");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        Debug.Log("OnRoomListUpdate " + roomList.Count);



        if (roomList.Count > 0)
        {
            this.updateRooms = roomList;

            // Loop through the updated room list and create UI elements
            foreach (RoomInfo roomInfo in roomList)
            {
                Debug.Log("Room: " + roomInfo.Name);

                // Add or remove rooms from the list
                if (roomInfo.RemovedFromList)
                {
                    Debug.Log("remove " + roomInfo.Name);
                    this.RoomRemove(roomInfo);
                }
                else
                {
                    Debug.Log("add " + roomInfo.Name);
                    this.RoomAdd(roomInfo);
                }
            }

            this.UpdateRoomProfileUI();
        }
    }
    private void ClearUIRoom()
    {
        foreach (Transform child in this.roomContent)
        {
            Destroy(child.gameObject);
        }
    }
    private void UpdateRoomProfileUI()
    {
        ClearUIRoom();
        Debug.Log("UpdateRoomProfileUI");
        foreach (RoomProfile roomProfile in this.rooms)
        {
            GameObject uiRoom = Instantiate(this.roomContentPrefab);
            UIRoomProfile uIRoomProfile = uiRoom.GetComponent<UIRoomProfile>();

            uIRoomProfile.SetRoomProfile(roomProfile);
            uIRoomProfile.transform.SetParent(this.roomContent);

            uiRoom.transform.localScale = Vector3.one;
            uiRoom.transform.position = Vector3.zero;
        }

    }

    private void RoomRemove(RoomInfo roomInfo)
    {
        for (int i = 0; i < this.rooms.Count; i++)
        {
            if (rooms[i].roomName == roomInfo.Name)
            {
                this.rooms.Remove(rooms[i]);
            }
        }
    }
    private void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = this.RoomByName(roomInfo);
        if (roomProfile == null) return;
        this.rooms.Add(roomProfile);
    }

    private RoomProfile RoomByName(RoomInfo roomInfo)
    {
        foreach (RoomProfile roomProfile in this.rooms)
        {
            if (roomProfile.roomName == roomInfo.Name) return null;
        }
        RoomProfile roomProfile1 = new RoomProfile { roomName = roomInfo.Name };
        return roomProfile1;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom " + PhotonNetwork.CurrentRoom.Name);

        //PhotonNetwork.LoadLevel("Room1");
        // PhotonNetwork.LoadLevel("GamePlay");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }
}
