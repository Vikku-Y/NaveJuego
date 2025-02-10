using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private float bulletFireRate;
    private GameObject player;
    private Vector3 playerLastPosition;

    public bool activated = false;

    public GameObject bullet;
    public float deviation;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        bulletFireRate = bullet.GetComponent<BulletClass>().bulletFireRate;
        player = GameObject.Find("Nave");
        playerLastPosition = player.transform.position;

        InvokeRepeating("bulletShoot", 1, bulletFireRate);
    }

    private void bulletShoot()
    {
        if (activated)
        {
            Vector3 hitpoint;
            float currentDeviation = deviation + (0.1f * GameObject.FindGameObjectsWithTag("Enemy").Length);

            float xDeviation = Random.Range(0f, currentDeviation) - currentDeviation / 2;
            float yDeviation = Random.Range(0f, currentDeviation) - currentDeviation / 2;

            if (player.transform.position.x == playerLastPosition.x && player.transform.position.y == playerLastPosition.y)
            {
                xDeviation /= 4;
                yDeviation /= 4;
            }

            hitpoint = new Vector3(player.transform.position.x + xDeviation, player.transform.position.y + yDeviation, player.transform.position.z);

            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            Vector3 bulletDirection = transform.position - hitpoint;
            newBullet.gameObject.transform.LookAt(hitpoint);
            float bulletSpeed = newBullet.gameObject.GetComponent<BulletClass>().bulletSpeed;

            newBullet.gameObject.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);

            playerLastPosition = player.transform.position;
        }
    }
}
