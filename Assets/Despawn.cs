using UnityEngine;
public class Despawn : MonoBehaviour {
    public float time = 10;
    void Start() {
        Destroy(gameObject, time);
    }
}