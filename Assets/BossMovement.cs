using UnityEngine;
public class BossMovement : MonoBehaviour {
    public float speed = 15;
    public Rigidbody rb;
    public Boss boss;
    protected virtual void Start() {/**/
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!boss) boss = GetComponent<Boss>();
        if (boss) boss.movementComponents.Add(this);
    }
}