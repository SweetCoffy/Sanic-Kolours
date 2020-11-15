using UnityEngine;
[ExecuteInEditMode]
public class CameraThing : MonoBehaviour
{
    public Rigidbody target;
    public Vector3 offset;
    public float rotationSpeed = 2;
    public Vector3 rotationOffset;
    public float speed = 10;
    Vector3 currVeloc = Vector3.zero;
    public Vector3 intensity = Vector3.one;
    void FixedUpdate()
    {
        currVeloc = Vector3.Lerp(currVeloc, target.velocity, speed * Time.deltaTime);
        Vector3 h = currVeloc;
        h.Scale(intensity);
        transform.position = target.position + h + offset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(target.transform.localEulerAngles.x + rotationOffset.x, target.transform.localEulerAngles.y + rotationOffset.y, target.transform.localEulerAngles.z + rotationOffset.z), rotationSpeed * Time.deltaTime);
    }
}
