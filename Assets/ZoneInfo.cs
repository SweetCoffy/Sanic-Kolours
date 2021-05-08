using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool startSuper = false;
    public int startRings = 0;
    public bool startWithMaxBoost = true;
    bool start = true;
    PlayerStuff s;
    void Start() {/*Debug.Log("Start");*/
        if (level) {
            zoneName = level.zoneName;
            act = level.act;
            next = level.next;
        }
        current = this;
        s = Instantiate(spawnOnStart).GetComponent<PlayerStuff>();
    }
    void LateUpdate() {
        if (!start) return;
        PlayerStart();
        start = false;
    }
    public void PlayerStart() {
        if (startWithMaxBoost) s.player.boost = s.player.maxBoost;
        s.player.lastCheckpoint = playerSpawn;
        if (startSuper) s.player.SuperTransform(true);
        s.player.rings = startRings;
        s.player.transform.position = playerSpawn.position;
    }
    public void SlowMotion(float timescale, float duration) {
        StartCoroutine(_SlowMotion(timescale, duration));
    }
    IEnumerator _SlowMotion(float timescale, float duration) {
        /*
        float timeLeft = duration;
        while (timeLeft > 0) {
            timeLeft -= Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(timescale, 1, timeLeft / duration);
            yield return null;
        }
        Time.timeScale = 1;
        */
        yield return null;
    }
    public void End() {
        if (next) next.Start();
        else StageLoader.main.GoToMainMenu();
    }
}