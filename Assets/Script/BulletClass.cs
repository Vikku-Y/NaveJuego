using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    public float bulletSpeed, bulletFireRate, bulletDistance, bulletDamage;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Nave");
    }

    void Update()
    {
        if (player == null || (Vector3.Distance(transform.position, player.transform.position) > 100 && gameObject.name != "CounterHitbox"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name != "CounterHitbox") Destroy(gameObject);
    }

    //audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 30);
}
