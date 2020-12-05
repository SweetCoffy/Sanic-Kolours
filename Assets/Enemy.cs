using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour {
    public float health = 1;
    private float startHealth;
    public GameObject destroyEffect;
    public bool respawn = true;
    Vector3 originalPos;
    public float boostDrop = 5;
    public float respawnTime = 10;
    void Start() {
        startHealth = health;
        originalPos = transform.position;
    }
    protected virtual void OnCollisionEnter(Collision col) {
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                if (p.BounceOffEnemies) p.rb.velocity = Vector3.up * p.jumpForce;
                //p.stomp = false;
                p.doingHomingAttack = false;
                SendMessage("OnDamage", p, SendMessageOptions.DontRequireReceiver);
                TakeDamage(p);
            }
        }
    }
    protected virtual void OnTriggerEnter(Collider col) {
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                if (p.BounceOffEnemies) p.rb.velocity = (-p.rb.velocity * 0.25f) + Vector3.up * p.jumpForce;
                //p.stomp = false;
                p.doingHomingAttack = false;
                SendMessage("OnDamage", p, SendMessageOptions.DontRequireReceiver);
                TakeDamage(p);
            }
        }
    }
    IEnumerator Respawn() {
        yield return new WaitForSeconds(respawnTime);
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
        health = startHealth;
        transform.position = originalPos;
    }
    protected virtual void TakeDamage(Player p, float damage = 1) {
        health -= damage;
        Debug.Log("oof");
        if (health <= 0) {
            if (destroyEffect) Instantiate(destroyEffect, transform.position, transform.rotation);
            if (respawn) {
                GetComponent<Collider>().enabled = false;
                GetComponent<Renderer>().enabled = false;
                SendMessage("OnDie", p, SendMessageOptions.DontRequireReceiver);
                CameraThing.main.Shake(0.1f, 0.5f);
                if (p.boost < p.maxBoost * 3) p.boost += boostDrop;
                StartCoroutine(Respawn());
            } else {
                SendMessage("OnDie", p, SendMessageOptions.DontRequireReceiver);
                CameraThing.main.Shake(0.1f, 0.5f);
                if (p.boost < p.maxBoost) p.boost += boostDrop;
                Destroy(gameObject);
            }
        }
    }
}