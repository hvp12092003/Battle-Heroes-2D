using UnityEngine;
using UnityEngine.UI;

public class MoveCamByScrollbarr : MonoBehaviour
{
    public GameObject cammera;
    public GameObject Layer03;
    public Scrollbar scrollbar;
    public void Move()
    {
        cammera.transform.position = new Vector3(scrollbar.value * 65.9f, 0f, -10f);
        if(Layer03==null)return;
        Layer03.transform.position = new Vector3(scrollbar.value * 6.59f+32.5f, -1.21f, 0);
    }

}
