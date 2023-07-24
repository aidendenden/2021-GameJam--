/*
    在开始关卡时，调用 Init(Vector2Int size) 进行初始化，参数是地图的大小；
    然后用 SetBlock()和SetWalk()来设置障碍物和可行走的格子（初始化时所有格子都是可行走的，所以一般只要用SetBlock()就行了），
        参数可以是Vector2Int或者Vector2Int数组；
    
    设置完成后，用 Find Path（）即可进行寻路，会返回一个 List<AStarNode> ，从前往后遍历就是要走的路线中的每一个节点，
        第0个节点是当前位置的下一节点

    AStarNode.pos是在网格内的坐标，到世界坐标的转换需要自己完成。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder
{
    private static AStarPathFinder instance;

    public Vector2Int size;

    public AStarNode[,] nodes;

    public List<AStarNode> openList = new List<AStarNode>();
    public List<AStarNode> closeList = new List<AStarNode>();
    List<AStarNode> wayList = new List<AStarNode>();
    public AStarNode curNode;

    static public AStarPathFinder GetInstance()
    {
        if (instance == null) 
        {
            instance = new AStarPathFinder();
        }
        return instance;
    }

    public void Init(Vector2Int size)
    {
        this.size = size;
        nodes = new AStarNode[size.x, size.y];
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                nodes[i,j] = new AStarNode(new Vector2Int(i,j),AStarNodeType.WALK);
            }
        }
    }

    public void SetBlock(List<Vector2Int> blocks)
    {
        foreach (Vector2Int b in blocks)
        {
            nodes[b.x, b.y].SetType(AStarNodeType.STOP);
        }
    }

    public void SetBlock(Vector2Int blocks)
    {
        nodes[blocks.x, blocks.y].SetType(AStarNodeType.STOP);
    }
    public void SetWalk(List<Vector2Int> blocks)
    {
        foreach (Vector2Int b in blocks)
        {
            nodes[b.x, b.y].SetType(AStarNodeType.WALK);
        }
    }

    public void SetWalk(Vector2Int blocks)
    {
        nodes[blocks.x, blocks.y].SetType(AStarNodeType.WALK);
    }
    public List<AStarNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        if (GetNode(startPos)!=null&& GetNode(endPos)!=null)
        {
            openList.Clear();
            closeList.Clear();
            if (GetNode(startPos).type == AStarNodeType.STOP || GetNode(endPos).type == AStarNodeType.STOP)
            {
                Debug.Log("起始点或目标点有障碍物");
                return null;
            }
            foreach (AStarNode node in nodes)
            {
                node.Reset();
            }
            AStarNode startNode = GetNode(startPos);

            AStarNode curNode = startNode;
            AStarNode bestNode = curNode;

            while (curNode.pos != endPos)
            {
                //上
                if (GetUpNode(curNode) != null && GetUpNode(curNode).type == AStarNodeType.WALK && !closeList.Contains(GetUpNode(curNode)) && !openList.Contains(GetUpNode(curNode)))
                {
                    CalculateNodeValue(GetUpNode(curNode), startPos, endPos, curNode);
                    openList.Add(GetUpNode(curNode));
                }
                //下
                if (GetDownNode(curNode) != null && GetDownNode(curNode).type == AStarNodeType.WALK && !closeList.Contains(GetDownNode(curNode)) && !openList.Contains(GetDownNode(curNode)))
                {
                    CalculateNodeValue(GetDownNode(curNode), startPos, endPos, curNode);
                    openList.Add(GetDownNode(curNode));
                }
                //左
                if (GetLeftNode(curNode) != null && GetLeftNode(curNode).type == AStarNodeType.WALK && !closeList.Contains(GetLeftNode(curNode)) && !openList.Contains(GetLeftNode(curNode)))
                {
                    CalculateNodeValue(GetLeftNode(curNode), startPos, endPos, curNode);
                    openList.Add(GetLeftNode(curNode));
                }
                //右
                if (GetRightNode(curNode) != null && GetRightNode(curNode).type == AStarNodeType.WALK && !closeList.Contains(GetRightNode(curNode)) && !openList.Contains(GetRightNode(curNode)))
                {
                    CalculateNodeValue(GetRightNode(curNode), startPos, endPos, curNode);
                    openList.Add(GetRightNode(curNode));
                }
                //把下面这部分代码的注释删掉就可以斜着寻路了
                /*
                //7
                if (GetLeftNode(GetUpNode(curNode)) != null && GetLeftNode(GetUpNode(curNode)).type == AStarNodeType.WALK
                    && !closeList.Contains(GetLeftNode(GetUpNode(curNode))) && !openList.Contains(GetLeftNode(GetUpNode(curNode))))
                {
                    CalculateNodeValue(GetLeftNode(GetUpNode(curNode)), startPos, endPos, curNode);
                    openList.Add(GetLeftNode(GetUpNode(curNode)));
                }
                //9
                if (GetRightNode(GetUpNode(curNode)) != null && GetRightNode(GetUpNode(curNode)).type == AStarNodeType.WALK
                    && !closeList.Contains(GetRightNode(GetUpNode(curNode))) && !openList.Contains(GetRightNode(GetUpNode(curNode))))
                {
                    CalculateNodeValue(GetRightNode(GetUpNode(curNode)), startPos, endPos, curNode);
                    openList.Add(GetRightNode(GetUpNode(curNode)));
                }
                //1
                if (GetLeftNode(GetDownNode(curNode)) != null && GetLeftNode(GetDownNode(curNode)).type == AStarNodeType.WALK
                    && !closeList.Contains(GetLeftNode(GetDownNode(curNode))) && !openList.Contains(GetLeftNode(GetDownNode(curNode))))
                {
                    CalculateNodeValue(GetLeftNode(GetDownNode(curNode)), startPos, endPos, curNode);
                    openList.Add(GetLeftNode(GetDownNode(curNode)));
                }
                //3
                if (GetRightNode(GetDownNode(curNode)) != null && GetRightNode(GetDownNode(curNode)).type == AStarNodeType.WALK
                    && !closeList.Contains(GetRightNode(GetDownNode(curNode))) && !openList.Contains(GetRightNode(GetDownNode(curNode))))
                {
                    CalculateNodeValue(GetRightNode(GetDownNode(curNode)), startPos, endPos, curNode);
                    openList.Add(GetRightNode(GetDownNode(curNode)));
                }
                */

                SortOpenList();
                closeList.Add(openList[0]);
                curNode = openList[0];
                openList.Remove(openList[0]);

                if (openList.Count == 0)
                {
                    return null;
                }
            }
            List<AStarNode> output = new List<AStarNode>();
            while (curNode.pos != startPos)
            {
                output.Add(curNode);
                curNode = curNode.parent;
            }
            //output.Add(startNode);
            output.Reverse();

            return output;
        }
        else
        {
            Debug.Log("无效的坐标");
            return null;
        }
    }

    public AStarNode GetNode(Vector2Int pos)
    {
        if (pos.x<nodes.GetLength(0)&& pos.y < nodes.GetLength(1))
        {
            return nodes[pos.x, pos.y];
        }
        else
        {
            return null;
        }
    }
    public AStarNode GetUpNode(AStarNode node)
    {
        if (node==null)
        {
            return null;
        }
        if (node.pos.y > 0) 
        {
            return nodes[node.pos.x, node.pos.y - 1];
        }
        else
        {
            return null;
        }

    }
    public AStarNode GetDownNode(AStarNode node)
    {
        if (node == null)
        {
            return null;
        }
        if (node.pos.y < nodes.GetLength(1) - 1) 
        {
            return nodes[node.pos.x, node.pos.y + 1];
        }
        else
        {
            return null;
        }
    }
    public AStarNode GetLeftNode(AStarNode node)
    {
        if (node == null)
        {
            return null;
        }
        if (node.pos.x > 0)
        {
            return nodes[node.pos.x - 1, node.pos.y];
        }
        else
        {
            return null;
        }
    }
    public AStarNode GetRightNode(AStarNode node)
    {
        if (node == null)
        {
            return null;
        }
        if (node.pos.x < nodes.GetLength(0) - 1)
        {
            return nodes[node.pos.x + 1, node.pos.y];
        }
        else
        {
            return null;
        }
    }

    public float DistanceValue(Vector2Int APos, Vector2Int BPos)
    {
        //return Mathf.Abs(BPos.x - APos.x) + Mathf.Abs(BPos.y - APos.y);
        return Vector2.Distance(APos, BPos);
    }
    public float CalculateNodeValue(AStarNode node, Vector2Int startPos, Vector2Int endPos,AStarNode parent)
    {
        node.g = DistanceValue(node.pos, startPos);
        node.h = DistanceValue(node.pos, endPos);
        node.f = node.g + node.h;
        node.parent = parent;
        return node.f;
    }

    public void SortOpenList()
    {
        AStarNode temp;
        for (int count = openList.Count-1; count>0;count--)
        {
            for (int i = 0; i < count; i++)
            {
                if (openList[i].f > openList[i + 1].f)
                {
                    temp = openList[i];
                    openList[i] = openList[i + 1];
                    openList[i + 1] = temp;
                }
            }
        }
    }

}
