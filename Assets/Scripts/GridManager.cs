using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager gridManager;

    [SerializeField] Vector2Int gridSize;
    [SerializeField] Node node;

    int unitySnapSize = 10;
    public int UnitySnapValue { get{ return unitySnapSize;}}

    Dictionary<Vector2Int,Node> grid = new Dictionary<Vector2Int,Node>();
    public Dictionary<Vector2Int,Node> Grid { get { return grid; } }

    void Awake()
    {
        gridManager = this;

        for(int i = 0;i<gridSize.x;i++)
        {
            for(int j = 0;j<gridSize.y;j++)
            {
                Vector2Int newCoordinates = new Vector2Int(i,j);
                grid.Add(newCoordinates,new Node(newCoordinates,true));
            }
        }
    }

    public void ResetNodes()
    {
        foreach(KeyValuePair<Vector2Int,Node> node in grid)
        {
            node.Value.connectedTo = null;
            node.Value.isExploride = false;
            node.Value.isPath = false;
        }
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWakable = false;
            grid[coordinates].isBlocked = true;
        }
    }

    public void UnBlockNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWakable = true;
            grid[coordinates].isBlocked = false;
        }
    }

    public Vector3 CoordinatesToPosition(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x*unitySnapSize;
        position.y = 0;
        position.z = coordinates.y*unitySnapSize;

        return position;
    }

    public Vector2Int PositionToCoordinates(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x/unitySnapSize);
        coordinates.y = Mathf.RoundToInt(position.z/unitySnapSize);

        return coordinates;
    }


    public Node GetNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }
}
