using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float health;
    public float speed;

    public bool movingEnemyX;

    private GameObject player;
    private float targetPosition;

    void Start()
    {
        if (movingEnemyX)
        {
            targetPosition = -transform.position.x;
        }
        
        player = GameObject.Find("NaveControl").gameObject;
    }

    void Update()
    {
        if (movingEnemyX)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition, transform.position.y, transform.position.z), speed * Time.deltaTime);

            if (transform.position.x <= -35 || transform.position.x >= 35)
            {
                targetPosition = -transform.position.x;
            }
        }

        if (health <= 0)
        {
            player.GetComponent<PlayerManager>().upgradeShip();
            player.GetComponent<PlayerManager>().health+= 50;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= collision.gameObject.GetComponent<BulletClass>().bulletDamage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= other.gameObject.GetComponent<BulletClass>().bulletDamage;
        }
    }
}
