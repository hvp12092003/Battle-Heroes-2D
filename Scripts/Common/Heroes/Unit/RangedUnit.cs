using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;
using GameNamespace;
public class RangedUnit : HeroController
{
    public LayerMask enemyLayer;
    public float attackRange;
    public string nameBullet;
    public Transform posCreateBullet;
    public BulletHeroController bulletScript;
    private Quaternion directionBullet;
    public float temp;
    // Start is called before the first frame update
    private void Start()
    {
        if (hero == null) hero = this.transform.parent.GetComponent<Transform>();
        photonView = this.GetComponent<PhotonView>();
        ranged = this.GetComponent<RangedUnit>();
        hPController = this.transform.parent.GetComponentInChildren<HPController>();
        animator = this.GetComponent<Animator>();
        posCreateBullet = this.transform.parent.Find("PosCreateBullet");
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Physics2D.queriesHitTriggers = true;
        box = this.GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        SetDirection();
        SetDirectionBullet();
    }
    public void SetupObj()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        if (enemyOBJ == null) CheckEnemy();
        CountEffect();
        UpdateStatus();
        ShowAttributeHeroes();
    }
    public void SetDirectionBullet()
    {
        if (hero.CompareTag("Right")) directionBullet = new Quaternion(0, 0, 0, 0);
        else directionBullet = new Quaternion(0, -180, 0, 0);
    }
    public void H4Attack()
    {
        if (meleeUnitScrip != null || photonView.IsMine)
        {
            PhotonView enemyPhotonView = enemyOBJ.GetComponentInChildren<PhotonView>();
            enemyPhotonView.RPC("TakeDamage", RpcTarget.All, this.heroAttribute.baseDamage, this.typeDamage, this.heroAttribute.baseTimeEffect);
        }
        else
        {
            PhotonView enemyPhotonView = enemyOBJ.GetComponentInChildren<PhotonView>();
            enemyPhotonView.RPC("TakeDamage", RpcTarget.All, this.heroAttribute.baseDamage, this.typeDamage, this.heroAttribute.baseTimeEffect);
        }
    }
    public GameObject bulletPrefab;
    public virtual void CreateBullet()
    {
        if (PhotonNetwork.NetworkClientState.ToString() != "Joined")
        {
            bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletOnline/" + nameBullet);
            GameObject bulletInstance = Instantiate(bulletPrefab, this.posCreateBullet.position, directionBullet);

            bulletScript = bulletInstance.GetComponentInChildren<BulletHeroController>();
            if (hero.CompareTag("Right")) bulletScript.dir = DirectionOfBullet.Right;
            else bulletScript.dir = DirectionOfBullet.Left;
            bulletScript.SetBulletProperties(this.heroAttribute.baseDamage, bulletScript.dir, temp, this.typeDamage, posCreateBullet.position);
        }
        else
        {
            if (photonView.IsMine) // Chỉ thực hiện khi là client sở hữu
            {
                GameObject bulletInstance = PhotonNetwork.Instantiate("Prefabs/BulletOnline/" + nameBullet, this.posCreateBullet.position, directionBullet, 0);

                // Lấy PhotonView của viên đạn vừa tạo
                PhotonView bulletPhotonView = bulletInstance.GetComponentInChildren<PhotonView>();

                BulletHeroController bulletScript = bulletInstance.transform.Find("Body").GetComponent<BulletHeroController>();

                // Xác định hướng đạn dựa vào tag của hero
                if (hero.transform.CompareTag("Right")) bulletScript.dir = DirectionOfBullet.Right;
                else bulletScript.dir = DirectionOfBullet.Left;

                // Gọi RPC để thiết lập thuộc tính đạn trên tất cả các client
                bulletPhotonView.RPC("SetBulletProperties", RpcTarget.All, this.heroAttribute.baseDamage, bulletScript.dir, this.heroAttribute.baseTimeEffect, this.typeDamage, posCreateBullet.position);

            }
        }
    }
    public void CheckEnemy()
    {
        if (enemyOBJ != null || barrackEnemyOBJ != null) return;
        Vector2 rayDirection = (hero.transform.tag == "Left") ? Vector2.right : Vector2.left; // Hướng tia dựa trên "tag"
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, rayDirection, attackRange);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;

            Transform objHit = hit.collider.transform.parent;
            if (objHit == null) continue;

            string objHitTag = hit.transform.parent.tag;
            string heroTag = hero.transform.tag;
            //cast enemy
            if ((objHitTag == "Left" && heroTag == "Right") || (objHitTag == "Right" && heroTag == "Left"))
            {
                Debug.Log("cast enemy");
                enemyOBJ = hit.collider.gameObject;
            }

            //cast enemyBarrack
            if ((objHitTag == "BarrackRight" && heroTag == "Left") || (objHitTag == "BarrackLeft" && heroTag == "Right"))
            {
                Debug.Log("cast enemyBarrack");
                barrackEnemyOBJ = hit.collider.gameObject;
            }
        }
    }
    void OnDrawGizmosSelected()
    {

        if (this.transform.parent.CompareTag("Right"))
        {
            Debug.DrawRay(this.transform.position, -transform.right * attackRange, Color.red);
        }
        else
        {
            Debug.DrawRay(this.transform.position, transform.right * attackRange, Color.red);
        }
    }

    // public void OnCollisionEnter2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //touch bullet
        if (collision.transform.CompareTag("Bullet") && PhotonNetwork.NetworkClientState.ToString() == "Joined")
        {
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            if (bulletScript.dir.ToString() == hero.tag) return;
            if (photonView.IsMine) photonView.RPC("TakeDamage", RpcTarget.All, bulletScript.damage, bulletScript.typeD, bulletScript.timeEffect);
            Destroy(collision.transform.parent.gameObject);
            return;
        }
        else if (collision.transform.CompareTag("Bullet") && PhotonNetwork.NetworkClientState.ToString() != "Joined")
        {
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            if (bulletScript.dir.ToString() == hero.tag) return;
            TakeDamage(bulletScript.damage, bulletScript.typeD, bulletScript.timeEffect);
            Destroy(collision.transform.parent.gameObject);
            return;
        }


        if (touchEnemyBarrack || touchEnemy) return;
        // touch Barrack Enemy
        if (collision.transform.CompareTag("BarrackLeft") && hero.transform.CompareTag("Right") || collision.transform.CompareTag("BarrackRight") && hero.transform.CompareTag("Left"))
        {
            barrackEnemyOBJ = collision.gameObject;
            touchEnemyBarrack = true;
            return;
        }
        // touch Enemy
        if (collision.transform.parent.CompareTag("Left") && hero.transform.CompareTag("Right") || collision.transform.parent.CompareTag("Right") && hero.transform.CompareTag("Left"))
        {
            enemyOBJ = collision.gameObject;
            touchEnemy = true;
            return;
        }


        // touch Ally 
        if (collision.transform.parent.CompareTag(this.hero.tag))
        {
            if (hero.CompareTag("Left"))
            {
                if (collision.transform.parent.position.x > this.hero.position.x) touchAlly = true;
            }
            else if (hero.CompareTag("Right"))
            {
                if (collision.transform.parent.position.x < this.hero.position.x) touchAlly = true;
            }

        }
    }
    // public void OnCollisionExit2D(Collision2D collision)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (touchEnemyBarrack || collision.transform.CompareTag("Bullet")) return;
        //laeve enemy walk
        if (collision.transform.parent.tag != this.hero.tag)
        {
            touchEnemy = false;
            enemyOBJ = null;
            meleeUnitScrip = null;
            rangedUnitScrip = null;
        }

        // laeve ally walk
        if (collision.transform.parent.CompareTag(this.hero.tag))
        {
            touchAlly = false;
        }
    }
}
