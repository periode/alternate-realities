using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed;

    private Vector3 MovingDirection = Vector3.up;


    void FixedUpdate()
    {
        gameObject.transform.Translate(MovingDirection * Time.smoothDeltaTime * speed);

        if (gameObject.transform.position.y > 3)
        {
            MovingDirection = Vector3.down;
        }
        else if (gameObject.transform.position.y < 0.5f)
        {
            MovingDirection = Vector3.up;
        }
    }
}
