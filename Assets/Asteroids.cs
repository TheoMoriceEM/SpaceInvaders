using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    readonly float initialSpeed = 5.0f;
    Vector2 speed;

    readonly float initialRotation = 100f;
    float rotation;

    public int points = 10;
    public GameObject[] divisions;

    Rigidbody2D rb;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rotation = Random.Range(-initialRotation, initialRotation);

        // déterminer la vitesse x / y
        float x = Random.Range(-initialSpeed, initialSpeed);
        float y = Random.Range(-initialSpeed, initialSpeed);

        speed = new Vector2(x, y);

        // appliquer la vélocité
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = speed;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.KillPlayer();
        }
        else if (collision.tag == "Bullet")
        {
            // détruire la bullet
            Destroy(collision.gameObject);
            // destruction = astéroïde initial
            Destroy(gameObject); // équivalent à this.gameObject
            // division = potentielle
            foreach (GameObject enemy in divisions)
            {
                Instantiate(enemy, transform.position, Quaternion.identity);
            }
            // score
            gameManager.AddScore(points);
        }
    }
}
