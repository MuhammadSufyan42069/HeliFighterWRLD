using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
    [SerializeField]
    Transform targetToFollow,targetToLookAt;

    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float smoothTime=0.15f;

    Vector3 velocity;
    void FixedUpdate()
    {
        Vector3 desiredPosition = targetToFollow.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
        transform.position = smoothedPosition;

        transform.LookAt(targetToLookAt);
    }
}
