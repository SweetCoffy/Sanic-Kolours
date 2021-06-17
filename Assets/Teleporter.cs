using UnityEngine;
public class Teleporter : MonoBehaviour {
    public Transform target;
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            p.rb.position = target.position;
        }
    }
}