using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HPController : MonoBehaviour
{
    public HeroController heroesController;
    public SpriteRenderer spriteRedBar;
    public float maxHP;
    public float damage;
    private void Start()
    {
        heroesController = this.transform.parent.GetComponentInChildren<HeroController>();
        spriteRedBar = GetComponentInChildren<SpriteRenderer>();
        maxHP = heroesController.heroAttribute.baseHP;
    }
    public void UpdateBarHP()
    {
        float temp = heroesController.heroAttribute.baseHP / maxHP;
        if (temp < 0) temp = 0;
        spriteRedBar.transform.localScale = new Vector3(temp, 1f, 1f);
    }
}
