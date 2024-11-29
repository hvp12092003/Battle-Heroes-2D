using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNamespace;
public class BulletHeroController : MonoBehaviour
{
    public float damage;
    public float speed = 10;
    public float timeEffect;
    public DirectionOfBullet dir;
    public Vector3 vectorDir;
    public TypeOfDamage typeD;
    private float countExister = 5, count;
    // Start is called before the first frame update
    void Start()
    {
        if (dir == DirectionOfBullet.Left) vectorDir = new Vector3(1, 0, 0);
        else vectorDir = new Vector3(-1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += vectorDir * speed * Time.deltaTime;
        count += Time.deltaTime;
        if (count > countExister) Destroy(this.gameObject.transform.parent.gameObject);
    }
    [PunRPC]
    public void SetBulletProperties(float bulletDamage, DirectionOfBullet bulletDir, float timeEffect, TypeOfDamage typeD, Vector3 pos)
    {
        this.damage = bulletDamage;
        this.dir = bulletDir;
        this.timeEffect = timeEffect;
        this.typeD = typeD;
        this.transform.position = pos;
    }
   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet")) return;
        if (collision.transform.parent == null)
        {
            if (collision.transform.CompareTag("BarrackLeft") && dir.ToString() == "Right" || collision.transform.CompareTag("BarrackRight") && dir.ToString() == "Left")
                Destroy(this.gameObject);
            return;
        }
        if (collision.transform.parent.tag != dir.ToString())
        {
            Destroy(this.gameObject);
        }
    }*/
}
