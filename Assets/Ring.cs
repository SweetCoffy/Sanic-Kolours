using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject particles;
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            if (particles) Instantiate(particles, transform.position, Quaternion.identity);
            p.rings++;
            Destroy(gameObject);
        }
    }
}
