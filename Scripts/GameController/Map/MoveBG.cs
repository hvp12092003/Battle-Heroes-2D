using UnityEngine;
using DG.Tweening;
public class MoveBG : MonoBehaviour
{
    public float speed;
    public float xStartPos, xEndPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void Update()
    {
        this.transform.position += Vector3.right * speed * Time.deltaTime;
        if (this.transform.position.x >= xEndPoint)
        {
            this.transform.position = new Vector3(xStartPos, this.transform.position.y);
        }
    }

}
