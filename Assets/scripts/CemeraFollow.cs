using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemeraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 rotationOffset;


    private void Update()
    {
        Vector3 desiredPosition = target.position + offset ;
        Vector3 smothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smothedPosition;
       // transform.LookAt(target);
    }
}
