using UnityEngine;
public class Spring : MonoBehaviour {
    public float force = 50;
    public bool forcePosition = true;
    void OnTriggerEnter(Collider col) {
        Rigidbody rb = col.GetComponent<Rigidbody>();
        Player p = col.GetComponent<Player>();
        if (p) {
            p.didJump = false;
            p.spring = true;
            p.spinning = false;
        }
        if (!rb) return;
        if (forcePosition) rb.position = transform.position;
        rb.velocity = transform.up * force;
    }
    void OnDamage(Player p) {
        Rigidbody rb = p.rb;
        p.didJump = false;
        p.spring = true;
        p.spinning = false;
        if (!rb) return;
        if (forcePosition) rb.position = transform.position;
        rb.velocity = transform.up * force;
    }
    void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, transform.up * force);
    }
}