using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsChooseHeroes : MonoBehaviour
{
    public ActionButtonChooseHeroes[] buttonScript;
    public Button[] myButton;
    private void Start()
    {
        buttonScript = new ActionButtonChooseHeroes[20];
        myButton = new Button[20];

        buttonScript = this.gameObject.transform.GetComponentsInChildren<ActionButtonChooseHeroes>();

        for (int i = 0; i < buttonScript.Length; i++)
        {
            buttonScript[i].idButton = i + 1;
            myButton[i] = buttonScript[i].gameObject.GetComponent<Button>();
            myButton[i].onClick.AddListener(buttonScript[i].EventChoose);
        }

    }
}
