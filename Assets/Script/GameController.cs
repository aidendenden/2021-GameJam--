using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        GAME.InitGame();

        EventManager.Instance.SubscribeEvent("Die", Restart);
        EventManager.Instance.SubscribeEvent("Win", NextLevel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Restart();
        }
    }

    public void Restart()
    {
        if (GAME.heart != null)
        {
            Destroy(GAME.heart.gameObject);
        }
        GAME.InitGame();
        LoadScripts.Instance.ReStart();
    }

    public void NextLevel()
    {
        if (GAME.heart != null)
        {
            Destroy(GAME.heart.gameObject);
        }
        if (!LoadScripts.Instance.LoadNextLevel())
        {
            Application.Quit();
        }
    }
}
