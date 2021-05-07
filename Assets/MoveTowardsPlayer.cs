using UnityEngine;
public class MoveTowardsPlayer : BossMovement {
    public float range = 20;
    public BossMovement disableInRange;
    public bool useVelocity = true;
    protected virtual void Update() {
        if (!enabled) return;
        Player player = Player.main;
        if (!player) return;
        if (Vector3.Distance(rb.position, player.rb.position) > range) {
            disableInRange.enabled = true;
            return;
        } else disableInRange.enabled = false;
        Vector3 dir = (player.rb.position - rb.position).normalized;
        if (useVelocity) rb.velocity = dir * speed;
        else rb.AddForce(dir * speed);
    }
}