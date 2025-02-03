using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 1.0f;
    public float timeExist = 5.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeExist -= Time.deltaTime;
        transform.position  +=  new Vector3(1,0) * speed * Time.deltaTime;
        if (timeExist < 0) Destroy(this.gameObject);
    }
}
