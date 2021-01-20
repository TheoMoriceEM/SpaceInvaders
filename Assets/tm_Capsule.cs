using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tm_Capsule : MonoBehaviour
{
    float life = 7f;

    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
