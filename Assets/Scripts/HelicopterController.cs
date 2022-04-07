using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    #region Movement
    public float forwardSpeed=500f;
    float horizontalThrust,forwardTilt,forwardTiltVelocity;
    #endregion
    #region Turn
    public float turnTorque=2.5f;
    float desiredYRotation,currentYRotation,rotationYVeclocity;
    #endregion
    #region DampingNdMovementControl
    Vector3 velocityToSmoothDampToZero;
    #endregion
    public Rigidbody rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        Turn();
        ClampSpeed();
        rb.AddRelativeForce(Vector3.up*horizontalThrust);
        rb.rotation=Quaternion.Euler(new Vector3(forwardTilt,currentYRotation,rb.rotation.z));
    }
    void MovementUpDown()
    {
        if(Input.GetKey(KeyCode.I))
        {
            horizontalThrust=450;
        }else if(Input.GetKey(KeyCode.K))
        {
            horizontalThrust=-200;
        }else if(!Input.GetKey(KeyCode.I)&&!Input.GetKey(KeyCode.K))
        {
            horizontalThrust=98.1f;
        }
    }
    void MovementForward()
    {
        if(Input.GetAxis("Vertical")!=0)
        {
            float yInput=Input.GetAxis("Vertical");
            rb.AddRelativeForce(Vector3.forward*yInput*forwardSpeed);
            forwardTilt=Mathf.SmoothDamp(forwardTilt,20*yInput,ref forwardTiltVelocity,0.1f);
        }
    }
    void Turn()
    {
        if(Input.GetKey(KeyCode.J))
        {
            desiredYRotation -= turnTorque;
        }
        if(Input.GetKey(KeyCode.L))
        {
            desiredYRotation += turnTorque;
        }
        currentYRotation = Mathf.SmoothDamp(currentYRotation, desiredYRotation, ref rotationYVeclocity,0.25f);
    }
    void ClampSpeed()
    {
        var yInput=Input.GetAxis("Vertical");
        var xInput=Input.GetAxis("Horizontal");
        if(Mathf.Abs(yInput)>0.2f&&Mathf.Abs(xInput)>0.2f)
        {
            rb.velocity=Vector3.ClampMagnitude(rb.velocity,Mathf.Lerp(rb.velocity.magnitude,10f,Time.deltaTime*5f));
        }
        if(Mathf.Abs(yInput)>0.2f&&Mathf.Abs(xInput)<0.2f)
        {
            rb.velocity=Vector3.ClampMagnitude(rb.velocity,Mathf.Lerp(rb.velocity.magnitude,10f,Time.deltaTime*5f));
        }
        if(Mathf.Abs(yInput)<0.2f&&Mathf.Abs(xInput)>0.2f)
        {
            rb.velocity=Vector3.ClampMagnitude(rb.velocity,Mathf.Lerp(rb.velocity.magnitude,5f,Time.deltaTime*5f));
        }
        if(Mathf.Abs(yInput)<0.2f&&Mathf.Abs(xInput)<0.2f)
        {
            rb.velocity=Vector3.SmoothDamp(rb.velocity,Vector3.zero,ref velocityToSmoothDampToZero,0.95f);
        }
    }
}
