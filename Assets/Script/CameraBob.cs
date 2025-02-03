using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        float offset = 0.015f;
        if (player.transform.localPosition.y > 3)
        {
            transform.localRotation = new Quaternion((-0.005f * player.transform.localPosition.y) + offset, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        } else if (player.transform.localPosition.y < -3)
        {
            transform.localRotation = new Quaternion((-0.005f * player.transform.localPosition.y) - offset, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        } else
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    //lerp: auto "transicion" de movimiento
}
