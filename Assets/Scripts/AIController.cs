using UnityEngine;
using UnityEngine.Serialization;

public class AIController : MonoBehaviour
{
    public Transform target;
    public float speed = 100.0f;
    public float turnSpeed = 10.0f;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Calculate the direction to the target
        var targetDirection = target.position - transform.position;
        targetDirection.Normalize();
        var desiredVelocity = targetDirection * speed;
        var velocityError = desiredVelocity - _rb.velocity;
        var thrustForce = velocityError * speed;
        _rb.AddForce(thrustForce);
        var desiredRotation = Quaternion.LookRotation(targetDirection);
        _rb.rotation = Quaternion.Slerp(_rb.rotation, desiredRotation, turnSpeed * Time.fixedDeltaTime);
    }
}