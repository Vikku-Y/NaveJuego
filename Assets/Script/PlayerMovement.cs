using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float camBorderX;
    public float camBorderY;

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        LocalMove(h, v, moveSpeed);

        transform.localRotation = new Quaternion(-0.2f * v, transform.localRotation.y, -0.1f * h, transform.localRotation.w);

        //Boost (To be tied to part wings)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 2);
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
