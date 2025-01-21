using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > 3 || player.transform.position.y < -3)
        {
            //NO LMAO
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -0.1f * player.transform.position.y, transform.rotation.w);
        }
    }
}
