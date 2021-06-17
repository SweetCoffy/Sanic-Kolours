[UnityEngine.ExecuteAlways]
public class SortOrderThing : UnityEngine.MonoBehaviour {
    public string layer;
    public int order;
    public UnityEngine.Renderer img;
    void Update() {
        img.sortingLayerName = layer;
        img.sortingOrder = order;
    }
}