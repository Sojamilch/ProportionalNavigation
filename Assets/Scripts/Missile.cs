using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum GuidanceModes { LineOfSight, Simple, True,}
public class Missile : MonoBehaviour
{

    private Rigidbody rb;
    public GuidanceModes mode;

    public Transform target;

    private float distance;

    // public float burnTime;
    public float fieldOfView;
    public float proximityFuseRange;
    public float navigationConstant;
    public float acceleration;
    public float maxSpeed;
    public float turnRate;
    public bool fire = false;
    public Vector3 lineOfSightPrevious;
    public float navigationDelay;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (fire == true)
        {
            if (target.gameObject.activeSelf != false && IsVisible())
            {
                distance = Vector3.Distance(transform.position, target.position);

                //To prevent any bugs where distance = 0 on creation
                if (distance < proximityFuseRange)
                {
                    Detonate();

                }

                if (distance > proximityFuseRange)
                {
                    if(navigationDelay <= 0)
                    {
                        Vector3 newDirection = Vector3.zero;

                        Debug.Log(mode);

                        switch (mode)
                        {
                            case GuidanceModes.True:
                                newDirection = TrueProportionalNavigation();
                                break;
                            case GuidanceModes.Simple: 
                                newDirection = SimpleProportionalNavigation(); 
                                break;
                            case GuidanceModes.LineOfSight:
                                newDirection = LineOfSightNavigation();
                                break;

                        }

                        Steer(newDirection);
                    }
                    else
                    {
                        navigationDelay -= Time.deltaTime;
                    }

                   
                    Thrust();
                }
            }
            else
            {
                // Self Destruct when target lost.
                Destroy(gameObject);
            }

        }


    }

    void Steer(Vector3 requiredAcceleration)
    {

        Quaternion newDirection = Quaternion.LookRotation(requiredAcceleration);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newDirection, turnRate);
    }
    void Thrust()
    {
        rb.AddForce(transform.forward * acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
   
    Vector3 TrueProportionalNavigation()
    {

        Vector3 lineOfSight = (target.position - transform.position).normalized;

        Vector3 lineOfSightDelta = lineOfSight - lineOfSightPrevious.normalized;

        float lineOfSightRate = getLineOfSightRate(lineOfSight, lineOfSightDelta);

        float closingRate = (lineOfSightDelta.magnitude) / Time.deltaTime;

        // Assume acceleration of at least 1G in any direction
        float targetAcceleration = 9.8f * Time.deltaTime;

        // A = (LOS * Vc * LOS_Rate * Nc) + (LOS_Delta * Nc / 2 * Ta)
        Vector3 requiredAcceleration = lineOfSight * closingRate * lineOfSightRate * navigationConstant + lineOfSightDelta * ((navigationConstant * targetAcceleration) / 2);

        return requiredAcceleration;


    }

    Vector3 LineOfSightNavigation()
    {
        lineOfSightPrevious = (target.position - transform.position).normalized;
        return lineOfSightPrevious;
    }

    Vector3 SimpleProportionalNavigation()
    {
        Vector3 lineOfSight = (target.position - transform.position).normalized;

        Vector3 lineOfSightDelta = lineOfSight - lineOfSightPrevious.normalized;

        float lineOfSightRate = getLineOfSightRate(lineOfSight, lineOfSightDelta);

        return navigationConstant * lineOfSightRate * lineOfSight;
    }

    float getLineOfSightRate(Vector3 lineOfSight, Vector3 lineOfSightDelta)
    {

        float lineOfSightRate = lineOfSightDelta.magnitude;

        lineOfSightPrevious = lineOfSight;

        return lineOfSightRate;
    }

    bool IsVisible()
    {

        var angle = Vector3.Angle(target.position, lineOfSightPrevious);

        if (angle < fieldOfView)
        {
            LogMessage("Tracking" + " \n Angle: " + angle + "\n Distance:  " + distance);
            // Debug.Log();
            return true;
        }
        else
        {

            LogMessage("Lost" + " \n Angle: " + angle + "\n Distance:  " + distance);
            return false;
        }



    }

    private void OnCollisionEnter(Collision collision)
    {

        Destroy(gameObject);

    }

    void Detonate()
    {
        LogMessage("Target Destroyed");
        target.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void LogMessage(string message)
    {
        TextMeshProUGUI textbox = GameObject.Find("Missile Log").GetComponent<TextMeshProUGUI>();
        textbox.text = message;


    }
}

