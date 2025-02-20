using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float health;
    public float speed;

    public bool movingEnemyX;
    public bool movingEnemyY;

    private float targetPosition;

    void Start()
    {
        if (movingEnemyX)
        {
            targetPosition = -transform.position.x;
        }

        if (movingEnemyY)
        {
            targetPosition = -transform.position.y;
        }
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

        if (movingEnemyY)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosition, transform.position.z), speed * Time.deltaTime);

            if (transform.position.y <= -7 || transform.position.y >= 14)
            {
                targetPosition = -transform.position.y + 7;
            }
        }

        if (health <= 0)
        {
            PlayerManager.Instance.UpgradeShip();
            PlayerManager.Instance.health+= 20;
            PlayerManager.Instance.UpdateHP();
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
