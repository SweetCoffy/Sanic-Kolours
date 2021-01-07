using UnityEngine;
[System.Serializable]
public class HoverWisp : Wisp {
    public float speed = 250;
    public float hoverForce = 250;
    public override void Start() {
        base.Start();
        player.inputEnabled = false;
        player.GetComponent<ConstantForce>().relativeForce *= 0.25f;
    }
    public override void Update() {
        base.Update();
        if (Input.GetButton("Jump")) {
            player.rb.AddForce(Vector3.up * hoverForce * Time.deltaTime);
            timeLeft -= Time.deltaTime;
        }
        player.rb.AddRelativeForce((player.camHolder.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime) + (player.camHolder.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime));
    }
    public override void End() {
        base.End();
        player.inputEnabled = true;
        player.GetComponent<ConstantForce>().relativeForce *= 4;
    }
}