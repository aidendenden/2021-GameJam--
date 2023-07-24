using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Actor
{
    public Vector2Int target;
    public SimpleAnimator simpleAnimator;

    private new void Awake()
    {
        base.Awake();
        isBlock = true;
    }

    private void Start()
    {
        simpleAnimator = GetComponent<SimpleAnimator>();
        EventManager.Instance.SubscribeEvent("MonsterTurn", OnMonsterTurn);//订阅PlayerMoved事件
        AutoPos();
    }

    private void OnEnable()
    {
        GAME.monsters.Add(this);
    }

    private void OnDisable()
    {
        GAME.monsters.Remove(this);
        EventManager.Instance.UnSubscribeEvent("MonsterTurn", OnMonsterTurn);
    }

    //对于"MonsterTurn"事件进行响应
    public void OnMonsterTurn()
    {
        if (!GAME.heartOut)
        {
            target = GAME.playerPos;//设置寻路的目标点，在玩家掏出心脏后应该设置为心脏的位置
        }
        else
        {
            target = GAME.heartPos;
        }

        List<AStarNode> nodes = CheckPath();

        if (nodes != null) 
        {
            if (nodes.Count > 1)
            {
                GAME.canPlayerMove = false;
                if (nodes[0].pos.x > gPos.x) 
                {
                    simpleAnimator.TurnRight();
                }
                else if (nodes[0].pos.x < gPos.x)
                {
                    simpleAnimator.TurnLeft();
                }
                MoveTo(nodes[0].pos);//返回的数组中第一个元素即为移动的目标点
                simpleAnimator.PlayAnim(0);
            }
            else if (nodes.Count == 1) //若这个数组中的元素数为一，也就是下一步便与目标位置重合，那么就表示当前位置和目标点相邻，可以吃一口
            {
                Eat();
            }
        }
        else
        {
            Debug.Log(name+":failedToFindPath");
        }
    }

    override protected void AfterMove()
    {
        GAME.canPlayerMove = true;
    }

    //搜索路径，返回的是从下一个位置开始直到目标点的数组
    private List<AStarNode> CheckPath()
    {
        if (GAME.grid.IsGPosValid(gPos) && GAME.grid.IsGPosValid(target))
        {
            List<AStarNode> nodes = GAME.grid.FindPath(gPos, target);
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    Debug.DrawLine(GAME.grid.GetWPos(nodes[i].pos), GAME.grid.GetWPos(nodes[i + 1].pos), Color.yellow, 0.7f);
                }
            }
            return nodes;
        }
        else
        {
            return null;
        }
    }

    //吃，玩家扣血或者心脏扣血
    private void Eat()
    {
        simpleAnimator.PlayAnim(1);
        StartCoroutine(EatCot());
    }
    private IEnumerator EatCot()
    {
        GAME.canPlayerMove = false;
        yield return new WaitForSeconds(0.04f * 9);
        if (GAME.heartOut)
        {
            //扣♥的血
            //Debug.Log("HeartHurt");
            GAME.EatHeart();
        }
        else
        {
            //扣玩家的血
            //Debug.Log("ManHurt");
            GAME.EatMan();
        }
        GAME.canPlayerMove = true;
    }

}
