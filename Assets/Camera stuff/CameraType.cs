using UnityEngine;
public class CameraType : MonoBehaviour {
    [HideInInspector] public Transform reference;
    [HideInInspector] public CameraTrigger trigger;
    [HideInInspector] public CameraThing cam;
    [HideInInspector] public Player player;
    public float deltaTime;
    public virtual void CameraUpdate() {}
    public virtual void CameraStart() {}
    public virtual void CameraEnd() {}
    public virtual void CameraPostEnd() {}
}