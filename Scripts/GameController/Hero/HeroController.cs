using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GameNamespace;
using Photon.Pun;

public class HeroController : MonoBehaviour
{
    [Header("Attribute")]
    public HeroAttribute heroAttribute;

    // show Attribute
    public float baseHP;
    public float baseDamage;
    public float speed = 2;
    public float baseTimeEffect;
    public float baseCoolDownAttack;

    public float countCoolDownAttack, takeDamagePoison, timeGetEffect;
    public float direction;
    public Status status = Status.Idle;
    public TypeOfDamage typeDamage;
    public TypeHero typeHero;
    public bool touchAlly = false, touchEnemy = false, touchEnemyBarrack = false;


    [Header("Component")]
    public Transform hero;
    public Animator animator;
    public MeleeUnit meleeEnemyUnitScrip;
    public RangedUnit rangedEnemyUnitScrip;
    public HPController hPController;
    public MeleeUnit melee;
    public RangedUnit ranged;
    public GameObject enemyOBJ, barrackEnemyOBJ, allyOBJ;
    public BoxCollider2D box;
    public Rigidbody2D rigidbody2;
    public AnimatorStateInfo stateInfo;
    public PhotonView photonView;
    public void Awake()
    {
        heroAttribute = new HeroAttribute();
        if (PhotonNetwork.NetworkClientState.ToString() == "PeerCreated")
        {
            this.heroAttribute.baseHP = this.baseHP;
            this.heroAttribute.baseDamage = this.baseDamage;
            this.heroAttribute.speed = this.speed;
            this.heroAttribute.baseTimeEffect = this.baseTimeEffect;
            this.heroAttribute.baseCoolDownAttack = this.baseCoolDownAttack;
        }
    }

    protected virtual void UpdateStatus()
    {
        if (this.heroAttribute.baseHP <= 0)
        {
            box.enabled = false;
            animator.Play("Die");
            return;
        }
        switch (status)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Walk:
                UpdateWalk();
                break;
            case Status.GetHit:
                UpdateGetHit();
                break;
            case Status.GetPoisoned:
                UpdateGetPoisoned();
                break;
            case Status.GetFreez:
                Debug.Log("UpdateGetFreez");
                UpdateGetFreez();
                break;
        }
    }
    protected virtual void CountEffect()
    {
        //cool down attack
        if (countCoolDownAttack > 0 || timeGetEffect <= 0) countCoolDownAttack -= Time.deltaTime;

        //count time freez
        if (timeGetEffect > 0 && status == Status.GetFreez)
        {
            timeGetEffect -= Time.deltaTime;
        }


        //count time Poison
        if (timeGetEffect > 0 && status == Status.GetPoisoned)
        {
            timeGetEffect -= Time.deltaTime;
            this.heroAttribute.baseHP -= takeDamagePoison * Time.deltaTime;
            hPController.UpdateBarHP();
            if (this.heroAttribute.baseHP <= -1)
            {
                animator.Play("Die");
                if (typeHero == TypeHero.Melee) melee.enabled = false;
                else ranged.enabled = false;
                return;
            }
        }
    }

    protected virtual void Move()
    {
        if (!touchAlly && !touchEnemy && !touchEnemyBarrack) this.hero.transform.position += new Vector3(this.heroAttribute.speed * direction, 0) * this.heroAttribute.speed * Time.deltaTime;
    }
    protected virtual void SetDirection()
    {
        if (hero.tag == "Right")
        {
            direction = -1;
            this.hero.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            direction = 1;
            this.hero.transform.localScale = new Vector3(-1, 1, 1);
        }
    }




    protected virtual void UpdateIdle()
    {
        animator.Play("Idle");

        if (countCoolDownAttack <= 0)
        {
            if (enemyOBJ != null || barrackEnemyOBJ != null)
            {
                status = Status.Attack;
                return;
            }
        }
        if(enemyOBJ==null) touchEnemy = false;
        if (!touchEnemy && !touchAlly && !touchEnemyBarrack) status = Status.Walk;
    }
    protected virtual void UpdateAttack()
    {
        animator.Play("Attack");
        countCoolDownAttack = this.heroAttribute.baseCoolDownAttack;
        if (enemyOBJ == null)
        {
            enemyOBJ = null;
            meleeEnemyUnitScrip = null;
            rangedEnemyUnitScrip = null;
            touchEnemy = false;
            touchEnemyBarrack = false;
        }
    }
    protected virtual void UpdateWalk()
    {
        animator.Play("Walk");
        Move();
        if (enemyOBJ != null && countCoolDownAttack <= 0 || barrackEnemyOBJ != null && countCoolDownAttack <= 0 || touchAlly || touchEnemy || touchEnemyBarrack) status = Status.Idle;
    }


    protected virtual void UpdateGetPoisoned()
    {
        Debug.Log("UpdateGetPoisoned");
        status = Status.GetPoisoned;
        animator.Play("GetPoisoned");
    }
    protected virtual void UpdateGetFreez()
    {

        status = Status.GetFreez;

        if (stateInfo.IsName("Attack"))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                status = Status.GetFreez;
                animator.Play("GetFreez");
            }
        }
        else
        {
            status = Status.GetFreez;
            animator.Play("GetFreez");
        }

        if (timeGetEffect < 0)
        {
            animator.SetBool("IsGetFreez", false);
            status = Status.Idle;
        }
    }
    protected virtual void UpdateGetHit()
    {
        if (stateInfo.IsName("Attack"))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                status = Status.GetHit;
                animator.Play("GetHit");
            }
        }
        else
        {
            status = Status.GetHit;
            animator.Play("GetHit");
        }
    }
    protected virtual void UpdateDie()
    {
        Debug.Log("Die");
        Destroy(this.transform.parent.gameObject);
    }
    public void EndAction()
    {
        status = Status.Idle;
        animator.Play("Idle");
    }

    [PunRPC]
    public void EnterDataToHero(float hp, float damage, float speed, float baseTimeEffect, float baseCoolDownAttack, string tagHero, Vector3 pos)
    {
        if (hero == null) hero = this.transform.parent.GetComponent<Transform>();
        this.heroAttribute.baseHP = hp;
        this.heroAttribute.baseDamage = damage;
        this.heroAttribute.speed = speed;
        this.heroAttribute.baseTimeEffect = baseTimeEffect;
        this.heroAttribute.baseCoolDownAttack = baseCoolDownAttack;
        this.transform.parent.tag = tagHero;
        this.transform.parent.position = pos;
        ShowAttributeHeroes();
    }
    public virtual void TakeDamage(float damage, TypeOfDamage typeD, float timeEffect)
    {
        Debug.Log("TakeDamage");
        this.heroAttribute.baseHP -= damage;
        hPController.UpdateBarHP();

        switch (typeD)
        {
            case TypeOfDamage.Hit:
                if (status == Status.GetFreez || status == Status.Attack) return;
                UpdateGetHit();
                break;
            case TypeOfDamage.Freez:
                timeGetEffect = timeEffect;
                UpdateGetFreez();
                break;
            case TypeOfDamage.Poison:
                timeGetEffect = timeEffect;
                takeDamagePoison = damage;
                if (status == Status.GetFreez || status == Status.Attack) return;
                UpdateGetPoisoned();
                break;
        }

    }
    public void ShowAttributeHeroes()
    {
        this.baseHP = this.heroAttribute.baseHP;
        this.baseDamage = this.heroAttribute.baseDamage;
        this.speed = this.heroAttribute.speed;
        this.baseTimeEffect = this.heroAttribute.baseTimeEffect;
        this.baseCoolDownAttack = this.heroAttribute.baseCoolDownAttack;
    }
}
