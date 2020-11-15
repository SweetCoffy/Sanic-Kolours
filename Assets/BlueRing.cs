using UnityEngine;
public class BlueRing : MonoBehaviour {
    public Renderer enabledRenderer;
    public Collider col;
    void Update() {
        enabledRenderer.enabled = Game.cubeEnabled;
        col.enabled = Game.cubeEnabled;
    }
}