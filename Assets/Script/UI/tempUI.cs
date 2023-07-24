using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempUI : MonoBehaviour
{
    Text tt;

    private void Start()
    {
        tt = GetComponent<Text>();
    }
    private void Update()
    {
        tt.text = "♥：" + GAME.HeartHealthCrt + "/" + GAME.HeartHealthMax;
    }
}
