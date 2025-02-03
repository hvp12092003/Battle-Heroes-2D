using UnityEngine;
using Photon.Pun;
using HVPUnityBase.Base.DesignPattern;
public class BarrackController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float baseHP, curentHP;
    public float amountHero;
    public float amountCoinCreatePerSecond;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        curentHP = baseHP;
        photonView = this.GetComponent<PhotonView>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }


    public void TakeDamage(float damage)
    {
        curentHP -= damage;
        if (curentHP <= 0)
        {
            if (photonView.IsMine) Observer.Instance.Notify("End_Match", "Lose");
            else Observer.Instance.Notify("End_Match", "Win");
            Destroy(this.gameObject);
        }


        if (this.transform.tag.Equals("BarrackLeft"))
        {
            UpdateHPBarUI.Instance.UpdateHPBarGreen(curentHP / baseHP);
            ImageProcessingByHP();
        }
        else
        {
            UpdateHPBarUI.Instance.UpdateHPBarRed(curentHP / baseHP);
            ImageProcessingByHP();
        }
    }
    public void ImageProcessingByHP()
    {
        if (baseHP <= baseHP * 0.2f) spriteRenderer.sprite = sprites[4];
        else if (baseHP <= baseHP * 0.4f) spriteRenderer.sprite = sprites[3];
        else if (baseHP <= baseHP * 0.6f) spriteRenderer.sprite = sprites[2];
        else if (baseHP <= baseHP * 0.8f) spriteRenderer.sprite = sprites[1];
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            BulletHeroController bulletScript = collision.GetComponent<BulletHeroController>();
            string barrackTag = this.transform.parent.tag;
            Debug.Log(barrackTag + " " + bulletScript.dir.ToString());
            if (barrackTag.Equals("BarrackRight") && bulletScript.dir.ToString().Equals("Left") || barrackTag.Equals("BarrackLeft") && bulletScript.dir.ToString().Equals("Right"))
            {
                TakeDamage(bulletScript.damage);
                Destroy(collision.gameObject);
            }
        }
        if (!photonView.IsMine) return;
        if (collision.transform.parent.CompareTag(this.transform.parent.tag))
        {
            Debug.Log("touch Ally Hero");
            GAMECTL.Instance.canCreateHero = false;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;
        if (collision.transform.parent.CompareTag(this.transform.parent.tag))
        {
            Debug.Log("exit Ally Hero");
            GAMECTL.Instance.canCreateHero = true;
        }
    }

    [PunRPC]
    public void EnterDataToBarrack(float hp, float amountH, float amountCoinCreatePerSecond, string tagBarrack, Vector3 localScale)
    {
        this.baseHP = hp;
        this.amountHero = amountH;
        this.amountCoinCreatePerSecond = amountCoinCreatePerSecond;
        this.transform.tag = tagBarrack;
        this.gameObject.transform.localScale = localScale;
        if (tagBarrack.Equals("BarrackLeft")) this.transform.parent.tag = "Left";
        else this.transform.parent.tag = "Right";
    }
}
