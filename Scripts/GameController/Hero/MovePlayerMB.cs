using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

public class MovePlayerMB : MonoBehaviour
{
    private Vector3 offset;
    private float posMouseZ;
    private PhotonView photonView;
    void OnMouseDown()
    {
        photonView = this.GetComponent<PhotonView>();

        posMouseZ = Camera.main.WorldToScreenPoint(transform.position).z;

        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (photonView.IsMine) transform.position = GetMouseWorldPos() + offset;

    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = posMouseZ;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
