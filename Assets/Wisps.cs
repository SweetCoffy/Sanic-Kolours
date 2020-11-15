using UnityEngine;
public class Wisps : MonoBehaviour{
    public HoverWisp hover;
    public RocketWisp rocket;
    public CubeWisp cube;
    public SlowmoWisp slowmo;
    public float shakeIntensity = 0.1f;
    public static Wisps main;
    void Start() {
        main = this;
    }
}