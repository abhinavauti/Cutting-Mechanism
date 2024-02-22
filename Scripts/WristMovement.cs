using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristMovement : MonoBehaviour
{
    public float speed = -10.0f;
    public float upperBound = 2.1f;
    public float lowerBound = 1.635f;
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && transform.position.y < upperBound)
        {
            // Move up
            Vector2 movement = new Vector2(0, 1);
            transform.position = transform.position + new Vector3(0, movement.y, 0) * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W) && transform.position.y > lowerBound)
        {
            // Move down
            Vector2 movement = new Vector2(0, -1);
            transform.position = transform.position + new Vector3(0, movement.y, 0) * speed * Time.deltaTime;
        }

    }
}
