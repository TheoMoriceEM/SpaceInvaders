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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.States.play)
        {
            Move();
            Turn();
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

    void Turn()
    {
        rotation = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, rotation * Time.deltaTime * rotationSpeed * -1);
    }

    void Move()
    {
        thrust = Input.GetAxisRaw("Vertical");
        if (thrust < 0)
        {
            thrust = 0;
        }
    }

    private void FixedUpdate()
    {
        Vector3 force = transform.TransformDirection(0, thrust * speed, 0);
        rb.AddForce(force);
    }
}
