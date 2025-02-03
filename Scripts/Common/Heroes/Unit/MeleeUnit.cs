using UnityEngine;
using UnityEngine.UIElements;
using GameNamespace;
using Photon.Pun;

public class MeleeUnit : HeroController
{
    public float attackRange;
    public BarrackController barrackScripts;
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
        rigidbody2 = this.GetComponent<Rigidbody2D>();
        rigidbody2.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2.gravityScale = 0;
        SetDirection();
    }

    // Update is called once per frame
    private void Update()
    {
        CountEffect();
        UpdateStatus();
        ShowAttributeHeroes();
        CheckEnemyAndAlly();
    }

    protected virtual void GiveDamage()
    {
        if (enemyOBJ != null)
        {
            if (meleeEnemyUnitScrip != null)
            {
                meleeEnemyUnitScrip.TakeDamage(this.heroAttribute.baseDamage, this.typeDamage, this.heroAttribute.baseTimeEffect);
            }
            else
            {
                rangedEnemyUnitScrip.TakeDamage(this.heroAttribute.baseDamage, this.typeDamage, this.heroAttribute.baseTimeEffect);
            }
        }
        else if (barrackEnemyOBJ != null)
        {
            barrackScripts = barrackEnemyOBJ.GetComponent<BarrackController>();
            barrackScripts.TakeDamage(this.heroAttribute.baseDamage);
        }
    }
    public void CheckEnemyAndAlly()
    {
        if (enemyOBJ != null || barrackEnemyOBJ != null) return;
        Vector2 rayDirection = (hero.transform.tag == "Left") ? Vector2.right : Vector2.left; // Hướng tia dựa trên "tag"
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, rayDirection, attackRange);

        // not touch ally
        if (hits.Length == 1) touchAlly = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject) continue;

            //cast enemyBarrack
            if (hit.collider.isTrigger) // Kiểm tra có phải trigger hay không
            {
                if (!hit.transform.parent.CompareTag(this.hero.tag))
                {
                    Debug.Log("cast enemyBarrack");
                    barrackEnemyOBJ = hit.collider.gameObject;
                    touchEnemyBarrack = true;
                    return;
                }
            }

            //cast enemy
            if (!hit.transform.parent.tag.Equals(hero.transform.tag))
            {
                Debug.Log("cast enemy");

                enemyOBJ = hit.collider.gameObject;
                if (enemyOBJ.tag.Equals("Melee")) meleeEnemyUnitScrip = hit.collider.gameObject.GetComponent<MeleeUnit>();
                else rangedEnemyUnitScrip = hit.collider.gameObject.GetComponent<RangedUnit>();

                touchEnemy = true;
                return;
            }

            // touch Ally             
            if (hit.transform.parent.tag.Equals(hero.transform.tag) && !hit.collider.isTrigger)
            {
                Debug.Log("touch Ally ");
                touchAlly = true;
                return;
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
