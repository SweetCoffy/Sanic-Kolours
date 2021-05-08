using UnityEngine;
using System.Collections.Generic;
public class ControlsThing : MonoBehaviour {
    public ControlThing[] controls;
    public static ControlsThing main;
    public Dictionary<string, int> controlsPerActionID;
    void Start() {
        if (main != null) {
            Destroy(transform.parent.gameObject);
            return;
        }
        DontDestroyOnLoad(transform.parent.gameObject);
        controlsPerActionID = new Dictionary<string, int>();
        main = this;
        ControlThing[] h = GetComponentsInChildren<ControlThing>();
        controls = h;
    }
    public bool this[string k] {
        get {
            if (!controlsPerActionID.ContainsKey(k)) {
                int match = -1;
                for (int i = 0; i < controls.Length; i++) {
                    if (controls[i].actionID == k) {
                        match = i;
                        break;
                    }
                }
                if (match > -1) controlsPerActionID[k] = match;
            }
            return controls[controlsPerActionID[k]].gameObject.activeSelf;
        }
        set {
            if (!controlsPerActionID.ContainsKey(k)) {
                int match = -1;
                for (int i = 0; i < controls.Length; i++) {
                    if (controls[i].actionID == k) {
                        match = i;
                        break;
                    }
                }
                if (match > -1) controlsPerActionID[k] = match;
                else return;
            }
            controls[controlsPerActionID[k]].gameObject.SetActive(value);
        }
    }
}