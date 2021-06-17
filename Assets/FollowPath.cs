using UnityEngine;
public class FollowPath : BossMovement {
    public enum FollowPathRepeatMode {
        LOOP,
        REVERSE,
        NONE,
    }
    public Transform[] waypoints;
    public bool useVelocity = true;
    public FollowPathRepeatMode mode = FollowPathRepeatMode.LOOP;
    int add = 1;
    bool move = true;
    public float distanceThreshold = 1;
    int curWaypoint = 0;
    protected override void Start() {/**/
        base.Start();
        Transform waypointsChild = null;
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.name == "_Waypoints") waypointsChild = transform.GetChild(i);
        }
        if (waypointsChild) {
            waypoints = new Transform[waypointsChild.childCount];
            int count = waypointsChild.childCount;
            for (int i = 0; i < count; i++) {
                Transform c = waypointsChild.GetChild(i);
                waypoints[i] = c;
            }
            waypointsChild.parent = null;
        }
    }
    protected virtual void FixedUpdate() {
        if (!enabled) return;
        Transform cur = waypoints[curWaypoint];
        if (cur == null) {
            curWaypoint++;
            if (curWaypoint > waypoints.Length - 1) curWaypoint = 0;
            return;
        }
        float dist = Vector3.Distance(cur.position, rb.position);
        if (move && useVelocity) rb.velocity = (cur.position - rb.position).normalized * speed;
        else if (!useVelocity && move) rb.AddForce((cur.position - rb.position).normalized * speed);
        if (dist < distanceThreshold) {
            curWaypoint += add;
            if (curWaypoint > waypoints.Length - 1) {
                if (mode == FollowPathRepeatMode.LOOP) curWaypoint = 0;
                else if (mode == FollowPathRepeatMode.REVERSE) {
                    curWaypoint--;
                    add = -add;
                }
                else if (mode == FollowPathRepeatMode.NONE) {
                    move = false;
                }
            }
        }
    }
}