using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;
    public Animator anim;

    public bool isInCollider = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInCollider = true;
        if (collision.tag == "Player")
        {
            anim.SetBool("IfExit", false);
            enterDialog.SetActive(true);
        }
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInCollider = false;
        if (collision.tag == "Player")
        {
            anim.SetBool("IfExit", true);
        }
        isInCollider = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("IfExit", true);
        enterDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInCollider && Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

   
}
