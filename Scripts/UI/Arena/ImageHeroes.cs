using HVPUnityBase.Base.DesignPattern;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageHeroes : MonoBehaviour
{
    public Image[] heroesP1, heroesP2;
    public Sprite[] heroes;
    public Sprite emtyIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Observer.Instance.AddObserver("UpdateUIHeroesP1", UpdateUIHeroes1);
        Observer.Instance.AddObserver("UpdateUIHeroesP2", UpdateUIHeroes2);
        Observer.Instance.AddObserver("ResetIconHeroesP2", ResetIconHeroesP2);
    }

    private void UpdateUIHeroes1(object data)
    {
        for (int i = 0; i < heroesP1.Length; i++)
        {
            heroesP1[i].sprite = heroes[UData.Instance.userData.heroes[i] - 1];
        }
    }
    private void UpdateUIHeroes2(object data)
    {
        int[] idHeroes = (int[])data;
        Debug.Log(idHeroes.Length);
        Debug.Log(idHeroes[2]);
        for (int i = 0; i < heroesP2.Length; i++)
        {
            heroesP2[i].sprite = heroes[idHeroes[i]-1];
        }
    }
    public void ResetIconHeroesP2(object data)
    {
        for (int i = 0; i < heroesP2.Length; i++)
        {
            heroesP2[i].sprite = emtyIcon;
        }
    }
    void OnDestroy()
    {
        Observer.Instance.RemoveObserver("UpdateUIHeroesP1", UpdateUIHeroes1);
        Observer.Instance.RemoveObserver("UpdateUIHeroesP2", UpdateUIHeroes2);
        Observer.Instance.RemoveObserver("ResetIconHeroesP2", ResetIconHeroesP2);
    }
}
