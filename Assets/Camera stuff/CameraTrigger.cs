using UnityEngine;
using System.Collections.Generic;
public class CameraTrigger : MonoBehaviour {
    public Transform reference;
    CameraThing thing;
    bool triggered = false;
    public CameraType[] types;
    Player player;
    void Start() {
        thing = CameraThing.main;
        CameraType[] t = GetComponents<CameraType>();
        this.types = t;
        foreach (CameraType type in types) {
            type.reference = reference;
            type.trigger = this;
        }
    }
    void Update() {
        if (triggered) {
            foreach (CameraType type in types) {
                type.player = player;
                type.deltaTime = Time.deltaTime;
                type.cam = CameraThing.main;
                type.CameraUpdate();
            }   
        } else {
            foreach (CameraType type in types) {
                type.CameraPostEnd();
            }   
        }
    }
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            player = p;
            triggered = true;
            foreach (CameraType type in types) {
                type.player = player;
                type.cam = CameraThing.main;
                type.CameraStart();
            }   
        }
    }
    void OnTriggerExit(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            foreach (CameraType type in types) {
                type.player = player;
                type.cam = CameraThing.main;
                type.CameraEnd();
            }   
            player = null;
            triggered = false;
        }
    }
}