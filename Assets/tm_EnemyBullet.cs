using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tm_EnemyBullet : MonoBehaviour
{
    float life = 7f;

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
