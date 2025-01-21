using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float clampOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        LocalMove(h, v, moveSpeed);

        transform.rotation = new Quaternion(-0.2f * v, transform.rotation.y, -0.1f * h, transform.rotation.w);
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        FixPosition();
    }

    void FixPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x - clampOffset);
        pos.y = Mathf.Clamp01(pos.y - clampOffset);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
