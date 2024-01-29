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

    private GrappleHook grappleHook;

    // Start is called before the first frame update
    void Start()
    {
        grappleHook = GetComponent<GrappleHook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LeftRightMove();
    }

    private void LeftRightMove(){
        if(grappleHook.grappling) return;
        //movement + tilt
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed/100, 0, 0);
            visual.transform.rotation = Quaternion.Euler(90, 0, 75);
            Debug.Log("right");

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