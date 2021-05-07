using UnityEngine;

public class ShadrThing : MonoBehaviour {
    public Renderer target;
    public Material material;
    [ColorUsage(true, true)] public Color color;
    Material m;
    [ContextMenu("Restart")]
    public void Start() {/*Debug.Log("Start");*/
        if (!material) return;
        m = new Material(material);
        target.material = m;
    }
    void Update() {
        if (!m) return;
        m.SetVector("_Position", (Vector4)transform.position);
        m.SetColor("_Color", color);
    }
}