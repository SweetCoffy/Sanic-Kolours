using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject {
    public int buildIndex = 0;
    public string zoneName = "Test Zone";
    public Sprite icon;
    public string fullName = "Test Zone";
    public string act = "";
    public void Start() {
        Debug.Log($"Starting level ${fullName}...");
        SceneManager.LoadScene(buildIndex);
        Debug.Log($"Level ${fullName} started");
    }
}
