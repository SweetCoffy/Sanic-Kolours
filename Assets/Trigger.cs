using UnityEngine;
public class Trigger : MonoBehaviour {
    public TriggerObject[] objects;
    public bool disableOnExit = false;
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            foreach (TriggerObject o in objects) {
                o.behaviour.enabled = o.enabled;
            }
        }
    }
    void OnTriggerExit(Collider col) {
        if (!disableOnExit) return;
        Player p = col.GetComponent<Player>();
        if (p) {
            foreach (TriggerObject o in objects) {
                o.behaviour.enabled = !o.enabled;
            }
        }
    }
    [System.Serializable]
    public class TriggerObject {
        public MonoBehaviour behaviour;
        public bool enabled;
    }
}