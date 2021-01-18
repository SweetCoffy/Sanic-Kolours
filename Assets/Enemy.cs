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
    public float parentDamageMultiplier = 1;
    void Start() {
        startHealth = health;
        originalPos = transform.position;
    }
    protected virtual void OnCollisionEnter(Collision col) {
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                if (p.BounceOffEnemies) p.rb.velocity = (p.rb.velocity.magnitude / 2) * p.transform.up;
                float damage = 1;
                if (p.stomp) damage += p.stompDamage;
                if (p.isSuper) damage += p.superDamage;
                if (p.isBoosting) damage += p.boostDamage;
                TakeDamage(p, damage);
                SendMessage("OnDamage", p, SendMessageOptions.DontRequireReceiver);
                p.doingHomingAttack = false;
            }
        }
    }
    protected virtual void OnTriggerEnter(Collider col) {
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                if (p.BounceOffEnemies) p.rb.velocity = (p.rb.velocity.magnitude / 2) * p.transform.up;
                float damage = 1;
                if (p.stomp) damage += p.stompDamage;
                if (p.isSuper) damage += p.superDamage;
                if (p.isBoosting) damage += p.boostDamage;
                TakeDamage(p, damage);
                SendMessage("OnDamage", p, SendMessageOptions.DontRequireReceiver);
                p.doingHomingAttack = false;
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
        if (transform.parent) {
            Enemy e = transform.parent.GetComponent<Enemy>();
            if (e) e.TakeDamage(p, damage * parentDamageMultiplier);
        }
        if (health <= 0) {
            if (destroyEffect) Instantiate(destroyEffect, transform.position, transform.rotation);
            ZoneInfo.current.SlowMotion(0.2f, Mathf.Clamp(damage, 0, startHealth) * 0.12f);
            if (respawn) {
                GetComponent<Collider>().enabled = false;
                GetComponent<Renderer>().enabled = false;
                SendMessage("OnDie", p, SendMessageOptions.DontRequireReceiver);
                CameraThing.main.Shake(0.15f * damage, 0.4f);
                if (p.boost < p.maxBoost) p.boost += boostDrop;
                StartCoroutine(Respawn());
            } else {
                SendMessage("OnDie", p, SendMessageOptions.DontRequireReceiver);
                CameraThing.main.Shake(0.15f * damage, 0.4f);
                if (p.boost < p.maxBoost) p.boost += boostDrop;
                Destroy(gameObject);
            }
        }
    }
}