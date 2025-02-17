using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerManager : MonoBehaviour
{
    public float health;
    public float moveSpeed;
    public float camBorderX;
    public float camBorderY;
    public float speedMod;

    public bool boosting = false;
    public bool braking = false;
    public bool blocking = false;
    public bool blockOnCooldown = false;
    public bool blockRelease = false;

    public GameObject slashParticle;
    public GameObject gameplayBox;

    private int upgradeTier = 0;
    private float storedDamage = 0;
    public GameObject counterHitbox;

    void Update()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        LocalMove(h, v, moveSpeed);

        transform.localRotation = new Quaternion(-0.2f * v, transform.localRotation.y, -0.1f * h, transform.localRotation.w);

        //Upgrade (For testing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            upgradeShip();
        }

        //Boost
        if (upgradeTier >= 2 && Input.GetKeyDown(KeyCode.LeftShift) && !braking && !blocking)
        {
            boosting = true;

            transform.Find("Nave").GameObject().GetComponent<Animator>().SetBool("Boosting", true);
            transform.parent.parent.Find("CameraZone").GameObject().GetComponent<Animator>().SetBool("Boosting", true);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Boosting", true);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed *= speedMod;
            moveSpeed *= speedMod;
        }

        if (upgradeTier >= 2 && Input.GetKeyUp(KeyCode.LeftShift) && !braking && !blocking && boosting)
        {
            boosting = false;

            transform.Find("Nave").GameObject().GetComponent<Animator>().SetBool("Boosting", false);
            transform.parent.parent.Find("CameraZone").GameObject().GetComponent<Animator>().SetBool("Boosting", false);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Boosting", false);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed /= speedMod;
            moveSpeed /= speedMod;
        }

        //Brake
        if (upgradeTier >= 1 && Input.GetKeyDown(KeyCode.LeftControl) && !boosting && !blocking)
        {
            braking = true;

            transform.Find("Nave").GameObject().GetComponent<Animator>().SetBool("Braking", true);
            transform.parent.parent.Find("CameraZone").GameObject().GetComponent<Animator>().SetBool("Braking", true);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Braking", true);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed /= speedMod;
            moveSpeed /= speedMod;
        }

        if (upgradeTier >= 1 && Input.GetKeyUp(KeyCode.LeftControl) && !boosting && !blocking && braking)
        {
            braking = false;

            transform.Find("Nave").GameObject().GetComponent<Animator>().SetBool("Braking", false);
            transform.parent.parent.Find("CameraZone").GameObject().GetComponent<Animator>().SetBool("Braking", false);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Braking", false);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed *= speedMod;
            moveSpeed *= speedMod;
        }

        //Block
        if (upgradeTier >= 2 && Input.GetMouseButtonDown(1) && !boosting && !braking && !blockOnCooldown)
        {
            blocking = true;

            transform.Find("Nave").GameObject().GetComponent<Animator>().SetBool("Blocking", true);
            transform.Find("Nave").Find("Upgrade3").GameObject().GetComponent<Animator>().SetBool("Blocking", true);

            StartCoroutine(blockReleaseTimer());
        }

        if (upgradeTier >= 2 && (Input.GetMouseButtonUp(1) || blockRelease) && !boosting && !braking && blocking)
        {
            blocking = false;
            blockRelease = false;

            transform.Find("Nave").GameObject().GetComponent<Animator>().SetBool("Blocking", false);
            transform.Find("Nave").Find("Upgrade3").GameObject().GetComponent<Animator>().SetBool("Blocking", false);
        }
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        FixPosition();
    }

    void FixPosition()
    {
        if (transform.localPosition.x > camBorderX)
        {
            transform.localPosition = new Vector3(camBorderX, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.x < -camBorderX)
        {
            transform.localPosition = new Vector3(-camBorderX, transform.localPosition.y, transform.localPosition.z);
        }

        if (transform.localPosition.y > camBorderY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, camBorderY, transform.localPosition.z);
        }
        else if (transform.localPosition.y < -camBorderY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -camBorderY, transform.localPosition.z);
        }
    }

    public void upgradeShip()
    {
        if (upgradeTier < 3)
        {
            upgradeTier++;
            transform.Find("Nave").Find("Upgrade" + upgradeTier).GameObject().SetActive(true);
        }
    }

    public void releaseCounter()
    {
        counterHitbox.SetActive(true);

        counterHitbox.GetComponent<BulletClass>().bulletDamage = storedDamage;
        float speed = counterHitbox.GetComponent<BulletClass>().bulletSpeed;

        counterHitbox.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.VelocityChange);

        Instantiate(slashParticle, transform.position, transform.rotation, gameplayBox.transform);
    }

    public void retreatCounter()
    {
        counterHitbox.GetComponent<BulletClass>().bulletDamage = 0;
        storedDamage = 0;

        counterHitbox.GetComponent<Rigidbody>().velocity = Vector3.zero;
        counterHitbox.transform.localPosition = Vector3.zero;

        counterHitbox.SetActive(false);
        StartCoroutine(blockCooldown());
    }

    private void OnCollisionEnter(Collision collision)
    {
        float dmg = collision.gameObject.GetComponent<BulletClass>().bulletDamage;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (blocking)
            {
                storedDamage += dmg;
                dmg /= 10;
            }

            health -= dmg;
        }
    }

    IEnumerator blockCooldown()
    {
        blockOnCooldown = true;

        yield return new WaitForSeconds(3);

        blockOnCooldown = false;
    }

    IEnumerator blockReleaseTimer()
    {
        yield return new WaitForSeconds(1.5f);

        blockRelease = true;
    }
}
