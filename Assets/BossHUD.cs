using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BossHUD : MonoBehaviour {
    public Enemy target;
    public Text bossName;
    public Animator anim;
    public Image healthBar;
    void Start() {/*Debug.Log("Start");*/
        bossName.text = target.gameObject.name;
    }
    void Update() {
        if (target != null) {
            healthBar.fillAmount = target.health / target.startHealth;
        } else healthBar.fillAmount = 0;
    }
    public void DeleteFromExistence() {
        StartCoroutine(_Delet());
    }
    IEnumerator _Delet() {
        if (anim) {
            anim.SetTrigger("End");
            yield return new WaitForSeconds(1);
        }
        Destroy(gameObject);
    }
}