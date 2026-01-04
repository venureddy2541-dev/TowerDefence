using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWakable;
    public bool isPath;
    public bool isExploride;
    public bool isBlocked;
    public Node connectedTo;

    public Node(Vector2Int coordinates,bool isWakable)
    {
        this.coordinates = coordinates;
        this.isWakable = isWakable;
    }
}
