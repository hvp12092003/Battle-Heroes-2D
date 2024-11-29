using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
public class UIRoomProfile : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI roomName;
    [SerializeField] protected RoomProfile roomProfile;
    [SerializeField] protected TMP_InputField inputRoomName;
    private void Start()
    {
        inputRoomName = GameObject.FindGameObjectWithTag("InputRoomName").GetComponent<TMP_InputField>();
    }
    public void SetRoomProfile(RoomProfile roomProfile)
    {
        this.roomProfile = roomProfile;
        this.roomName.text = roomProfile.roomName;
    }
    public void TakeRoomName()
    {
        inputRoomName.text = roomName.text;
    }
}
