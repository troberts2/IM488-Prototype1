using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private float mobSpeed;

    private GameObject cheese;
    private Rigidbody rb;

    private LossCondition loss;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cheese = FindObjectOfType<Movement>().gameObject;
        loss = FindObjectOfType<LossCondition>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position = Vector3.MoveTowards(rb.position, cheese.transform.position, mobSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            loss.Lose();
        }
    }
}
