using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Actor
{
    private void Start()
    {
        AutoPos();
    }
    public void Get()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GAME.keys.Add(this);
    }
    private void OnDisable()
    {
        GAME.keys.Remove(this);
    }
}
