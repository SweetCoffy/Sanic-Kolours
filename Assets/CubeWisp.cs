using UnityEngine;
[System.Serializable]
public class CubeWisp : Wisp {
    //float originalDrag;
    //public Color boostColor = Color.red;
    public override void Start() {
        base.Start();
        Game.cubeEnabled = !Game.cubeEnabled;
    }
    public override void End() {
        base.End();
        Game.cubeEnabled = false;
    }
}
