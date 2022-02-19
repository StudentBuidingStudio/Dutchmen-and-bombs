using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    private float startPointx,startPointy;
    // Start is called before the first frame update
    void Start()
    {
        startPointx = transform.position.x;
        startPointy = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position = new Vector2(startPointx + (cam.position.x * moveRate), startPointy + (cam.position.y * moveRate));
    }
}
