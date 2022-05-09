using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Whale : MonoBehaviour
{
    //��������
    Rigidbody2D rb;
    Animator anim;
    public Animator animPlayer;
    public BoxCollider2D coll;
    AudioSource audioDeath;
    public Transform playerTra;
    public LayerMask ground;

    public int heart;



    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioDeath = GetComponent<AudioSource>();


        coll.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetBool("IfGround", false);
    }

    // Update is called once per frame
    void Update()
    {
        AnimCtrl();
        Dead();
    }

    private void FixedUpdate()
    {
        if (!anim.GetBool("IfDead") && !anim.GetBool("IfAttack"))
        {
            Movement();
        }
    }

    void Movement()
    {
        int playerFace = (int)(-1f * (transform.position.x - playerTra.position.x) / Mathf.Abs(transform.position.x - playerTra.position.x));

        //׷��
        if ((Mathf.Abs(transform.position.x - playerTra.position.x) < 8) && !animPlayer.GetBool("IfDead"))
        {

            if (!coll.IsTouchingLayers(ground))
                {
                rb.velocity = new Vector2(speed * playerFace,rb.velocity.y);
                }
                anim.SetBool("IfRunning", true);   
               
            

            if (anim.GetBool("IfJumping") && rb.velocity.y < 0.1f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 6);
                
            }
          
        }
        else
        {
            anim.SetBool("IfRunning", false);
            anim.SetBool("IfGround", false);
        }
        //����
        transform.localScale = new Vector2(
        ((transform.position.x - playerTra.position.x) / Mathf.Abs(transform.position.x - playerTra.position.x)),
        1);


    }


    void AnimCtrl()
    {
        if (rb.velocity.y<0 && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("IfFalling", true);
            anim.SetBool("IfJumping", false);
        }
        if (anim.GetBool("IfFalling") && coll.IsTouchingLayers(ground))
        {
            anim.SetBool("IfFalling", false);
            anim.SetBool("IfGround", true);
        }
        if (anim.GetBool("IfRunning") && !anim.GetBool("IfGround") && !anim.GetBool("IfFalling"))
        {
            anim.SetBool("IfJumping", true);
        }

        //ʬ����������
        if (anim.GetBool("IfDead") && coll.IsTouchingLayers(ground))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }


    //��ʼ��Ծ //�����¼�
    void Jump()
    {
        anim.SetBool("IfGround", false);
    }


    void Dead()
    {
        //������
        if (tag == "Untagged")
        {
            heart -= 1;
            if (heart != 0)
            {
                //Ѫ��û����
                gameObject.tag = "Enemies";
            }
            else
            {
                //����
                anim.SetBool("IfDead", true);
                coll.isTrigger = true;
                GetComponent<SpriteRenderer>().sortingLayerName = "DeadBodyLayer";
                audioDeath.Play();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //����
            anim.SetBool("IfAttack", true);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }



    //�˳�����״̬  //�����¼�
    public void StopAttack()
    {
        anim.SetBool("IfAttack", false);
    }
}
