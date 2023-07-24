using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LoadScripts : MonoBehaviour
{
    public int LevelNum=1;
    public GameObject LevelPrefab;

    private static LoadScripts _instance;
    public static LoadScripts Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelNum = int.Parse(LevelPrefab.name.Substring(LevelPrefab.name.Length - 1, 1));
    }

    /*//test
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
            Debug.Log("Next");

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReStart();
        }
    }
    */

    public bool LoadNextLevel()
    {
        if (LevelPrefab != null)
        {
            Destroy(LevelPrefab);
        }

        LevelNum++;
        GameObject prefab = (GameObject)Resources.Load($"prefabs/Level/Level_{LevelNum}");
        if (prefab == null)
        {
            //game complete
            return false;
        }
        else
        {
            LevelPrefab = Instantiate(prefab);
            return true;
        }

    }

    public void ReStart()
    {
        if (LevelPrefab != null)
        {
            Destroy(LevelPrefab);
        }

        GameObject prefab = (GameObject)Resources.Load($"prefabs/Level/Level_{LevelNum}");
        LevelPrefab = Instantiate(prefab);

    }
}
