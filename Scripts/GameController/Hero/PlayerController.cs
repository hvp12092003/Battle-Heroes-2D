using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MovePlayer();
        }
        if(Input.GetMouseButtonDown(0)) Shot();
    }

    private void Shot()
    {
            GameObject bullet = PhotonNetwork.Instantiate("Prefabs/BulletOnline/Bullet", this.transform.position, Quaternion.identity, 0);
    }
   
    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}
