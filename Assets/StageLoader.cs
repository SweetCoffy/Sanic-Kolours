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
    public Level mainMenu;
    public static StageLoader main;
    bool loadingStage = false;
    void Start() {/*Debug.Log("Start");*/
        if (main) {
            Destroy(gameObject);
            return;
        }
        bosses = new List<BossHUD>();
        DontDestroyOnLoad(canvas.gameObject);
        main = this;
        DontDestroyOnLoad(gameObject);
        LoadStage(mainMenu);
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
        /*Debug.Log("Loading but 1");*/
        if (loadingStage) return null;
        curStage = l;
        loadingStage = true;
        return StartCoroutine(_Loading(l, tryAgain));
    }
    public Coroutine EndStage() {
        return LoadStage(curStage.next);
    }
    public Coroutine RestartStage() {
        return LoadStage(curStage, true);
    }
    public Coroutine GoToMainMenu() {
        return LoadStage(mainMenu);
    }
    IEnumerator _Loading(Level l, bool tryAgain) {
        /*Debug.Log("Loading");*/
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
        /*Debug.Log("Loading but 2");*/
        int c = bosses.Count;
        for (int i = c - 1; i > 0; i--) {
            bosses[i].DeleteFromExistence();
            yield return new WaitForEndOfFrame();
        }
        bosses.RemoveAll(h => true);
        /*Debug.Log("Loading but 3");*/
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