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
    private float curSpeed;
    public float groundedCap;
    public float boostedCap;
    public float airborneCap;
    public float boostTime;

    internal float hp;
    internal float hpMax;

    private Vector3 curVelocity;

    private bool grounded;
    private bool boostFloor;
    private bool boosted;
    private bool immune;
    private bool stagnant;
    [SerializeField] private bool debugTickDamageOff;

    public GameObject visual; //defined in prefab
    private Material cheese;

    private GrappleHook grappleHook;
    private HealthManager health;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grappleHook = GetComponent<GrappleHook>();
        health = GetComponentInChildren<HealthManager>();

        curSpeed = moveSpeed;
        hpMax = 3; //make public later so that design can change?
        hp = hpMax;

        cheese = visual.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grappleHook.isSlowed)
        {
            moveSpeed = curSpeed / 2;
        }
        else
        {
            moveSpeed = curSpeed;
        }

        LeftRightMove();
        CapVelocity();

        if (boostFloor)
        {
            boosted = true;
        }

        if (rb.velocity.magnitude == 0)
        {
            if (!stagnant)
            {
                stagnant = true;
                StartCoroutine(HealthTick());
            }
        }
        else
        {
            stagnant = false;
        }
    }

    private void LeftRightMove()
    {
        if (grappleHook.grappling) return;
        //movement + tilt
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(moveSpeed, 0, 0));
            rb.MovePosition(rb.position + Vector3.right * Time.deltaTime);
            visual.transform.rotation = Quaternion.Lerp(visual.transform.rotation, Quaternion.Euler(90, 0, 65), 0.1f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-moveSpeed, 0, 0));
            rb.MovePosition(rb.position + Vector3.left * Time.deltaTime);
            visual.transform.rotation = Quaternion.Lerp(visual.transform.rotation, Quaternion.Euler(90, 0, 115), 0.1f);
        }
        else
        {
            rb.velocity.Normalize();
            visual.transform.rotation = Quaternion.Lerp(visual.transform.rotation, Quaternion.Euler(90, 0, 90), 0.1f);
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
            boostFloor = true;
        }
        else
        {
            boostFloor = false;
        }

        if (collision.gameObject.tag == "Obstacle" && !immune)
        {
            StartCoroutine("Hit");
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        boostFloor = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boost")
        {
            boosted = true;
            StartCoroutine("Boost");
        }
    }

    IEnumerator Boost()
    {
        rb.velocity = new Vector3(0, -boostedCap, boostedCap);

        hp++;
        health.UpdateHealth();

        yield return new WaitForSecondsRealtime(boostTime);

        boosted = false;
    }

    IEnumerator Hit()
    {
        hp--;
        immune = true;
        health.UpdateHealth();
        if(hp > 0){
            health.flashImage.image.enabled = true;
            health.flashImage.StartFlash(0.5f, 0.5f, Color.red);
        }
        

        StartCoroutine(Blink());

        yield return new WaitForSecondsRealtime(3);

        immune = false;
        cheese.color = new Color(cheese.color.r, cheese.color.g, cheese.color.b, 1);
    }

    IEnumerator Blink()
    {
        while (immune)
        {
            cheese.color = new Color(cheese.color.r, cheese.color.g, cheese.color.b, 0.5f);
            yield return new WaitForSeconds(0.25f);
            cheese.color = new Color(cheese.color.r, cheese.color.g, cheese.color.b, 1);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator HealthTick()
    {
        yield return new WaitForSecondsRealtime(3);
        if (stagnant && !debugTickDamageOff)
        {
            hp--;
            health.UpdateHealth();
            StartCoroutine(HealthTick());
        }
    }
}
