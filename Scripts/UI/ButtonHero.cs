using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHero : MonoBehaviour
{
    public int num;
    public void CreateHero()
    {
        GAMECTL.Instance.CreateHero(num);
    }
    public void EXPHero()
    {
        UData.Instance.HandlerLevelHeroData(0,100);
    }
}
