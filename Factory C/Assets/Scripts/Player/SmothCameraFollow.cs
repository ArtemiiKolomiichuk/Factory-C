using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;   
    public float smoothSpeed = 0.125f;
    public float rott;
    //public Quaternion rotation;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
        transform.rotation = Quaternion.Euler(rott, 0, 0); 

        //transform.LookAt(target);
    }
    void Start()
    {
        
        transform.rotation = Quaternion.Euler(rott, 0, 0); 
    }
}