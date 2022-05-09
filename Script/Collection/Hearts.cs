using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim =GetComponent<Animator>();
        anim.SetBool("IfCollecting", false);
    }

    // Update is called once per frame
    void Update()
    {
        CollectAnim();
    }
    //�ռ�����
    void CollectAnim()
    {
        if (gameObject.tag == "Untagged")
        {
            anim.SetBool("IfCollecting", true);
        }
    }

    //����
    void Destoy()
    {
        Destroy(gameObject);
    }


}
