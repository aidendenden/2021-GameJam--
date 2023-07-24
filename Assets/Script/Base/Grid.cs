using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    [SerializeField]
    private bool draw;

    [Header("GridData")]
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private float c;

    [HideInInspector]
    public int xSize;
    [HideInInspector]
    public int ySize;
    [HideInInspector]
    public float cellSize;

    private float halfCellSize { get { return 0.5f * cellSize; } }
    private Vector3 logicalOriginWPos = new Vector3();

    public AStarPathFinder pathFinder;
    private List<Vector2Int> blocks = new List<Vector2Int>();

    private void Start()
    {
        //SetUpGrid();
    }

    private void OnEnable()
    {

        GAME.grid = this;
        SetUpGrid();
    }   

    private void Update()
    {
        if (transform.position != Vector3.zero)
        {
            transform.position = Vector3.zero;
        }
        if (draw)
        {
            DrawGrid();
        }
    }

    [ContextMenu("SetUp")]
    private void SetUpGrid()
    {
        x = x == 0 ? 1 : x;
        y = y == 0 ? 1 : y;
        c = c == 0 ? 0.5f : c;

        SetUpGrid(x, y, c);
    }

    private void SetUpGrid(int _xSize, int _ySize, float _cellSize = 0.5f)
    {
        xSize = _xSize;
        ySize = _ySize;
        cellSize = _cellSize;

        logicalOriginWPos = new Vector3(-xSize * cellSize * 0.5f + halfCellSize, ySize * cellSize * 0.5f - halfCellSize, 0);

        pathFinder = new AStarPathFinder();
        pathFinder.Init(new Vector2Int(xSize, ySize));

        Debug.Log(name+"Initialized");
    }

    public bool IsGPosValid(Vector2Int gPos)
    {
        return !(gPos.x > xSize - 1 || gPos.x < 0 || gPos.y > ySize - 1 || gPos.y < 0);
    }

    public bool IsBlock(Vector2Int gPos)
    {
        if (blocks == null)
            return false;

        return blocks.Contains(gPos);
    }
    public Vector3 GetWPos(Vector2Int gPos)
    {
        gPos.x = Mathf.Clamp(gPos.x, 0, xSize - 1);
        gPos.y = Mathf.Clamp(gPos.y, 0, ySize - 1);
        return new Vector3(logicalOriginWPos.x + gPos.x * cellSize, logicalOriginWPos.y - gPos.y * cellSize, 0);
    }
    public Vector2Int GetGPos(Vector3 wPos)
    {
        float distanceX = wPos.x - logicalOriginWPos.x + halfCellSize;
        float distanceY = logicalOriginWPos.y - wPos.y + halfCellSize;

        int gridOffsetX = (int)Mathf.Floor(distanceX / cellSize);
        int gridOffsetY = (int)Mathf.Floor(distanceY / cellSize);

        return new Vector2Int(Mathf.Clamp(gridOffsetX, 0, gridOffsetX), Mathf.Clamp(gridOffsetY, 0, gridOffsetY));
    }
    public void SetBlock(Vector2Int pos)
    {
        if (IsGPosValid(pos))
        {
            pathFinder.SetBlock(pos);
            blocks.Add(pos);
        }
    }

    public void SetWalk(Vector2Int pos)
    {
        if (IsGPosValid(pos))
        {
            pathFinder.SetWalk(pos);
            blocks.Remove(pos);
        }
    }

    public List<AStarNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        return pathFinder.FindPath(startPos, endPos);
    }

    private void DrawGrid()
    {
        Color col = Color.blue;

        //画竖线
        float up = cellSize * ySize * 0.5f;
        float down = -cellSize * ySize * 0.5f;

        for (int i = 0; i <= xSize; i++) 
        {
            Debug.DrawLine(new Vector3((i - xSize * 0.5f)*cellSize, up, 0), new Vector3((i - xSize * 0.5f)*cellSize, down, 0), col);
        }
        //画横线
        float left = -cellSize * xSize * 0.5f;
        float right= cellSize * xSize * 0.5f;

        for (int i = 0; i <= ySize; i++)
        {
            Debug.DrawLine(new Vector3(left, (i - ySize * 0.5f) * cellSize, 0), new Vector3(right, (i - ySize * 0.5f) * cellSize, 0), col);
        }

        //标记障碍物
        for (int i = 0; i < blocks.Count; i++) 
        {
            Vector3 center = GetWPos(blocks[i]);
            Debug.DrawLine(new Vector3(center.x - halfCellSize, center.y + halfCellSize, 0), new Vector3(center.x + halfCellSize, center.y - halfCellSize, 0), Color.red);
            Debug.DrawLine(new Vector3(center.x + halfCellSize, center.y + halfCellSize, 0), new Vector3(center.x - halfCellSize, center.y - halfCellSize, 0), Color.red);
        }
    }

    private void OnValidate()
    {
        SetUpGrid();
    }


}
