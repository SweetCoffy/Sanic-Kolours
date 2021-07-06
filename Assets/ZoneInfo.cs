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
    HUD h;
    void Start() {/**/
        if (level) {
            zoneName = level.zoneName;
            act = level.act;
            next = level.next;
        }
        s = Instantiate(spawnOnStart).GetComponent<PlayerStuff>();
        h = Instantiate(Game.hudStyle, Vector3.zero, Quaternion.identity, s.canvas.transform).GetComponent<HUD>();
        current = this;
        s.player.hud = h;
        h.player = s.player;
    }
    void LateUpdate() {
        if (!start) return;
        PlayerStart();
        start = false;
    }
    public void PlayerStart() {
        if (startWithMaxBoost) s.player.boost = s.player.MaxBoost;
        s.player.lastCheckpoint = playerSpawn;
        if (startSuper) s.player.SuperTransform(true);
        s.player.rings = startRings;
        h.player = s.player;
        s.player.hud = h;
        s.player.transform.position = playerSpawn.position;
        if (StageLoader.main.tryAgain) s.player.transform.position = StageLoader.main.lastCheckpoint;
        else StageLoader.main.lastCheckpoint = playerSpawn.position;
    }
    public void SlowMotion(float timescale, float duration) {
        StartCoroutine(_SlowMotion(timescale, duration));
    }
    IEnumerator _SlowMotion(float timescale, float duration) {
        float timeLeft = duration;
        while (timeLeft > 0) {
            timeLeft -= Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(timescale, 1, timeLeft / duration);
            yield return null;
        }
        Time.timeScale = 1;
        yield return null;
    }
    public void End() {
        if (Player.main) {
            Game.totalRings += Player.main.rings;
        }
        StageLoader.main.lastCheckpoint = Vector3.zero;
        if (next) next.Start();
        else StageLoader.main.GoToMainMenu();
    }
}