using UnityEngine;
public class ZoneInfo : MonoBehaviour {
    public string zoneName = "Test Zone";
    public string act = "";
    public GameObject spawnOnStart;
    public static ZoneInfo current;
    public Transform playerSpawn;
    void Start() {
        current = this;
        PlayerStuff s = Instantiate(spawnOnStart).GetComponent<PlayerStuff>();
        s.player.lastCheckpoint = playerSpawn;
        s.player.transform.position = playerSpawn.position;
        s.titleCard.nameText.text = zoneName;
        s.titleCard.actText.text = act;
    }
}