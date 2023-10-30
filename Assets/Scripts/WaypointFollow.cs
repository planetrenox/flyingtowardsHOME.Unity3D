using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    public Transform target;


    // Update is called once per frame
    private void Update()
    {
        transform.position = target.position;
    }
}
