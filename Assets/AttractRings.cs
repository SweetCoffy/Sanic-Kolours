using UnityEngine;
public class AttractRings : MonoBehaviour {
    public float speed = 10;
    public bool enabled = true;
    void OnTriggerEnter(Collider col) {
        if (!enabled) return;
        Ring r = col.GetComponent<Ring>();
        if (r) {
           r.followTransform = transform;
           r.speed = speed;
        }
    }
}