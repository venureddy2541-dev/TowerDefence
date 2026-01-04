using UnityEngine;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    public static PathFinder pathFinder;

    [SerializeField] Vector2Int startCoords;
    public Vector2Int StartCoords { get { return startCoords;}}
    [SerializeField] Vector2Int destinationCoords;

    Node startNode;
    Node destinationNode;
    Node currentNode;

    Vector2Int[] directions = { Vector2Int.right,Vector2Int.left,Vector2Int.up,Vector2Int.down };

    Dictionary<Vector2Int,Node> grid = new Dictionary<Vector2Int,Node>();
    Queue<Node> path = new Queue<Node>();
    Dictionary<Vector2Int,Node> exploridePath =  new Dictionary<Vector2Int,Node>();

    void Awake()
    {
        pathFinder = this;

        if(GridManager.gridManager != null)
        {
            grid = GridManager.gridManager.Grid;
            startNode = grid[startCoords];
            destinationNode = grid[destinationCoords];
        }
    }

    void Start()
    {
        RecalculatePath();
    }

    public List<Node> RecalculatePath()
    {
        return RecalculatePath(startCoords);
    }

    public List<Node> RecalculatePath(Vector2Int coordinates)
    {
        Reset();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    void Reset()
    {
        GridManager.gridManager.ResetNodes();
    }

    void pahtFinder()
    {
        List<Node> nighbours = new List<Node>();
        foreach(Vector2Int direction in directions)
        {
            Vector2Int newDirection = currentNode.coordinates + direction;
            if(grid.ContainsKey(newDirection))
            {
                nighbours.Add(grid[newDirection]);
            }
        }

        foreach(Node nighbour in nighbours)
        {
            if(!exploridePath.ContainsKey(nighbour.coordinates) && nighbour.isWakable == true)
            {
                nighbour.connectedTo = currentNode;
                path.Enqueue(nighbour);
                exploridePath.Add(nighbour.coordinates,nighbour);
                nighbour.isExploride = true;
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        bool isRunning = true;

        path.Clear();
        exploridePath.Clear();

        startNode.isWakable = true;
        destinationNode.isWakable = true;

        path.Enqueue(grid[coordinates]);
        exploridePath.Add(coordinates,grid[coordinates]);

        while(path.Count > 0 && isRunning)
        {
            currentNode = path.Dequeue();
            currentNode.isExploride = true;
            pahtFinder();
            if(currentNode.coordinates == destinationCoords)
            {
                isRunning = false;
            }
        }
    }

    public List<Node> BuildPath()
    {
        List<Node> Path = new List<Node>();
        Node connected = destinationNode;
        Path.Add(connected);
        connected.isPath = true;

        while(connected.connectedTo != null)
        {
            connected = connected.connectedTo;
            Path.Add(connected);
            connected.isPath = true;
        }

        Path.Reverse();
        return Path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWakable;

            grid[coordinates].isWakable = false;
            List<Node> checkPath = RecalculatePath();
            grid[coordinates].isWakable = previousState;

            if(checkPath.Count<=1)
            {
                return true;
            }
        }
        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("NewPath",false,SendMessageOptions.DontRequireReceiver);
    }
}
