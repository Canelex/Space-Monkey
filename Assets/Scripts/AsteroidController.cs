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
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Bullet") // Hit by bullet
        {
            if (child != null) // If asteroid has smaller version, split.
            {
                Rigidbody2D bullet = coll.GetComponent<Rigidbody2D>();
                Split(bullet.velocity, 30, 5);
                Split(bullet.velocity, -30, 5);
            }

            GameController.instance.AsteroidDestroyed(this, coll.gameObject);
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
