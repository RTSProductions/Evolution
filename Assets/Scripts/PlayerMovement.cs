using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
     Rigidbody rb;

    public float fowardForce = 50f;

    public float sidewaysForce = 50f;

    NavMeshAgent agent;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

            

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(0, 0, fowardForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(0, 0, -fowardForce * Time.deltaTime, ForceMode.VelocityChange);
            }




    }

    public void Take()
    {
        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();

        rb.isKinematic = false;

        agent.enabled = false;
    }

    public void UnTake()
    {
        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();

        rb.isKinematic = true;

        agent.enabled = true;
    }



}
