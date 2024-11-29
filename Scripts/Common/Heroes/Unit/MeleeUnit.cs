using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using GameNamespace;
using Photon.Pun;
public class MeleeUnit : HeroController
{

    // Start is called before the first frame update
    private void Start()
    {
        if (hero == null) hero = this.transform.parent.GetComponent<Transform>();
        photonView = this.GetComponent<PhotonView>();
        melee = this.GetComponent<MeleeUnit>();
        hPController = this.transform.parent.GetComponentInChildren<HPController>();
        animator = this.GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        box = this.GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        SetDirection();
    }
    // Update is called once per frame
    private void Update()
    {
        CountEffect();
        UpdateStatus();
        ShowAttributeHeroes();
    }
    protected virtual void GiveDamage()
    {
        if (enemyOBJ != null)
        {
            if (enemyOBJ == null || !photonView.IsMine) return;
            if (meleeUnitScrip != null)
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
        else if (barrackEnemyOBJ != null)
        {
            if (!photonView.IsMine) return;
            PhotonView barrackPhotonView = barrackEnemyOBJ.GetComponent<PhotonView>();
            barrackPhotonView.RPC("TakeDamage", RpcTarget.All, this.heroAttribute.baseDamage);
        }
    }
    public PhotonView thisPhotonView;
    // public void OnCollisionEnter2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //touch bullet
        if (collision.transform.CompareTag("Bullet") && PhotonNetwork.NetworkClientState.ToString() == "Joined")
        {
            if (!photonView.IsMine) return;
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            if (bulletScript.dir.ToString() == hero.tag) return;
            photonView.RPC("TakeDamage", RpcTarget.All, bulletScript.damage, bulletScript.typeD, bulletScript.timeEffect);
            Destroy(collision.gameObject);
            return;
        }
        else if (collision.transform.CompareTag("Bullet") && PhotonNetwork.NetworkClientState.ToString() != "Joined")
        {
            Debug.Log("Touch Bullet");
            if (!photonView.IsMine) return;
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            if (bulletScript.dir.ToString() == hero.tag) return;
            TakeDamage(bulletScript.damage, bulletScript.typeD, bulletScript.timeEffect);
            Destroy(collision.gameObject);
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
                if (collision.transform.parent.position.x > this.transform.parent.position.x) touchAlly = true;
            }
            else if (hero.CompareTag("Right"))
            {
                if (collision.transform.parent.position.x < this.transform.parent.position.x) touchAlly = true;
            }
            return;
        }
    }
    // public void OnCollisionExit2D(Collision2D collision)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent == null || touchEnemyBarrack) return;

        //laeve enemy walk
        if (collision.transform.parent.tag != this.transform.parent.tag)
        {
            enemyOBJ = null;
            meleeUnitScrip = null;
            rangedUnitScrip = null;
            touchEnemy = false;
            touchEnemyBarrack = false;
        }

        // laeve ally walk
        if (collision.transform.parent.CompareTag(this.transform.parent.tag))
        {
            touchAlly = false;
        }

    }
}
