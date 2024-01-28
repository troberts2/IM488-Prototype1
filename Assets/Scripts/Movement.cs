///
/// Attached to the player cheese wheel
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Right now a good movespeed is 2;
    public float moveSpeed;
    //defined in prefab
    public GameObject visual;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //movement + tilt
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed/100, 0, 0);
            visual.transform.rotation = Quaternion.Euler(90, 0, 75);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-moveSpeed/100, 0, 0);
            visual.transform.rotation = Quaternion.Euler(90, 0, 105);
        }
        else
        {
            visual.transform.rotation = Quaternion.Euler(90, 0, 90);
        }
    }
}