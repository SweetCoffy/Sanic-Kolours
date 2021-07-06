using UnityEngine;
public class Checkpoint : MonoBehaviour {
    public int id = 0;
    public float animTargetRotationTime = 2;
    public float animTargetPositionTime = 2;
    float t = 0;
    bool doAnim = false;
    public Transform animTarget;
    public Transform anim;
    public Transform obj;
    Vector3 startPos;
    Quaternion startRot;
    void Start() {
        startPos = anim.localPosition;
        startRot = anim.localRotation;
    }
    void Update() {
        if (doAnim) {
            if (t < 1) t += Time.deltaTime;
            if (t > 1) t = 1;
            anim.localPosition = Vector3.LerpUnclamped(startPos, animTarget.localPosition, t * animTargetPositionTime);
            anim.localRotation = Quaternion.SlerpUnclamped(startRot, animTarget.localRotation, t * animTargetPositionTime);

        }
    }
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            if (StageLoader.main.checkpointId < id) {
                StageLoader.main.checkpointId = id;
                StageLoader.main.lastCheckpoint = obj.position;
                doAnim = true;
            }
        }
    }
}