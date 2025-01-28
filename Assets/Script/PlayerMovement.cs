using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float camBorderX;
    public float camBorderY;
    public float speedMod;

    private bool boosting = false;
    private bool braking = false;

    private int upgradeTier = 0;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        LocalMove(h, v, moveSpeed);

        transform.localRotation = new Quaternion(-0.2f * v, transform.localRotation.y, -0.1f * h, transform.localRotation.w);

        //Upgrade (For testing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            upgradeTier++;
            transform.Find("Upgrade" + upgradeTier).GameObject().SetActive(true);
        }

        //Boost
        if (upgradeTier >= 2 && Input.GetKeyDown(KeyCode.LeftShift) && !braking)
        {
            boosting = true;

            transform.parent.GameObject().GetComponent<Animator>().SetBool("Boosting", true);
            transform.Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Boosting", true);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed *= speedMod;
            moveSpeed *= speedMod;
        }

        if (upgradeTier >= 2 && Input.GetKeyUp(KeyCode.LeftShift) && !braking)
        {
            boosting = false;

            transform.parent.GameObject().GetComponent<Animator>().SetBool("Boosting", false);
            transform.Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Boosting", false);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed /= speedMod;
            moveSpeed /= speedMod;
        }

        //Brake
        if (upgradeTier >= 1 && Input.GetKeyDown(KeyCode.LeftControl) && !boosting)
        {
            braking = true;

            transform.parent.GameObject().GetComponent<Animator>().SetBool("Braking", true);
            transform.Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Braking", true);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed /= speedMod;
            moveSpeed /= speedMod;
        }

        if (upgradeTier >= 1 && Input.GetKeyUp(KeyCode.LeftControl) && !boosting)
        {
            braking = false;

            transform.parent.GameObject().GetComponent<Animator>().SetBool("Braking", false);
            transform.Find("Upgrade1").GameObject().GetComponent<Animator>().SetBool("Braking", false);

            transform.parent.parent.GameObject().GetComponent<MoveCamera>().camSpeed *= speedMod;
            moveSpeed *= speedMod;
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
}
