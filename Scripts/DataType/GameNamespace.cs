using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNamespace
{
    public enum Status
    {
        Walk, Idle, Attack, Die, GetFreez, GetHit, GetPoisoned
    }
    public enum TypeOfDamage
    {
        Poison, Freez, Hit
    }
    public enum TypeHero
    {
        ranged, Melee
    }
    public enum DirectionOfBullet
    {
        Right, Left
    }
    public class HeroAttribute
    {
        public float baseHP;
        public float baseDamage;
        public float speed = 2;
        public float baseTimeEffect;
        public float baseCoolDownAttack;
    }
    public class BarrackAttribute
    {
        public float baseHP;
        public float amountHero;
        public float amountCoinCreatePerSecond;
    }
    public class DataLevelBarrack
    {
        public int level;
        public float amountEXP;
    }
    public class DataUser
    {
        public HeroAttribute[] DataHeroes;
        public GameObject[] heroes;
        public float gold;

        public DataLevelHeroes[] dataHeroes;
    }
    public class DataLevelHeroes
    {
        public float level;
        public float amountEXP;
    }
    public class DataInstance
    {
        public float[] pointOfInstance;
    }
    public class PostData
    {
        public string username;
        public string password;
    }
    [System.Serializable]
    public class LoginResponse
    {
        public string token;
        public string message;
        public bool success;
    }
    public class Response
    {
        public string message;
        public string token;
    }
}
