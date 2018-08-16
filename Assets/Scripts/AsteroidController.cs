using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D child;
    private Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        if (myRigidbody.velocity == Vector2.zero)
        {
            // Start flying in direction of player.
            TargetPos(FindObjectOfType<PlayerController>().transform.position);
        }
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Bullet") // Hit by bullet
        {
            Rigidbody2D bullet = coll.gameObject.GetComponent<Rigidbody2D>();

            if (child != null)
            {
                Split(bullet.velocity, 30, 5);
                Split(bullet.velocity, -30, 5);
            }

            Destroy(coll.gameObject);
            Destroy(gameObject);
        }
    }

    void Split(Vector3 velocity, float angleOff, float splitSpeed)
    {
        Rigidbody2D clone = Instantiate(child, transform.position, transform.rotation);
        clone.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));
        float angleVelocity = Mathf.Atan2(-velocity.x, -velocity.y);
        angleVelocity += angleOff * Mathf.Rad2Deg;
        clone.velocity = new Vector3(Mathf.Sin(angleVelocity), Mathf.Cos(angleVelocity)) * splitSpeed;
    }

    public void TargetPos(Vector3 position)
    {
        Vector3 direction = Vector3.Normalize(position - transform.position); // Vector of magnitude 1
        myRigidbody.velocity = direction * speed;
    }
}
