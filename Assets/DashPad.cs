using UnityEngine;
public class DashPad : MonoBehaviour {
    public float speed = 50;
    void OnTriggerEnter(Collider col) {
        Rigidbody rb = col.GetComponent<Rigidbody>();
        Player p = col.GetComponent<Player>();
        if (!rb) return;
        rb.position = transform.position;
        rb.velocity = transform.forward * speed;
    }
    void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, transform.forward * speed * Time.deltaTime);
    }
}