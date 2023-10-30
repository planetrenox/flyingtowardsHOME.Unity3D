using System.Collections;
using Baracuda.Monitoring;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;

    [Monitor] [MPosition(UIPosition.LowerLeft)]
    private float _thrust;

    public float PitchRate = 10f;
    public float RollRate = 25f;
    public float BankRate = 10f;

    private const float Duration = .25f; // duration of change in rotation of plane
    private const float EaseInFactor = 2.0f; // exponent for ease-in function of plane rotation

    private void Start()
    {
        this.StartMonitoring();
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        this.StopMonitoring();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    private float _pitchInput;
    private float _rollInput;
    private float _bankInput;

    private void HandleInput()
    {
        var thrustInput = Input.GetKey(KeyCode.Mouse0) ? 5000f : Input.GetKey(KeyCode.Mouse1) ? -5000f : 0f;
        _thrust += thrustInput * Time.fixedDeltaTime;
        _thrust = Mathf.Clamp(_thrust, 0f, 30000f);

        _pitchInput = GetDeltaInput(KeyCode.W, KeyCode.S, PitchRate);
        _rollInput = GetDeltaInput(KeyCode.D, KeyCode.A, -RollRate);
        _bankInput = GetDeltaInput(KeyCode.E, KeyCode.Q, -BankRate);
    }

    private float GetDeltaInput(KeyCode positiveKey, KeyCode negativeKey, float speed)
    {
        var delta = 0f;
        if (Input.GetKey(positiveKey))
            delta += speed;
        if (Input.GetKey(negativeKey))
            delta -= speed;
        return delta * Time.fixedDeltaTime;
    }

    private void UpdatePhysics()
    {
        StartCoroutine(RotateWithEaseIn(Quaternion.Euler(_pitchInput, -_bankInput, _rollInput), Duration,
            EaseInFactor));
        _rigidbody.velocity = _transform.forward * (_thrust * Time.fixedDeltaTime);
    }

    private IEnumerator RotateWithEaseIn(Quaternion deltaRotation, float duration, float easeInFactor)
    {
        var elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            var t = elapsedTime / duration;
            t = Mathf.Pow(t, easeInFactor); // Apply an easing function

            // Perform a Slerp to interpolate between the current rotation and the target rotation
            var rotation = _rigidbody.rotation;
            _rigidbody.MoveRotation(Quaternion.Slerp(rotation, rotation * deltaRotation, t));

            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        // Ensure the final rotation matches the target rotation exactly
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }
}