using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    public float bulletSpeed, bulletFireRate, bulletDistance;
    public int bulletDamage;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Nave");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 100)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
