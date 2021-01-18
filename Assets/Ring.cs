using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject particles;
    public GameObject destroyObject;
    public Transform followTransform;
    public float speed = 5;
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            if (particles) Instantiate(particles, transform.position, Quaternion.identity);
            p.rings++;
            if (!destroyObject) Destroy(gameObject);
            else Destroy(destroyObject);
        }
    }
    void FixedUpdate() {
        if (!followTransform) return;
        if (!destroyObject) transform.position = Vector3.Lerp(transform.position, followTransform.position, speed * Time.deltaTime);
        else destroyObject.transform.position = Vector3.Lerp(destroyObject.transform.position, followTransform.position, speed * Time.deltaTime);
    }
}
