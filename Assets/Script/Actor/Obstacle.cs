using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Actor
{
    private new void Awake()
    {
        base.Awake();
        isBlock = true;
    }
    private void Start()
    {
        AutoPos();
    }
}
