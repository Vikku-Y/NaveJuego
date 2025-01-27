using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float camBorderX;
    public float camBorderY;

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

        /*switch (upgrade)
        {
            case 1:
                transform.Find("Upgrade1").GameObject().SetActive(true);
                break;
            case 2:
                transform.Find("Upgrade1").GameObject().SetActive(true);
                break;
        }*/

        //Boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && upgradeTier >= 2)
        {
            transform.parent.GameObject().GetComponent<Animator>().SetBool("Boosting", true);
            moveSpeed += 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && upgradeTier >= 2)
        {
            transform.parent.GameObject().GetComponent<Animator>().SetBool("Boosting", false);
            moveSpeed -= 2;  // <--------------------- LAST CHANGE
        }

        //Brake
        if (Input.GetKeyDown(KeyCode.LeftControl) && upgradeTier >= 2)
        {

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
