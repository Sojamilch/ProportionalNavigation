using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Rigidbody targetRb;
    public Transform target;
    public Camera firstPersonCamera;
    public Camera topDownCamera;


    // Update is called once per frame
    void Update()
    {

        TrackTarget();

    }

    void TrackTarget()
    {
        if (target != null)
        {
            firstPersonCamera.transform.LookAt(target);
        }
    }

    public void SwapPerspective()
    {
        if(firstPersonCamera.gameObject.activeSelf == false)
        {
            firstPersonCamera.gameObject.SetActive(true);
            topDownCamera.gameObject.SetActive(false);
        }
        else
        {
            firstPersonCamera.gameObject.SetActive(false);
            topDownCamera.gameObject.SetActive(true);
        }

    }

}
