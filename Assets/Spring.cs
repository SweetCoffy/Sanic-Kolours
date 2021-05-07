using UnityEngine;
public class Spring : MonoBehaviour {
    public float force = 50;
    public bool forcePosition = true;
    Settings settings;
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
    public Vector3 PlotTrajectoryAtTime (Vector3 start, Vector3 startVelocity, float time) {
        return start + startVelocity*time + Settings.main.gravity*time*time*0.5f;
    }
    public void PlotTrajectory (Vector3 start, Vector3 startVelocity, float timestep, float maxTime) {
        Vector3 prev = start;
        for (int i=1;;i++) {
            float t = timestep*i;
            if (t > maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime (start, startVelocity, t);
            RaycastHit hitInfo;
            bool didHit = false;
            if (Physics.Linecast (prev,pos,out hitInfo,Physics.AllLayers)) {
                pos = hitInfo.point;
                didHit = true;
            }
            Debug.DrawLine (prev,pos,Color.red);
            prev = pos;
            if (didHit) break;
        }
    }
    void OnDrawGizmos() {
        if (Settings.main.calculateSpringTrajectory) PlotTrajectory(transform.position, transform.up * force, .15f, 1.25f);
        else Gizmos.DrawLine(transform.position, transform.position + (transform.up * force));
    }
}