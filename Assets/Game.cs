using UnityEngine;
[ExecuteInEditMode]
public class Game : MonoBehaviour{
    public static bool cubeEnabled = false;
    void Start() {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}