using UnityEngine;
public class DialogTrigger : MonoBehaviour {
    [TextArea]
    public string text = "Void";
    public bool hide = false;
    public bool oneTimeUse = false;
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            if (hide) TextBoxHUD.main.Hide();
            else TextBoxHUD.main.Show(text);
            if (oneTimeUse) gameObject.SetActive(false);
        }
    }
}