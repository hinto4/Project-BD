using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    void Update()
    {
        StartCoroutine(StartLevelLoading());        
    }
    // IF MAP IS LOADED; GET NETWORK MANAGER COMPONENT AND THEN LOAD IT ONLINE LEVEL THAT LOADED LEVEL!
    IEnumerator StartLevelLoading()
    {
        yield return new WaitForSeconds(3);

        AsyncOperation async = SceneManager.LoadSceneAsync(2);
        
        if(!async.isDone)
        {
            yield return null;
        }     
    }
}
