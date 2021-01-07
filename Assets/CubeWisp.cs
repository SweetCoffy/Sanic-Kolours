using UnityEngine;
[System.Serializable]
public class CubeWisp : Wisp {
    private bool h = false;
    public override void Start() {
        base.Start();
        //Game.cubeEnabled = !Game.cubeEnabled;
        player.rb.velocity = Vector3.down * 75;
        consumeRate = 0.5f;
        Game.cubeEnabled = !Game.cubeEnabled;
        player.inputEnabled = false;
    }
    public override void Update() {
        base.Update();
        if (player.isGrounded && !h) {
            consumeRate = 1;
            player.inputEnabled = true;
            CameraThing.main.Shake(0.3f, 0.5f);
            h = true;
        }
    }
    public override void WispPowerButtonDown() {
        if (player.isGrounded) return;
        consumeRate = 0;
        player.rb.velocity = Vector3.down * 75;
        Game.cubeEnabled = !Game.cubeEnabled;
        player.inputEnabled = false;
        h = false;
    }
    public override void End() {
        base.End();
        Game.cubeEnabled = false;
        player.inputEnabled = true;
    }
}
