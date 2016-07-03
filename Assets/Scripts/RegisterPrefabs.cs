using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RegisterPrefabs : MonoBehaviour
{
    public List<GameObject> registerPrefabs;

	public void RegisterPrefabsForNetwork()
    {
        foreach(GameObject prefab in registerPrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }
}
