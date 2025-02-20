using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float moveSpeed;
    public float camBorderX;
    public float camBorderY;
    public float speedMod;
    public float counterDuration;
    public float timeForEnergyRegen;
    private float maxEnergyBar = 100;
    private float energyBar = 100;

    public bool boosting = false;
    public bool braking = false;
    public bool blocking = false;
    public bool blockOnCooldown = false;
    public bool blockRelease = false;

    public GameObject slashParticle;
    public GameObject gameplayBox;
    public GameObject counterHitbox;

    private int upgradeTier = 0;
    private float storedDamage = 0;
    private Coroutine boostConsume;
    private Coroutine energyFill;

    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        health = maxHealth;
    }

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
            UpgradeShip();
        }

        //Boost
        if (upgradeTier >= 2 && Input.GetKeyDown(KeyCode.LeftShift) && !braking && !blocking && energyBar > 10)
        {
            boosting = true;

            transform.GetComponentInChildren<Animator>().SetBool("Boosting", true);
            transform.parent.parent.Find("CameraZone").GetComponentInChildren<Animator>().SetBool("Boosting", true);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Boosting", true);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed *= speedMod;
            moveSpeed *= speedMod;
            camBorderX+= 2;
            camBorderY+= 2;

            boostConsume = StartCoroutine(ConsumeEnergyBar(5, 0.25f));
            if (energyFill != null)
            {
                StopCoroutine(energyFill);
            }
        }

        if (upgradeTier >= 2 && ((Input.GetKeyUp(KeyCode.LeftShift) && !braking && !blocking) || energyBar < 5) && boosting)
        {
            boosting = false;

            transform.GetComponentInChildren<Animator>().SetBool("Boosting", false);
            transform.parent.parent.Find("CameraZone").GetComponentInChildren<Animator>().SetBool("Boosting", false);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Boosting", false);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed /= speedMod;
            moveSpeed /= speedMod;
            camBorderX-= 2;
            camBorderY-= 2;

            StopCoroutine(boostConsume);
            energyFill = StartCoroutine(FillEnergyBar(1.5f, 0.1f));
        }

        //Brake
        if (upgradeTier >= 1 && Input.GetKeyDown(KeyCode.LeftControl) && !boosting && !blocking)
        {
            braking = true;

            transform.GetComponentInChildren<Animator>().SetBool("Braking", true);
            transform.parent.parent.Find("CameraZone").GetComponentInChildren<Animator>().SetBool("Braking", true);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Braking", true);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed /= speedMod;
            moveSpeed /= speedMod;
            camBorderX -= 2;
            camBorderY -= 2;
        }

        if (upgradeTier >= 1 && Input.GetKeyUp(KeyCode.LeftControl) && !boosting && !blocking && braking)
        {
            braking = false;

            transform.GetComponentInChildren<Animator>().SetBool("Braking", false);
            transform.parent.parent.Find("CameraZone").GetComponentInChildren<Animator>().SetBool("Braking", false);
            transform.Find("Nave").Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Braking", false);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed *= speedMod;
            moveSpeed *= speedMod;
            camBorderX += 2;
            camBorderY += 2;
        }

        //Block
        if (upgradeTier >= 3 && Input.GetMouseButtonDown(1) && !boosting && !braking && !blockOnCooldown && energyBar == maxEnergyBar)
        {
            blocking = true;

            transform.GetComponentInChildren<Animator>().SetBool("Blocking", true);
            transform.Find("Nave").Find("Upgrade3").GameObject().GetComponent<Animator>().SetBool("Blocking", true);

            slashParticle.SetActive(false);
            StartCoroutine(BlockReleaseTimer());
        }

        if (upgradeTier >= 3 && (Input.GetMouseButtonUp(1) || blockRelease) && !boosting && !braking && blocking)
        {
            blockRelease = false;

            transform.GetComponentInChildren<Animator>().SetBool("Blocking", false);
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

    public void UpgradeShip()
    {
        if (upgradeTier < 3)
        {
            upgradeTier++;
            transform.Find("Nave").Find("Upgrade" + upgradeTier).GameObject().SetActive(true);
        }
    }

    public void ReleaseCounter()
    {
        counterHitbox.SetActive(true);

        counterHitbox.GetComponent<BulletClass>().bulletDamage = storedDamage;
        float speed = counterHitbox.GetComponent<BulletClass>().bulletSpeed;

        counterHitbox.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.VelocityChange);

        slashParticle.transform.position = transform.position;
        slashParticle.SetActive(true);

        energyBar = 0;
        UIManager.Instance.energyBar.fillAmount = 0;

        energyFill = StartCoroutine(FillEnergyBar(1.5f, 0.1f));
    }

    public void RetreatCounter()
    {
        counterHitbox.GetComponent<BulletClass>().bulletDamage = 0;
        storedDamage = 0;

        counterHitbox.GetComponent<Rigidbody>().velocity = Vector3.zero;
        counterHitbox.transform.localPosition = Vector3.zero;

        counterHitbox.SetActive(false);

        blocking = false;
    }

    public void UpdateHP()
    {
        UIManager.Instance.hpBar.fillAmount = ((health * 100) / maxHealth) / 100;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float dmg = collision.gameObject.GetComponent<BulletClass>().bulletDamage;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (blocking)
            {
                storedDamage += dmg;
            } else
            {
                health -= dmg;
                UpdateHP();
            }
        }
    }

    IEnumerator BlockReleaseTimer()
    {
        yield return new WaitForSeconds(counterDuration);

        blockRelease = true;
    }

    IEnumerator FillEnergyBar(float ammount, float fillRate)
    {
        while (energyBar < maxEnergyBar)
        {
            energyBar += ammount;
            UIManager.Instance.energyBar.fillAmount = energyBar / 100;
            yield return new WaitForSeconds(fillRate);
        }

        if (energyBar > maxEnergyBar) energyBar = maxEnergyBar;
        energyFill = null;
    }

    IEnumerator ConsumeEnergyBar(float ammount, float consumeRate)
    {
        while (energyBar > 0) { 
            energyBar-= ammount;
            UIManager.Instance.energyBar.fillAmount = energyBar / 100;
            yield return new WaitForSeconds(consumeRate);
        }

        if (energyBar < 0) energyBar = 0;
    }
}
