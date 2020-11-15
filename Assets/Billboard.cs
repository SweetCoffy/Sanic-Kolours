using UnityEngine;
public class Billboard : MonoBehaviour {
    public float multiplier = 1;
    void Update() {
        transform.forward = Camera.main.transform.forward * multiplier;
    }
}