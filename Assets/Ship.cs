using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // accélération / décélération
    readonly float speed = 10f;
    readonly float drag = 1; // résistance
    float thrust; // poussée

    // rotation
    readonly float rotationSpeed = 150f;
    float rotation;

    // pouvoir tirer
    public GameObject projectile;
    readonly float projectileSpeed = 4f;

    readonly float fireRate = .25f;
    float nextFire;

    Rigidbody2D rb;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.States.play || GameManager.state == GameManager.States.levelup)
        {
            Move();
            Fire();
        }
    }

    void Fire()
    {
        nextFire += Time.deltaTime;

        if (Input.GetButton("Fire1") && nextFire > fireRate)
        {
            Shoot();
            nextFire = 0;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, projectileSpeed, 0);
    }

    void Move()
    {
        thrust = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector3 force = transform.TransformDirection(-thrust * speed, 0, 0);
        rb.AddForce(force);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {
            gameManager.KillPlayer();
        }
    }
}
