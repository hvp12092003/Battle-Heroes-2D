using HVPUnityBase.Base.DesignPattern;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HP_Barrack_UI : MonoBehaviour
{
    public Image hp_bar_Red, hp_bar_Green;
    public void UpdateHPBarRed(float percent)
    {
        hp_bar_Red.fillAmount = percent;
    }
    public void UpdateHPBarGreen(float percent)
    {
        hp_bar_Green.fillAmount = percent;
    }
}
public class UpdateHPBarUI : SingletonMonoBehaviour<HP_Barrack_UI> { }
