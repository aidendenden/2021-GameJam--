using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AStarNodeType
{
    WALK,
    STOP,
}

public class AStarNode
{
    public float f;
    public float g;
    public float h;

    public Vector2Int pos;

    public AStarNode parent;
    public AStarNodeType type;

    public AStarNode(Vector2Int pos, AStarNodeType type)
    {
        this.pos = pos;
        this.type = type;
    }

    public void SetType(AStarNodeType type)
    {
        this.type = type;
    }

    public void Reset()
    {
        f = g = h = 0;
        parent = null;
    }

}
