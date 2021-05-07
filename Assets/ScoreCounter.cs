using UnityEngine;
using UnityEngine.UI;
public class ScoreCounter : Text {
    void Update() {
        text = Player.score + "";
    }
}