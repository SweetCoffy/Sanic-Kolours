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
    public bool damagePlayer = false;
    public float knockback = 13;
    public float parentDamageMultiplier = 1;
    bool respawning = false;
    float invincibility = 1;
    void Start() {
        startHealth = health;
        originalPos = transform.position;
    }
    void Update() {
        if (transform.position.y < -600) {
            transform.position = originalPos;
            health = startHealth;
        }
        if (invincibility > 0) invincibility -= Time.deltaTime;
    }
    protected virtual void OnCollisionStay(Collision col) {
        if (invincibility > 0) return;
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                if (p.doingHomingAttack) p.rb.velocity = (p.rb.velocity.magnitude / 1.7f) * p.transform.up;
                else if (p.BounceOffEnemies) p.rb.velocity = -p.rb.velocity / 1.8f;
                float damage = 1;
                if (p.stomp) damage += p.stompDamage;
                if (p.isSuper) damage += p.superDamage;
                if (p.isBoosting) damage += p.boostDamage;
                TakeDamage(p, damage);
                p.doingHomingAttack = false;
            } else if (damagePlayer) {
                p.TakeDamage(-p.facing * knockback);
            }
        }
    }
    protected virtual void OnTriggerStay(Collider col) {
        if (invincibility > 0) return;
        Player p = col.gameObject.GetComponent<Player>();
        if (p) {
            Debug.Log("h");
            if (p.DestroyEnemies) {
                if (p.doingHomingAttack) p.rb.velocity = (p.rb.velocity.magnitude / 1.7f) * p.transform.up;
                else if (p.BounceOffEnemies) p.rb.velocity = -p.rb.velocity / 1.8f;
                float damage = 1;
                if (p.stomp) damage += p.stompDamage;
                if (p.isSuper) damage += p.superDamage;
                if (p.isBoosting) damage += p.boostDamage;
                TakeDamage(p, damage);
                p.doingHomingAttack = false;
            } else if (damagePlayer) {
                p.TakeDamage(-p.facing * knockback);
            }
        }
    }
    IEnumerator Respawn() {
        respawning = true;
        yield return new WaitForSeconds(respawnTime);
        GetComponent<Collider>().enabled = true;
        if (GetComponent<Renderer>()) {
            GetComponent<Renderer>().enabled = true;
        }
        if (transform.childCount > 0) {
            for (var i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        health = startHealth;
        transform.position = originalPos;
        respawning = false;
    }

    protected virtual void TakeDamage(Player p, float damage = 1) {
        if (invincibility > 0) return;
        invincibility = 0.5f;
        health -= damage;
        Debug.Log("oof");
        SendMessage("OnDamage", p, SendMessageOptions.DontRequireReceiver);
        if (transform.parent) {
            Enemy e = transform.parent.GetComponent<Enemy>();
            if (e) e.TakeDamage(p, damage * parentDamageMultiplier);
        }
        if (health <= 0) {
            if (destroyEffect) Instantiate(destroyEffect, transform.position, transform.rotation);
            ZoneInfo.current.SlowMotion(0.2f, Mathf.Clamp(damage, 0, startHealth) * 0.03f);
            Player.score += 1000 * (int)startHealth;
            CameraThing.main.Shake(0.1f * damage, 0.2f);
            if (respawn) {
                GetComponent<Collider>().enabled = false;
                if (GetComponent<Renderer>()) {
                    GetComponent<Renderer>().enabled = false;
                }
                if (transform.childCount > 0) {
                    for (var i = 0; i < transform.childCount; i++) {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                SendMessage("OnDie", p, SendMessageOptions.DontRequireReceiver);
                if (p.boost < p.maxBoost) p.boost += boostDrop;
                StartCoroutine(Respawn());
            } else {
                SendMessage("OnDie", p, SendMessageOptions.DontRequireReceiver);
                if (p.boost < p.maxBoost) p.boost += boostDrop;
                Destroy(gameObject);
            }
        }
    }
}