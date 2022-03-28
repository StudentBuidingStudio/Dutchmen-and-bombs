using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Bomb : MonoBehaviour
{
    //还是创建一大堆变量
    public float speed = 2;
    public Animator anim;
    Rigidbody2D rb;
    
    
    
    
    
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
        Explotion();
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    public void beTh(Vector2 playerpos)
    {
        GameObject newBomb = Instantiate(gameObject);
        newBomb.transform.position = playerpos;
        rb.velocity = new Vector2(3f, 5f);    
    }

    void Explotion()
    {

    }

    
}
