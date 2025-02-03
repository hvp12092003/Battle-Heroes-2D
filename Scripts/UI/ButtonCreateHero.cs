using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCreateHero : MonoBehaviour
{
    public int numButton;
    public Button button;
    public Image image;
    public Sprite[] sprites;
    public int index;

    private void Start()
    {
        index = GAMECTL.Instance.iD_Heroes[numButton - 1];

        button = this.GetComponent<Button>();
        button.onClick.AddListener(ButtonCreate);

        image = this.GetComponent<Image>();
        image.sprite = sprites[index - 1];
    }
    public void ButtonCreate()
    {
        if (GAMECTL.Instance.coinForMath >= UData.Instance.heroAttributes[index].coin && GAMECTL.Instance.canCreateHero)
        {
            GAMECTL.Instance.CreateHero(index);
            GAMECTL.Instance.coinForMath -= UData.Instance.heroAttributes[index].coin;
        }
    }
}
