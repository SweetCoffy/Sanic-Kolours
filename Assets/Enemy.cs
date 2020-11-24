using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour {
    public float health = 1;
    public GameObject destroyEffect;
    public bool respawn = true;
    public float respawnTime = 10;
    protected virtual void OnCollisionEnter(Collision col) {
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                p.rb.velocity = Vector3.up * p.jumpForce;
                p.stomp = false;
                p.doingHomingAttack = false;
                TakeDamage();
            }
        }
    }
    protected virtual void OnTriggerEnter(Collider col) {
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                p.rb.velocity = Vector3.up * p.jumpForce;
                p.stomp = false;
                p.doingHomingAttack = false;
                TakeDamage();
            }
        }
    }
    IEnumerator Respawn() {
        yield return new WaitForSeconds(respawnTime);
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }
    protected virtual void TakeDamage(float damage = 1) {
        health -= damage;
        Debug.Log("oof");
        if (health <= 0) {
            if (destroyEffect) Instantiate(destroyEffect, transform.position, Quaternion.identity);
            if (respawn) {
                GetComponent<Collider>().enabled = false;
                GetComponent<Renderer>().enabled = false;
                StartCoroutine(Respawn());
            } else {
                Destroy(gameObject);
            }
        }
    }
}