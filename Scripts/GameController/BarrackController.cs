using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
public class BarrackController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float baseHP;
    public float amountHero;
    public float amountCoinCreatePerSecond;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            string barrackTag = this.transform.tag;
            Debug.Log(barrackTag + " " + bulletScript.dir.ToString());
            if (barrackTag == "BarrackRight" && bulletScript.dir.ToString() == "Left" || barrackTag == "BarrackLeft" && bulletScript.dir.ToString() == "Right")
            {
                if (photonView.IsMine) photonView.RPC("TakeDamage", RpcTarget.All, bulletScript.damage);
                Destroy(collision.gameObject);
            }
        }
    }
    public void ImageProcessingByHP()
    {
        if (baseHP <= baseHP * 0.2f) spriteRenderer.sprite = sprites[4];
        else if (baseHP <= baseHP * 0.4f) spriteRenderer.sprite = sprites[3];
        else if (baseHP <= baseHP * 0.6f) spriteRenderer.sprite = sprites[2];
        else if (baseHP <= baseHP * 0.8f) spriteRenderer.sprite = sprites[1];
    }
    [PunRPC]
    public void TakeDamage(float damage)
    {
        baseHP -= damage;
        if (baseHP <= 0 && photonView.IsMine)
        {
            //handel lose
            string titleInRoom;
            if (PhotonNetwork.IsMasterClient) titleInRoom = "Master";
            else titleInRoom = "Client";

            byte evencode = 3;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(evencode, titleInRoom, raiseEventOptions, SendOptions.SendReliable);

            Destroy(this.gameObject);
        }
        ImageProcessingByHP();
    }
    [PunRPC]
    public void EnterDataToBarrack(float hp, float amountH, float amountCoinCreatePerSecond, string tagBarrack, Vector3 localScale)
    {
        this.baseHP = hp;
        this.amountHero = amountH;
        this.amountCoinCreatePerSecond = amountCoinCreatePerSecond;
        this.transform.tag = tagBarrack;
        this.gameObject.transform.localScale = localScale;
    }
}
