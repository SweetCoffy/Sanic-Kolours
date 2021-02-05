using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody rb;
    Enemy e;
    public float speed = 100;
    public float fleeThreshold = 1;
    void Start() {
        rb = GetComponent<Rigidbody>();
        e = GetComponent<Enemy>();
    }
    void FixedUpdate() {
        if (Player.main) if (Player.main.dead) return;
        Transform player = Player.main.transform;
        Vector3 dir = player.position - rb.position;
        Vector3 force = (dir * speed);
        force.Scale(Vector3.right + Vector3.forward);
        if ((e && e.health < fleeThreshold) || Player.main.isSuper) force = -force; 
        Vector3 v = rb.velocity;
        v.Scale(Vector3.right + Vector3.forward);
        transform.forward = v;
        rb.AddForce(force, ForceMode.Acceleration);
    }
}
