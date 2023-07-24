using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHeartMan : Actor
{
    private SimpleAnimator simpleAnimator;

    private Heart heart;
    private GameObject heartPrefab;
    private string heartPath = "prefabs/heart";

    private new void Awake()
    {
        base.Awake();
        isBlock = false;
        moveInterp = 0.06f;
    }

    private void Start()
    {
        simpleAnimator = GetComponent<SimpleAnimator>();
        heartPrefab = Resources.Load<GameObject>(heartPath);
        AutoPos();
    }

    private new void Update()
    {
        base.Update();

        GAME.playerPos = gPos;

        CheckSync();

        if (GAME.canPlayerMove && !moving)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!GAME.heartOut)
                {
                    Rua();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W) && GAME.hsUp) 
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
                simpleAnimator.TurnLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D) && GAME.hsRight)
            {
                MoveTo(new Vector2Int(gPos.x + 1, gPos.y));
                simpleAnimator.TurnRight();
            }
        }
    }

    public void Rua()
    {
        //掏出心脏
        simpleAnimator.PlayAnim(1);
        StartCoroutine(RuaCot());
        //GAME.heartPos = GAME.playerPos;//玩家掏出的心脏在其位置上
    }
    private IEnumerator RuaCot()
    {
        GAME.canPlayerMove = false;
        yield return new WaitForSeconds(0.04f * 14);
        GameObject heartGO = Instantiate(heartPrefab);
        heartGO.transform.position = transform.position;
        heart = heartGO.GetComponent<Heart>();
        heart.gPos = gPos;
        GAME.canPlayerMove = true;

    }
    protected new void MoveTo(Vector2Int targetPos)
    {
        foreach (Monster mons in GAME.monsters)
        {
            if (mons.gPos == targetPos) 
            {
                return;
            }
        }
        base.MoveTo(targetPos);
    }

    private IEnumerator MonsterTurn()
    {
        yield return new WaitForSeconds(0.1f);
        EventManager.Instance.SendEvent("MonsterTurn");//发出玩家移动完成事件
    }
    protected override void StartMove()
    {
        StartCoroutine(MonsterTurn());
        simpleAnimator.PlayAnim(0);
    }
    override protected void AfterMove()
    {
        if (GAME.door != null && GAME.door.gPos == gPos) 
        {
            GAME.door.Check();
            return;
        }
        for (int i = 0; i < GAME.keys.Count; i++)
        {
            if (gPos == GAME.keys[i].gPos) 
            {
                GAME.keys[i].Get();
            }
        }
        EventManager.Instance.SendEvent("PlayerMoved");//发出玩家移动完成事件
    }


    private void CheckSync()
    {
        if (GAME.heartOut && GAME.syncOn)  
        {
            //left
            Vector2Int leftM = new Vector2Int(gPos.x - 1, gPos.y);
            Vector2Int leftH = new Vector2Int(GAME.heartPos.x - 1, GAME.heartPos.y);
            GAME.hsLeft = !GAME.grid.IsBlock(leftH) && !GAME.grid.IsBlock(leftM);
            //right
            Vector2Int rightM = new Vector2Int(gPos.x + 1, gPos.y);
            Vector2Int rightH = new Vector2Int(GAME.heartPos.x + 1, GAME.heartPos.y);
            GAME.hsRight = !GAME.grid.IsBlock(rightH) && !GAME.grid.IsBlock(rightM);
            //left
            Vector2Int upM = new Vector2Int(gPos.x, gPos.y - 1);
            Vector2Int upH = new Vector2Int(GAME.heartPos.x, GAME.heartPos.y - 1);
            GAME.hsUp = !GAME.grid.IsBlock(upH) && !GAME.grid.IsBlock(upM);
            //left
            Vector2Int downM = new Vector2Int(gPos.x, gPos.y + 1);
            Vector2Int downH = new Vector2Int(GAME.heartPos.x, GAME.heartPos.y + 1);
            GAME.hsDown = !GAME.grid.IsBlock(downH) && !GAME.grid.IsBlock(downM);

            Debug.Log("leftH:" + GAME.grid.IsBlock(leftH));
        }
        else
        {
            GAME.hsLeft = GAME.hsRight = GAME.hsUp = GAME.hsDown = true;
        }
        GAME.hsLeft = GAME.hsRight = GAME.hsUp = GAME.hsDown = true;

    }
}
