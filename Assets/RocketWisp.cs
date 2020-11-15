using UnityEngine;
[System.Serializable]
public class RocketWisp : Wisp {
    public float maxSpeed = 50;
    public override void Start() {
        base.Start();
        player.inputEnabled = false;
    }
    public override void Update() {
        base.Update();
        float targetVelocity = (1 - (timeLeft / duration)) * maxSpeed;
        player.rb.velocity = player.transform.up * targetVelocity;
    }
    public override void End() {
        base.End();
        player.inputEnabled = true;
        player.rb.velocity = Vector3.zero;
    }
}