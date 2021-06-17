using UnityEngine;

[ExecuteAlways]
public class ShadrThing : MonoBehaviour {
    public Renderer target;
    public Material material;
    [ColorUsage(true, true)] public Color color;
    [ColorUsage(true, true)] public Color mul = Color.white;
    Material m;
    [ContextMenu("Restart")]
    public void Start() {/**/
        if (!material) return;
        m = new Material(material);
        target.material = m;
    }
    void Update() {
        if (!m) return;
        m.SetVector("_Position", (Vector4)transform.position);
        m.SetColor("_Color", color * mul);
    }
}