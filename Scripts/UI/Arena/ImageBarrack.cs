using HVPUnityBase.Base.DesignPattern;
using UnityEngine;
using UnityEngine.UI;

public class ImageBarrack : MonoBehaviour
{
    public Image barrackP1, barrackP2;
    public Sprite[] barrack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barrackP1.gameObject.SetActive(false);
        barrackP2.gameObject.SetActive(false);
        Observer.Instance.AddObserver("UpdateImageBarrackP1", UpdateImageBarrackP1);
        Observer.Instance.AddObserver("UpdateImageBarrackP2", UpdateImageBarrackP2);
        Observer.Instance.AddObserver("ResetImageBarrackP2", ResetImageBarrackP2);
    }
    public void UpdateImageBarrackP1(object data)
    {
        barrackP1.gameObject.SetActive(true);
        barrackP1.sprite = barrack[UData.Instance.dataLevelBarrack.level - 1];
    }
    public void UpdateImageBarrackP2(object data)
    {
        int barrackLevel = (int)data;
        barrackP2.gameObject.SetActive(true);
        Debug.Log($"LvBarrackP2: {barrackLevel}");
        barrackP2.sprite = barrack[barrackLevel - 1];
    }
    public void ResetImageBarrackP2(object data)
    {
        barrackP2.gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        Observer.Instance.RemoveObserver("UpdateImageBarrackP1", UpdateImageBarrackP1);
        Observer.Instance.RemoveObserver("UpdateImageBarrackP2", UpdateImageBarrackP2);
        Observer.Instance.RemoveObserver("ResetImageBarrackP2", ResetImageBarrackP2);
    }
}
