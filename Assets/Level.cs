using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject {
    public int buildIndex = 0;
    public int stageNumber = 0;
    public string zoneName = "Test Zone";
    public Sprite icon;
    public string fullName = "Test Zone";
    public string act = "";
    public string[] unlockOnEnd;
    public Level next;
    [ContextMenu("Force Load")]
    public void Start() {/**/
        StageLoader.main.LoadStage(this);
    }
}
