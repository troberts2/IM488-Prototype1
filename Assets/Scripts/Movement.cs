///
/// Attached to the player cheese wheel
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed; //a good movespeed is 2;
    public float groundedCap;
    public float boostedCap;
    public float airborneCap;

    private Vector3 curVelocity;

    private bool grounded;
    private bool velocityCapMet;
    private bool boosted;

    public GameObject visual; //defined in prefab

    private GrappleHook grappleHook;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grappleHook = GetComponent<GrappleHook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LeftRightMove();
        CapVelocity();
    }

    private void LeftRightMove(){
        if(grappleHook.grappling) return;
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

    private void CapVelocity()
    {
        if (grounded)
        {
            if (boosted)
            {
                if (rb.velocity.magnitude >= boostedCap)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, boostedCap);
                }
            }
            else
            {
                if (rb.velocity.magnitude >= groundedCap)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, groundedCap);
                }
            }
        }
        else
        {
            if (rb.velocity.magnitude >= airborneCap)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, airborneCap);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        if (collision.gameObject.layer == 8)
        {
            boosted = true;
            Debug.Log("you're boosted!");
        }
        else
        {
            boosted = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        boosted = false;
    }
}