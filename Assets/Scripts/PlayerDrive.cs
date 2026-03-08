using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrive : MonoBehaviour
{
    private float steering = 120;
    private float speed = 10;

    bool isDriving = false;
    float slowTimer = 0f;


    void Update()
    {
        if (!isDriving)
            return;

        float x = 0, y = 0;

        // get forward/backward input
        y = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // if vehicle is moving, allow steering
        if (Input.GetAxis("Vertical") != 0)
            x = Input.GetAxis("Horizontal") * steering * Time.deltaTime;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, y);

        if (Time.time > slowTimer)
        {
            speed = 10;
            steering = 120;
        }
        else
        {
            speed = 2;
            steering = 50;
        }
    }

    public void SetToDrive()
    {
        isDriving = true;
    }

    public void StopDrive()
    {
        isDriving = false;
    }
}
