using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tm_Enemy : MonoBehaviour
{
    Camera cam;

    float camHeight;
    float camWidth;

    readonly float initialSpeed = 5.0f;
    Vector2 speed;

    readonly float initialRotation = 100f;
    float rotation;

    public int points = 10;

    Rigidbody2D rb;

    tm_GameManager gameManager;

    int direction = 1;

    readonly float fireRate = 1f;
    float nextFire;

    public GameObject projectile;
    readonly float projectileSpeed = 4f;

    void Start()
    {
        cam = Camera.main;

        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        gameManager = GameObject.Find("GameManager").GetComponent<tm_GameManager>();

        rotation = Random.Range(-initialRotation, initialRotation);

        float x = Random.Range(-initialSpeed, initialSpeed);
        float y = Random.Range(-initialSpeed, initialSpeed);

        speed = new Vector2(initialSpeed, 0);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
    }

    void Update()
    {
        rb.velocity = direction * speed;

        if (direction == 1 && rb.transform.position.x >= camWidth)
        {
            direction = -1;
        } else if (direction == -1 && rb.transform.position.x <= -camWidth) {
            direction = 1;
        }

        if (tm_GameManager.state == tm_GameManager.States.play)
        {
            tm_Fire();
        }
    }

    void tm_Fire()
    {
        nextFire += Time.deltaTime;

        if (nextFire > fireRate)
        {
            tm_Shoot();
            nextFire = 0;
        }
    }

    void tm_Shoot()
    {
        GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, projectileSpeed, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.tm_KillPlayer();
        }
        else if (collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            gameManager.tm_AddScore(points);
        }
    }
}
