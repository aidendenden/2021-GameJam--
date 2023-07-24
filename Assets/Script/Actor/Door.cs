using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Actor
{
    public bool opening { get { return GAME.keys.Count == 0; } }

    private void Start()
    {
        AutoPos();
    }
    public void Check()
    {
        if (opening)
        {
            EventManager.Instance.SendEvent("Win");
            Debug.Log("WIN");
        }
    }

    private void OnEnable()
    {
        GAME.door = this;
    }
    private void OnDisable()
    {
        GAME.door = null;
    }
}
