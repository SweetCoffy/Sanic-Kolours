using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeShadowCaster : MonoBehaviour
{
    public LayerMask groundMask;
    public Transform fakeShadow;
    public float distance = 0.1f;
    public float maxTimeSinceLastRaycast = 0.15f;
    float secondsSinceLastRaycast = 0;
    void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.up * distance);
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, -Vector3.up, out hit, distance, groundMask);
        if (didHit) {
            fakeShadow.gameObject.SetActive(true);
            Quaternion h = Quaternion.FromToRotation(Vector3.up, hit.normal.normalized);
            fakeShadow.position = hit.point;
            fakeShadow.rotation = h;
            secondsSinceLastRaycast = 0;
        } else if (secondsSinceLastRaycast > maxTimeSinceLastRaycast){
            fakeShadow.gameObject.SetActive(false);
        }
    }
}
