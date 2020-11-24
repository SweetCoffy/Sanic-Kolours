using UnityEngine;
public class BlueRing : MonoBehaviour {
    public Renderer enabledRenderer;
    public bool invert = false;
    public Collider col;
    void Update() {
        if (!invert) {
            enabledRenderer.enabled = Game.cubeEnabled;
            col.enabled = Game.cubeEnabled;
        } else {
            enabledRenderer.enabled = !Game.cubeEnabled;
            col.enabled = !Game.cubeEnabled;
        }
    }
}