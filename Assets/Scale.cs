using UnityEngine;
public class Scale : MonoBehaviour {
    public float x = 0;
    public float y = 0;
    public float z = 0;
    void FixedUpdate() {
        transform.localScale += new Vector3(x, y, z) * Time.deltaTime;
    }
}