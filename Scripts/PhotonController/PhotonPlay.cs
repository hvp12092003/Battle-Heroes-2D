using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PhotonPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }
    private void CreatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("Prefabs/PlayerOnline/Player", Vector3.zero, Quaternion.identity, 0);
        //  PhotonNetwork.Instantiate("PlayerPrefab", Vector3.zero, Quaternion.identity, 0, new object[] { }).GetComponent<PhotonView>().RPC("SetupPlayer", RpcTarget.AllBuffered);

    }
}
