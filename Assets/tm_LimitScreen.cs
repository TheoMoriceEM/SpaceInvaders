using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tm_LimitScreen : MonoBehaviour
{
    Camera cam;

    float height;
    float width;

    void Start()
    {
        cam = Camera.main;

        height = cam.orthographicSize;
        width = height * cam.aspect;
    }

    void Update()
    {
        if (transform.position.x >= width)
        {
            transform.position = new Vector3(width, transform.position.y, 0);
        }
        else if (transform.position.x <= -width)
        {
            transform.position = new Vector3(-width, transform.position.y, 0);
        }
    }
}
