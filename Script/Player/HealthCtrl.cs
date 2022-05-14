using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCtrl : MonoBehaviour
{
    public GameObject playerHea;
    public GameObject heartImage1, heartImage2, heartImage3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController player = playerHea.GetComponent<PlayerController>();
        int heart = player.heart;
        

        if (heart < 3)
        { heartImage3.SetActive(false); }
        else
        { heartImage3.SetActive(true); }
        if (heart < 2)
        { heartImage2.SetActive(false); }
        else
        { heartImage2.SetActive(true); }
        if (heart < 1)
        { heartImage1.SetActive(false); }
        else
        { heartImage1.SetActive(true); }
    }
}
