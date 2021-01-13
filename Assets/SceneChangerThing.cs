using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangerThing : MonoBehaviour
{
    public void ChangeScene(int buildIndex) {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex);
    }
}
