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
        public float coin;
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
    public class UserDataPost
    {
        public string username;
        public string password;

    }
    //online
    public class UserDataOn
    {
        public string _id;
        public string username;
        public string nickName;
        public int coin;
        public int pointRank;
        public int[] levelOfHeroes;
        public int[] pointOfDungeon;
    }
    // offline
    public class UserDataOff
    {
        public int gold;
        public string account;
        public string passwold;
        public string nickName;
        public int[] heroes;
    }
    public class Response
    {
        public string message;
        public string token;
        public UserDataOn userDataOn;
    }
    public class EventUpdateHP
    {
        public string role;
        public float indexPercentHP;
    }
    public enum TypeButtonMainScene
    {
        Shop, Heroes, Arena, Map, Option
    }
    public class InformationOnJoinedRoom
    {
        public string nickName;
        public int levelBarrack;
        public int[] heroes;
    }
}
