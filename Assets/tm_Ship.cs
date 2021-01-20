using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tm_Ship : MonoBehaviour
{
    readonly float speed = 10f;
    readonly float drag = 1;
    float thrust;

    float rotation;

    public GameObject projectile;
    readonly float projectileSpeed = 4f;

    private float fireRate = 1f;
    float nextFire;

    Rigidbody2D rb;

    tm_GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;

        gameManager = GameObject.Find("GameManager").GetComponent<tm_GameManager>();
    }

    void Update()
    {
        if (tm_GameManager.state == tm_GameManager.States.play || tm_GameManager.state == tm_GameManager.States.levelup)
        {
            tm_Move();
            if (tm_GameManager.state == tm_GameManager.States.play) {
                tm_Fire();
            }
        }
    }

    void tm_Fire()
    {
        nextFire += Time.deltaTime;

        if (Input.GetButton("Fire1") && nextFire > fireRate)
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

    void tm_Move()
    {
        thrust = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector3 force = transform.TransformDirection(-thrust * speed, 0, 0);
        rb.AddForce(force);
    }

    IEnumerator tm_ManageBonus()
    {
        fireRate = .5f;
        yield return new WaitForSecondsRealtime(10f);
        fireRate = 1f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {
            gameManager.tm_KillPlayer();
        }

        if (collision.tag == "Capsule")
        {
            Destroy(collision.gameObject);
            StartCoroutine(tm_ManageBonus());
        }
    }
}
