using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapScreen : MonoBehaviour
{
    Camera cam;

    float height;
    float width;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        height = cam.orthographicSize;
        width = height * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > width)
        {
            transform.position = new Vector3(-width, transform.position.y, 0);
        }
        else if (transform.position.x < -width)
        {
            transform.position = new Vector3(width, transform.position.x, 0);
        }

        if (transform.position.y > height)
        {
            transform.position = new Vector3(transform.position.x, -height, 0);
        }
        else if (transform.position.y < -height)
        {
            transform.position = new Vector3(transform.position.x, height, 0);
        }
    }
}
