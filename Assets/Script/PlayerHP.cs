using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP Instance;
    public Slider _HPslider;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _HPslider.maxValue = GAME.HeartHealthMax;
        _HPslider.value = GAME.HeartHealthMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
