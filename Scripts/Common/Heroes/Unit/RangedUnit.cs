using Photon.Pun;
using UnityEngine;
using GameNamespace;
using UnityEngine.UIElements;

public class RangedUnit : HeroController
{
    public LayerMask enemyLayer;
    public float attackRange, touchAllyRange;
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
        box.isTrigger = false;
        box.compositeOperation = Collider2D.CompositeOperation.None;
        rigidbody2 = this.GetComponent<Rigidbody2D>();
        rigidbody2.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2.gravityScale = 0;
        SetDirection();
        SetDirectionBullet();
    }

    public void SetupObj()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (enemyOBJ == null || allyOBJ == null)
        {
            CheckCast();
            CheckTouch();
        }
        CountEffect();
        UpdateStatus();
        ShowAttributeHeroes();
    }

    public void SetDirectionBullet()
    {
        if (hero.CompareTag("Right")) directionBullet = new Quaternion(0, 0, 0, 0);
        else directionBullet = new Quaternion(0, -180, 0, 0);
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
            if (photonView.IsMine)
            {
                GameObject bulletInstance = PhotonNetwork.Instantiate("Prefabs/BulletOnline/" + nameBullet, this.posCreateBullet.position, directionBullet, 0);

                PhotonView bulletPhotonView = bulletInstance.GetComponentInChildren<PhotonView>();

                BulletHeroController bulletScript = bulletInstance.transform.Find("Body").GetComponent<BulletHeroController>();

                if (hero.transform.CompareTag("Right")) bulletScript.dir = DirectionOfBullet.Right;
                else bulletScript.dir = DirectionOfBullet.Left;

                bulletPhotonView.RPC("SetBulletProperties", RpcTarget.All, this.heroAttribute.baseDamage, bulletScript.dir, this.heroAttribute.baseTimeEffect, this.typeDamage, posCreateBullet.position);
            }
        }
    }

    public void CheckCast()
    {
        //cast
        Vector2 rayDirection = (hero.transform.tag == "Left") ? Vector2.right : Vector2.left;
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, rayDirection, attackRange);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            //cast enemyBarrack
            if (hit.collider.isTrigger)
            {
                if (!hit.transform.parent.CompareTag(this.hero.tag))
                {
                    Debug.Log("cast enemyBarrack");
                    barrackEnemyOBJ = hit.collider.gameObject;
                    continue;
                }
            }
            //cast enemy
            if (!hit.transform.parent.tag.Equals(hero.transform.tag))
            {
                Debug.Log("cast enemy");
                enemyOBJ = hit.collider.gameObject;
            }
        }
    }
    public void CheckTouch()
    { //  touch
        Vector2 rayDirection = (hero.transform.tag == "Left") ? Vector2.right : Vector2.left;
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(this.transform.position, rayDirection, touchAllyRange);
        if (hits2.Length == 1) touchAlly = false;

        foreach (RaycastHit2D hit in hits2)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            // touch Ally
            if (hit.transform.parent.CompareTag(hero.tag) && !hit.collider.isTrigger)
            {
                Debug.Log("touch Ally");
                touchAlly = true;
                return;
            }

            // touch Barrack Enemy
            if (hit.collider.isTrigger)
            {
                if (!hit.transform.parent.CompareTag(this.hero.tag))
                {
                    touchEnemyBarrack = true;
                    return;
                }
            }

            // touch Enemy
            if (!hit.transform.parent.tag.Equals(hero.transform.tag))
            {
                //enemyOBJ = hit.collider.gameObject;
                touchEnemy = true;
                return;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (this.transform.parent.CompareTag("Right"))
        {
            Debug.DrawRay(this.transform.position, -transform.right * touchAllyRange, Color.red);
        }
        else
        {
            Debug.DrawRay(this.transform.position, transform.right * touchAllyRange, Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //touch bullet
        if (collision.transform.parent.CompareTag("Bullet") && !collision.transform.CompareTag(hero.tag))
        {
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            TakeDamage(bulletScript.damage, bulletScript.typeD, bulletScript.timeEffect);
            Destroy(collision.transform.parent.gameObject);
            return;
        }
    }
}
