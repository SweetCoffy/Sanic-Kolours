using UnityEngine;
[CreateAssetMenu(fileName = "Settings")]
public class Settings : ScriptableObject {
    public Vector3 gravity;
    public bool calculateSpringTrajectory = true;
    static Settings r;
    public float shakeIntensity = 2.1f;
    public float ringsShakeIntensity = 2.1f;
    public static Settings main {
        get {
            if (!r) {
                return r = Resources.Load<Settings>("Settings");
            } else {
                return r;
            }
        }
    }
}