using UnityEngine;

public class SmoothFollowWithSpring : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5.0f; // How fast the camera follows the target
    public float rotationSpeed = 5.0f; // How fast the camera rotates to match the target's rotation
    public float springDamping = 1.0f;
    public Vector3 offset = new(0, 2, -5); // Offset from the target's position

    private Quaternion _targetRotation;

    private void Start()
    {

        _targetRotation = transform.rotation;
    }

    private void Update()
    {

        // Calculate the desired camera position behind the target
        var targetPosition = target.position + target.TransformDirection(offset);

        // Smoothly move the camera toward the desired position
        var position = transform.position;
        position = Vector3.Lerp(position, targetPosition, followSpeed * Time.deltaTime);

        // Update the target's rotation
        _targetRotation = Quaternion.Slerp(_targetRotation, target.rotation, rotationSpeed * Time.deltaTime);

        // Smoothly rotate the camera to match the target's rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

        // Apply spring dampening effect
        var deltaPosition = targetPosition - position;
        position += deltaPosition * (springDamping * Time.deltaTime);
        transform.position = position;
    }
}