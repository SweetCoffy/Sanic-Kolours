using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDModeTrigger : MonoBehaviour
{
    public bool changeZPos = false;
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            p.TwoDMode = true;
            if (changeZPos) p.twoDModeZ = transform.position.z;
        }
    }
    void OnTriggerExit(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            p.twoDModeZ = 0;
            p.TwoDMode = false;
        }
    }
}
