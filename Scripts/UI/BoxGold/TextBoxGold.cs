using HVPUnityBase.Base.DesignPattern;
using TMPro;
using UnityEngine;

public class TextBoxGold : MonoBehaviour
{
    public TextMeshProUGUI amountGold;
    public void UpdateTextGold(int x)
    {
        amountGold.text = x.ToString();
    }
}
public class TEXTGOLD : SingletonMonoBehaviour<TextBoxGold> { }