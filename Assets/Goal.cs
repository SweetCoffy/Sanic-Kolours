using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            StageLoader.main.EndStage();
        }
    }
}
