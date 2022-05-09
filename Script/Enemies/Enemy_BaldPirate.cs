using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BaldPirate : MonoBehaviour
{
    //��������
    Rigidbody2D rb;
    Animator anim;
    public Animator animPlayer;
    public BoxCollider2D coll;
    private AudioSource audioDeath;
    public Transform playerTra;
    public LayerMask ground;




    public float speed;
    public int heart;






    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

        AnimCtrl();
    }

    //�ƶ�
    void Movement()
    {

        int playerFace = (int)(-1f * (transform.position.x - playerTra.position.x) / Mathf.Abs(transform.position.x - playerTra.position.x));

        //׷��
        if ((Mathf.Abs(transform.position.x - playerTra.position.x) < 8) && !animPlayer.GetBool("IfDead"))
        {
            
            rb.velocity = new Vector2(speed * playerFace,rb.velocity.y);
            
            

            anim.SetBool("IfRunning", true);
        }
        else
        {
            anim.SetBool("IfRunning", false);
        }
        //����
        transform.localScale = new Vector2(playerFace,1);
        
    }

    //�����ж�
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
                GetComponent<SpriteRenderer>().sortingLayerName = "DeadBodyLayer";
                audioDeath.Play();
            }
        }

       

    }


    //����
    void AnimCtrl()
    {
        //ʬ����������
        if (anim.GetBool("IfDead") && coll.IsTouchingLayers(ground))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        //�����Ӱ˵������
        if (rb.velocity.y < 0 && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("IfFalling", true);
            anim.SetBool("IfGround", false);
        }
        if (anim.GetBool("IfFalling") && coll.IsTouchingLayers(ground))
        {
            anim.SetBool("IfFalling", false);
            anim.SetBool("IfGround", true);
        }
        

    }

    //ֹͣ���� //�����¼�
    void StopGroundAnim()
    {
        anim.SetBool("IfGround", false);
    }














    //�����ж�
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
