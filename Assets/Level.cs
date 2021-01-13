using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject {
    public int buildIndex = 0;
    public string zoneName = "Test Zone";
    public Sprite icon;
    public string fullName = "Test Zone";
    public string act = "";
    public Level next;
    public void Start() {
        Debug.Log($"Starting level {fullName}...");
        AsyncOperation ao = SceneManager.LoadSceneAsync(buildIndex);
        if (ZoneInfo.current) ZoneInfo.current.Loading(ao, this);
    }
}
