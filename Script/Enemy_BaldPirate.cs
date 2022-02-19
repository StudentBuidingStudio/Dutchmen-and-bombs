using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BaldPirate : MonoBehaviour
{
    //创建变量
    private Rigidbody2D rb;
    private Animator anim;
    public Animator animPlayer;
    private BoxCollider2D coll;
    private AudioSource audioDeath;
    public Transform playerX;



    public float speed;
    public int heart;






    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        audioDeath = GetComponent<AudioSource>();

        coll.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;

        heart = 1;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!anim.GetBool("IfDead") && !anim.GetBool("IfAttack"))
        {
            Movement();
        }
    }
    void Update()
    { 
        if (!coll.isTrigger)
        {
            Dead();
        }
    }

    //移动
    void Movement()
    {
        //追踪
        if ((Mathf.Abs(transform.position.x - playerX.position.x) < 8) && !animPlayer.GetBool("IfDead"))
        {
            if (Mathf.Abs(rb.velocity.x) < 4)
            {
                rb.velocity = new Vector2(rb.velocity.x + ((float)(-1 * speed *
                (transform.position.x - playerX.position.x) / Mathf.Abs(transform.position.x - playerX.position.x))),
                rb.velocity.y);
            }

            anim.SetBool("IfRunning", true);
        }
        else
        {
            anim.SetBool("IfRunning", false);
        }
        //朝向
        transform.localScale = new Vector2(-1 *
            ((transform.position.x - playerX.position.x) / Mathf.Abs(transform.position.x - playerX.position.x)),
            1);
        
    }

    //死亡判定
    void Dead()
    {
        if (tag == "Untagged")
        {
            heart -= 1;
            if (heart != 0)
            {
                gameObject.tag = "Enemies";
            }
            else
            {
                anim.SetBool("IfDead", true);
                coll.isTrigger = true;
                rb.bodyType = RigidbodyType2D.Static;
                audioDeath.Play();
            }
        }

    }

    //攻击判定
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("IfAttack", true);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }



    public void StopAttack()
    {
        anim.SetBool("IfAttack", false);
    }

   
}
