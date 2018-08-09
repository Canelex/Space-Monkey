using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [Tooltip("Speed of asteroid in space")]
    public float speed;
    [Tooltip("Type of asteroid(s) created after destruction")]
    public GameObject prefab;
    private Rigidbody2D myRigidBody;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Collider2D other = coll.collider;

        if (other.gameObject.tag == "Bullet")
        {
            if (prefab != null)
            {
                CreateChildAsteroid(other.transform, 120);
                CreateChildAsteroid(other.transform, -120);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void CreateChildAsteroid(Transform bullet, float zRot)
    {
        GameObject asteroid = Instantiate(prefab, transform.position, bullet.rotation);
        asteroid.transform.Rotate(0, 0, zRot);
    }
}
