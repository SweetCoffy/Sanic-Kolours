using UnityEngine;
public class Despawn : MonoBehaviour {
    public float time = 10;
    void Start() {/*Debug.Log("Start");*/
        Destroy(gameObject, time);
    }
}