using UnityEngine;
using HVPUnityBase.Base.DesignPattern;
using DG.Tweening;
public class GamePlay : MonoBehaviour
{
    public GameObject panelWin, panelLose;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Register the observer to a topic
        Observer.Instance.AddObserver("End_Match", OnReceiveData);

        panelWin.SetActive(false);
        panelLose.SetActive(false);
    }
    void OnReceiveData(object data)
    {
        string result = data.ToString();
        if (result.Equals("Win"))
        {
            ShowPanelWin();
        }
        else ShowPanelLose();
        GAMECTL.Instance.SaveResult();
    }
    private void ShowPanelWin()
    {
        panelWin.SetActive(true);
        panelWin.transform.DOMoveY(0,1f).SetEase(Ease.Linear);
    }
    private void ShowPanelLose()
    {
        panelLose.SetActive(true);
        panelLose.transform.DOMoveY(0,1f).SetEase(Ease.Linear);
    }
}
