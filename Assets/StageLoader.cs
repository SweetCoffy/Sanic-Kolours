using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
public class StageLoader : MonoBehaviour {
    public GameObject loadingScreen;
    public GameObject tryAgainScreen;
    public GameObject bossHUD;
    public Transform bossesHolder;
    public List<BossHUD> bosses;
    public Canvas canvas;
    public Level curStage;
    public GameObject defaultHudStyle;
    public Level mainMenu;
    public static StageLoader main;
    bool loadingStage = false;
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.Period)) {
            GoToMainMenu();
        }
        Game.Update();
    }
    void Start() {/**/
        if (Game.hudStyle == null) {
            Game.hudStyle = defaultHudStyle;
        }
        if (main) {
            Destroy(gameObject);
            return;
        }
        bosses = new List<BossHUD>();
        DontDestroyOnLoad(canvas.gameObject);
        main = this;
        DontDestroyOnLoad(gameObject);
        LoadStage(mainMenu);
        Game.Start();
    }
    public void AddBoss(Enemy b) {
        BossHUD h = Instantiate(bossHUD, Vector3.zero, Quaternion.identity, bossesHolder).GetComponent<BossHUD>();
        h.target = b;
        bosses.Add(h);
    }
    public void RemoveBoss(Enemy b) {
        foreach (BossHUD h in bosses) {
            if (h == null) continue;
            if (h.target == b) {
                h.DeleteFromExistence();
            }
        }
        bosses = bosses.FindAll(b => b != null);
    }
    public Coroutine LoadStage(Level l, bool tryAgain = false) {
        /**/
        if (loadingStage) return null;
        if (l == null) l = mainMenu;
        curStage = l;
        loadingStage = true;
        return StartCoroutine(_Loading(l, tryAgain));
    }
    public Coroutine EndStage() {
        if (Player.main) {
            Game.totalRings += Player.main.rings;
        }
        if (curStage.unlockOnEnd != null) {
            for (int i = 0; i < curStage.unlockOnEnd.Length; i++) {
                Game.UnlockByID(curStage.unlockOnEnd[i]);
            }
        }
        return LoadStage(curStage.next);
    }
    public Coroutine RestartStage() {
        return LoadStage(curStage, true);
    }
    public Coroutine GoToMainMenu() {
        return LoadStage(mainMenu);
    }
    IEnumerator _Loading(Level l, bool tryAgain) {
        /**/
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        LoadingScreen ls = null;
        Boss.bossCount = 0;
        TryAgainScreen ta = null;
        if (!tryAgain) {            
            ls = Instantiate(loadingScreen).GetComponent<LoadingScreen>();
            DontDestroyOnLoad(ls.gameObject);
            ls.zName = l.zoneName;
            ls.zAct = l.act;
            ls.sNumber = l.stageNumber;
            ls.anim.SetTrigger("Start");
        } else {
            ta = Instantiate(tryAgainScreen).GetComponent<TryAgainScreen>();
            DontDestroyOnLoad(ta.gameObject);
            ta.anim.SetTrigger("Start");
        }
        /**/
        int c = bosses.Count;
        for (int i = c - 1; i >= 0; i--) {
            try {
                bosses[i].DeleteFromExistence();
            } catch (System.Exception) {
                try {
                    Destroy(bosses[i].gameObject);
                } catch (System.Exception) {}
            }
            yield return new WaitForEndOfFrame();
        }
        bosses.RemoveAll(h => true);
        /**/
        yield return new WaitForSeconds(2.2f);
        AsyncOperation ao = SceneManager.LoadSceneAsync(l.buildIndex);
        ao.priority = -1;
        while (!ao.isDone) {
            if (!tryAgain) ls.progress = ao.progress / .9f;
            yield return new WaitForEndOfFrame();
        } 
        if (!tryAgain) ls.anim.SetTrigger("End");
        else ta.anim.SetTrigger("End");
        loadingStage = false;
        yield return new WaitForSeconds(1.6f);
        if (!tryAgain) Destroy(ls.gameObject);
        else Destroy(ta.gameObject);
        yield return null;
    }
}