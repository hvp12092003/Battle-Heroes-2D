using DG.Tweening;
using System;
using UnityEngine;

public class ActionButtonChooseHeroes : MonoBehaviour
{
    public bool chosen = false;
    public int idButton, numberPosIDHero;
    public Vector3 posOrigin;
    public void Start()
    {
        posOrigin = this.transform.position;
    }
    public void EventChoose()
    {
        if (!chosen)
        {
            if (GAMECTL.Instance.iD_Heroes.Count >= 5 || GAMECTL.Instance.iD_Heroes.Contains(idButton)) return;
            chosen = true;
            GAMECTL.Instance.iD_Heroes.Add(idButton);

            for (int i = 0; i < GAMECTL.Instance.posHeroID.Count; i++)
            {
                if (!GAMECTL.Instance.posHeroID[i])
                {
                    numberPosIDHero = i;
                    GAMECTL.Instance.posHeroID[i] = true;
                    this.transform.DOMove(new Vector3(GAMECTL.Instance.objPosHeroID[i].transform.position.x, GAMECTL.Instance.objPosHeroID[i].transform.position.y), 0.5f).SetEase(Ease.Linear);
                    return;
                }
            }
        }
        else
        {
           // if (GAMECTL.Instance.heroIDMine.Count >= 5) return;
            chosen = false;
            GAMECTL.Instance.posHeroID[numberPosIDHero] = false;
            this.transform.DOMove(posOrigin, 0.5f).SetEase(Ease.Linear);
            GAMECTL.Instance.iD_Heroes.Remove(idButton);
        }

    }
}
