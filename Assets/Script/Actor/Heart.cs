using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Actor
{
    private new void Update()
    {
        base.Update();
        GAME.heartPos = gPos;

        if (!GAME.syncOn && Input.GetKeyDown(KeyCode.Space)) 
        {
            GAME.syncOn = true;
            Debug.Log("SyncOn");
        }
        if (GAME.syncOn& GAME.canPlayerMove)
        {
            if (Input.GetKeyDown(KeyCode.W) && GAME.hsUp)
            {
                MoveTo(new Vector2Int(gPos.x, gPos.y - 1));
            }
            else if (Input.GetKeyDown(KeyCode.S) && GAME.hsDown)
            {
                MoveTo(new Vector2Int(gPos.x, gPos.y + 1));
            }
            else if (Input.GetKeyDown(KeyCode.A) && GAME.hsLeft)
            {
                MoveTo(new Vector2Int(gPos.x - 1, gPos.y));
            }
            else if (Input.GetKeyDown(KeyCode.D) && GAME.hsRight) 
            {
                MoveTo(new Vector2Int(gPos.x + 1, gPos.y));
            }
        }
        if (GAME.HeartHealthCrt <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Delete()
    {
        //Destroy(gameObject);
    }
    private void OnEnable()
    {
        EventManager.Instance.SubscribeEvent("Die", Delete);

        if (GAME.heart != null) 
        {
            Destroy(GAME.heart.gameObject);
        }
        Debug.Log("Enable");
        GAME.heartOut = true;
        GAME.heart = this;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnSubscribeEvent("Die",Delete);
        GAME.heartOut = false;
        if (GAME.heart != null)
        {
            GAME.heart = null;
        }
    }

}
