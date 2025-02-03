using HVPUnityBase.Base.DesignPattern;
using System;
using System.Data;
using UnityEngine;

public class ImageReady : MonoBehaviour
{
    public GameObject objReadyP1, objReadyP2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objReadyP1.SetActive(false);
        objReadyP2.SetActive(false);
        Observer.Instance.AddObserver("UpdateStatusP1", UpdateStatusP1);
        Observer.Instance.AddObserver("UpdateStatusP2", UpdateStatusP2);
    }

    private void UpdateStatusP1(object data)
    {
        bool ready = (bool)data;
        if (ready) objReadyP1.SetActive(true);
        else objReadyP1.SetActive(false);
    }

    private void UpdateStatusP2(object data)
    {
        bool ready = (bool)data;
        if (ready) objReadyP2.SetActive(true);
        else objReadyP2.SetActive(false);
    }

    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver("UpdateStatusP1", UpdateStatusP1);
        Observer.Instance.RemoveObserver("UpdateStatusP2", UpdateStatusP2);
    }
}
