using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        GAME.MainMenuOn = true;
    }

    public void StartGame()
    {
        GAME.MainMenuOn = false;
        EventManager.Instance.SendEvent("GameStart");
        gameObject.SetActive(false);
    }


}
