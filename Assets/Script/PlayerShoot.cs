using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Ray ray;
    private float bulletFireRate;

    public GameObject bullet;
    public Vector3 hitpoint;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        bulletFireRate = bullet.GetComponent<BulletClass>().bulletFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("bulletShoot", 0, bulletFireRate);
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("bulletShoot");
        }
    }

    private void bulletShoot()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

        hitpoint = hit.point;
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector3 bulletDirection = transform.position - hitpoint;
        newBullet.gameObject.transform.LookAt(hitpoint);
        float bulletSpeed = newBullet.gameObject.GetComponent<BulletClass>().bulletSpeed;
        newBullet.gameObject.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
