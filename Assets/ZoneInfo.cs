using UnityEngine;
using System.Collections;
public class ZoneInfo : MonoBehaviour {
    public string zoneName = "Test Zone";
    public string act = "";
    public GameObject spawnOnStart;
    public static ZoneInfo current;
    public Transform playerSpawn;
    public Level level;
    public Level next;
    public float gravityModifier = 1;
    void Start() {
        if (level) {
            zoneName = level.zoneName;
            act = level.act;
            next = level.next;
        }
        current = this;
        PlayerStuff s = Instantiate(spawnOnStart).GetComponent<PlayerStuff>();
        s.player.lastCheckpoint = playerSpawn;
        s.player.transform.position = playerSpawn.position;
        s.titleCard.nameText.text = zoneName;
        s.titleCard.actText.text = act;
    }
    public void Loading(AsyncOperation ao, Level l) {
        StartCoroutine(_Loading(ao, l));
    }
    IEnumerator _Loading(AsyncOperation ao, Level l) {
        while (!ao.isDone) {
            Debug.Log($"Loading level {l.fullName}: {Mathf.Floor(ao.progress * 100)}%");
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log($"Level {l.fullName} started");
    }
    public void End() {
        if (next) next.Start();
    }
}