using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private AStarPathFinder pathFinder = new AStarPathFinder();

    // Start is called before the first frame update
    void Start()
    {
        pathFinder.Init(new Vector2Int(6, 3));
        List<AStarNode> nodes = pathFinder.FindPath(new Vector2Int(0, 0), new Vector2Int(5, 2));
        if (nodes == null) 
        {
            Debug.Log("NULL");
        }
        else
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Debug.Log(nodes[i].pos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
