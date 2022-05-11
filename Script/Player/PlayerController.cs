using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //��������
    [Header("����")]
    Rigidbody2D rb;
    Animator anim;
    public float speed, jumpForce, jumpDuration,
    fallingDuration;
    public int jumpCount;
    public bool ifJump, ifJumped1, ifJumped2;
    public LayerMask ground;

    [Header("��Ƶ")]
    public AudioSource audioCollectCrystal,audioCollectHeart, audioJump,audioDie;
    
    [Header("����")]
    public BoxCollider2D collButton, collMain;
    public Text crystalNumber;
    public int heart = 3, crystal = 0 ,opTime;
    public bool ifHurt = false;
    
    [Header("������Ʒ")]
    public GameObject cameraMove,
    bomb;
    



    // Start is called before the first frame update
    void Start()
    {
        //������ֵ
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        

        heart = 3;
        crystal = 0;
        cameraMove.SetActive(true);
        anim.SetBool("IfGround", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ifHurt && !anim.GetBool("IfDead") && !anim.GetBool("IfGround"))
        {
            Jump();
        }
        ControllAnim();

        HeartImage();

        if (Input.GetKeyUp(KeyCode.K))
        {
            ThrowBomb();
        }
    }

    private void FixedUpdate()
    {
        if (!ifHurt && !anim.GetBool("IfDead") && !anim.GetBool("IfGround"))
        {
            Movement();
        }
    }


    //�ƶ�����
    void Movement()
    {
        //��ȡ��������
        float move = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        

        //�ƶ�
        rb.velocity = new Vector2(
                speed * move, rb.velocity.y);
        
        //����

        if (move != 0)
        {
            
            anim.SetBool("IfRunning", true);
            
        }
     
        else
        {
            anim.SetBool("IfRunning", false);
        }

        //����
        if (face != 0)
        {
            transform.localScale = new Vector3(face, 1,1);
        }

        if (ifJump && jumpDuration >= 0 && !(jumpCount == 0 && !ifJumped2))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("IfJumping", true);
            anim.SetBool("IfFalling", false);
            jumpDuration -= 0.1f;
        }
        
        //׹������ж�
        if (anim.GetBool("IfFalling"))
        {
            fallingDuration += 1f;
        }
        else
        {
            fallingDuration = 0;
        }
    }

    //��Ծ����
    void Jump()
    {
        ifJump = Input.GetButton("Jump");
        

        
        if (ifJump &&jumpCount == 2)
        {
            jumpCount = 1;
            ifJumped1 = true;
            if (!audioJump.isPlaying)
            { audioJump.Play(); }

        }
        else if (ifJump && jumpCount == 1 && !ifJumped1 && !Input.GetButtonUp("Jump"))
        {
            jumpCount = 0;
            jumpDuration = 2;
            anim.SetBool("IfTwice", true);
            ifJumped2 = true;
            audioJump.Play(); 

        }



        if (Input.GetButtonUp("Jump"))
        {
            if (jumpCount == 1)
            {
                ifJumped1 = false;
            }
            else if (jumpCount == 0)
            {
                ifJumped2 = false;
            }
        }

    }


    //��������
    void ControllAnim()
    {
        
        //����״̬�ж�
        if (rb.velocity.y < -0.2 && !collButton.IsTouchingLayers(ground))
        {
            
            anim.SetBool("IfJumping", false);
            anim.SetBool("IfFalling", true);
            anim.SetBool("IfTwice", false);
            if (jumpCount == 2)
            { jumpCount = 1; }

        }
        else if (ifHurt)
        {         
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("IfHurting", false);
                ifHurt = false;
            }
        }
        
        //����ж�
        else if (collButton.IsTouchingLayers(ground) && !anim.GetBool("IfJumping"))
        {
            
            anim.SetBool("IfFalling", false);
            if (fallingDuration > 40) 
            {
                anim.SetBool("IfGround", true);
                fallingDuration = 0;
            }
            
            jumpCount = 2;
            jumpDuration = 2;
        }
        //�������ж�

        else if (jumpCount == 0)
        {
            anim.SetBool("IfTwice", true);
            anim.SetBool("IfFalling", false);
        }
        


    }

    //��ײ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ռ�
        if (collision.tag == "Hearts")
        {
            if (heart < 3)
            { heart += 1; }
            audioCollectHeart.Play();
            collision.gameObject.tag = "Untagged";
        }
        if (collision.tag == "Crystals")
        {
            Destroy(collision.gameObject);
            crystal += 1;
            crystalNumber.text = crystal.ToString();
            audioCollectCrystal.Play();
            collision.gameObject.tag = "Untagged";
            
        }


        //����
        if (collision.tag == "Enemies")
        {
            

            //��ȡ���˷���
            float enemyFace = (transform.position.x - collision.gameObject.transform.position.x)
                / Mathf.Abs(transform.position.x - collision.gameObject.transform.position.x);
            //��̤�˺�
            if (anim.GetBool("IfFalling") && collButton.IsTouching(collision))
            {
                /*�˺�
                collision.gameObject.tag = "Untagged";*/

                //�˺�
                

                //�����������໥��
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                anim.SetBool("IfJumping", true);
                anim.SetBool("IfFalling", false);
            }
            //����
            else if (opTime==0)
            {
                //����
                rb.velocity = new Vector2(5 * enemyFace, 0);
                transform.localScale = new Vector3(-enemyFace, 1, 1);
                //opʱ��                
                opTime = 10; 

                heart -= 1; 
                ifHurt = true;

                anim.SetBool("IfHurting", true);

            }
        }

    
    
        


    }
    
    //��������
    void HeartImage()
    {
        

        //������ͼ
        if (transform.position.y < -30)
        {
            heart = 0;
        }
        
        //�����ж�
        if (heart == 0)
        {
            Invoke("Die", 2f);
            if (collButton.IsTouchingLayers(ground))
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
            
            //��������
            collMain.isTrigger = true;
            collButton.isTrigger = true;
            rb.velocity = new Vector2(0, rb.velocity.y);

            //��ֹ��Ƶ
            GetComponent<AudioSource>().enabled = false;
            
            //�رվ�ͷ����
            cameraMove.SetActive(false);



            if (!anim.GetBool("IfDead"))
            { audioDie.Play(); }
            anim.SetBool("IfDead", true);
        }

        //opʱ��
        if (opTime != 0)
        {
            opTime -= 1;
        }
    }

    //�ؿ� //Invoke���� 
    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //ȡ�����״̬ //�����¼�
    void StopGroundAnim()
    {
        anim.SetBool("IfGround", false);
    }


    //ԭ��Ͷ��
    public void ThrowBomb()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Bomb ctrledBomb = bomb.GetComponent<Bomb>();
        Vector2 bombPos = new Vector2(transform.position.x + (transform.localScale.x * -1), transform.position.y);
        ctrledBomb.beTh(bombPos);
        
    
    }
    
    
}
