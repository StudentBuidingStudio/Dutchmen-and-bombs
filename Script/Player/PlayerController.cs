using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //变量创建
    [Header("物理")]
    Rigidbody2D rb;
    Animator anim;
    public float speed, jumpForce, jumpDuration,
    fallingDuration;
    public int jumpCount;
    public bool ifJump, ifJumped1, ifJumped2;
    public LayerMask ground;

    [Header("音频")]
    public AudioSource audioCollectCrystal,audioCollectHeart, audioJump,audioDie;
    
    [Header("属性")]
    public BoxCollider2D collButton, collMain;
    public Text crystalNumber;
    public int heart = 3, crystal = 0 ,opTime;
    public bool ifHurt = false;
    
    [Header("其他物品")]
    public GameObject cameraMove,
    bomb;
    



    // Start is called before the first frame update
    void Start()
    {
        //变量赋值
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


    //移动函数
    void Movement()
    {
        //获取键盘输入
        float move = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        

        //移动
        rb.velocity = new Vector2(
                speed * move, rb.velocity.y);
        
        //动画

        if (move != 0)
        {
            
            anim.SetBool("IfRunning", true);
            
        }
     
        else
        {
            anim.SetBool("IfRunning", false);
        }

        //朝向
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
        
        //坠落距离判定
        if (anim.GetBool("IfFalling"))
        {
            fallingDuration += 1f;
        }
        else
        {
            fallingDuration = 0;
        }
    }

    //跳跃控制
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


    //动画控制
    void ControllAnim()
    {
        
        //下落状态判定
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
        
        //落地判定
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
        //二段跳判定

        else if (jumpCount == 0)
        {
            anim.SetBool("IfTwice", true);
            anim.SetBool("IfFalling", false);
        }
        


    }

    //碰撞互动
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集
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


        //敌人
        if (collision.tag == "Enemies")
        {
            

            //获取敌人方向
            float enemyFace = (transform.position.x - collision.gameObject.transform.position.x)
                / Mathf.Abs(transform.position.x - collision.gameObject.transform.position.x);
            //踩踏伤害
            if (anim.GetBool("IfFalling") && collButton.IsTouching(collision))
            {
                /*伤害
                collision.gameObject.tag = "Untagged";*/

                //伤害
                

                //力的作用是相互的
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                anim.SetBool("IfJumping", true);
                anim.SetBool("IfFalling", false);
            }
            //受伤
            else if (opTime==0)
            {
                //物理
                rb.velocity = new Vector2(5 * enemyFace, 0);
                transform.localScale = new Vector3(-enemyFace, 1, 1);
                //op时间                
                opTime = 10; 

                heart -= 1; 
                ifHurt = true;

                anim.SetBool("IfHurting", true);

            }
        }

    
    
        


    }
    
    //生命控制
    void HeartImage()
    {
        

        //掉出地图
        if (transform.position.y < -30)
        {
            heart = 0;
        }
        
        //死亡判定
        if (heart == 0)
        {
            Invoke("Die", 2f);
            if (collButton.IsTouchingLayers(ground))
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
            
            //物理属性
            collMain.isTrigger = true;
            collButton.isTrigger = true;
            rb.velocity = new Vector2(0, rb.velocity.y);

            //终止音频
            GetComponent<AudioSource>().enabled = false;
            
            //关闭镜头跟随
            cameraMove.SetActive(false);



            if (!anim.GetBool("IfDead"))
            { audioDie.Play(); }
            anim.SetBool("IfDead", true);
        }

        //op时间
        if (opTime != 0)
        {
            opTime -= 1;
        }
    }

    //重开 //Invoke引用 
    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //取消落地状态 //动画事件
    void StopGroundAnim()
    {
        anim.SetBool("IfGround", false);
    }


    //原力投弹
    public void ThrowBomb()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Bomb ctrledBomb = bomb.GetComponent<Bomb>();
        Vector2 bombPos = new Vector2(transform.position.x + (transform.localScale.x * -1), transform.position.y);
        ctrledBomb.beTh(bombPos);
        
    
    }
    
    
}
