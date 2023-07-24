using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏中大多数元素的基类，继承这个基类使其拥有在网格中移动的功能
/// </summary>
public class Actor : MonoBehaviour
{
    public float moveInterp = 0.05f;//移动一格所用的时间
    public bool isBlock = false;

    public Vector2Int gPos;

    public bool moving;

    private SpriteRenderer spriteRenderer;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        if (!moving)
        {
            transform.position = GAME.grid.GetWPos(gPos);
        }
        if (spriteRenderer != null) 
        {
            spriteRenderer.sortingOrder = gPos.y;
        }
        //CheckPath();
    }

    protected void AutoPos()
    {
        SetPosTo(GAME.grid.GetGPos(transform.position));
    }

    protected void SetPosTo(Vector2Int targetPos)
    {
        if (isBlock)
        {
            GAME.grid.SetWalk(gPos);
        }
        gPos = targetPos;
        if (isBlock)
        {
            GAME.grid.SetBlock(gPos);
        }
    }
    protected void MoveTo(Vector2Int targetPos)
    {
        if (isBlock)
        {
            GAME.grid.SetWalk(gPos);
        }
        if (moving || !GAME.grid.IsGPosValid(targetPos) || GAME.grid.IsBlock(targetPos)) 
            return;

        StartCoroutine(MoveCoroutine(targetPos));
    }
    private IEnumerator MoveCoroutine(Vector2Int targetPos)
    {
        moving = true;
        gPos = targetPos;
        StartMove();
        if (isBlock)
        {
            GAME.grid.SetBlock(gPos);
        }
        Vector3 wTargetPos = GAME.grid.GetWPos(targetPos);
        WaitForEndOfFrame waiter = new WaitForEndOfFrame();
        while (Vector3.Distance(transform.position, wTargetPos) > 0.04f)   
        {
            transform.position = Vector3.Lerp(transform.position, wTargetPos, moveInterp);
            yield return waiter;
        }
        //transform.position = wTargetPos;
        moving = false;
        AfterMove();
    }
    protected virtual void StartMove() { }
    protected virtual void AfterMove() { }
    
}
