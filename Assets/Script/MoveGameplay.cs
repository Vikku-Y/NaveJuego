using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float camSpeed = 3;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * camSpeed * Time.deltaTime;
        transform.TransformDirection(transform.localPosition);
    }
}
