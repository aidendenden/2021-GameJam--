using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 储存需要共享的所有数据
/// </summary>
public static class GAME
{
    public static int ManHealthMax = 1;
    public static int HeartHealthMax = 7;

    public static int ManHealthCrt;
    public static int HeartHealthCrt;

    public static Grid grid;//当前的网格
    public static Door door;//current door

    //turn base 
    public static bool canPlayerMove = true;

    public static Vector2Int playerPos = new Vector2Int();//玩家的位置

    public static bool heartOut = false;//是否已掏出心脏
    public static Vector2Int heartPos = new Vector2Int();//心脏位置
    public static Heart heart;//心脏的引用

    public static List<Monster> monsters = new List<Monster>();//所有的怪物
    public static List<Key> keys = new List<Key>();//所有的钥匙

    //heart sync
    public static bool syncOn;
    public static bool hsLeft;      //♥或主角左边被阻挡时此值为False,两者左边都无阻挡时才为true
    public static bool hsRight;
    public static bool hsUp;
    public static bool hsDown;


    public static void InitGame()
    {
        ManHealthCrt = ManHealthMax;
        HeartHealthCrt = HeartHealthMax;
        canPlayerMove = true;
        syncOn = false;
    }

    public static void EatMan()
    {
        EventManager.Instance.SendEvent("Die");
        Debug.Log("YouDied");
    }
    public static void EatHeart()
    {
        if (HeartHealthCrt - 1 > 0) 
        {
            HeartHealthCrt--;
        }
        else
        {
            EventManager.Instance.SendEvent("Die");
            Debug.Log("YouDied");
        }
    }

}
