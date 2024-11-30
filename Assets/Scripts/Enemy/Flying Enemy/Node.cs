using UnityEngine;

public class Node
{
    public int _gridX, _gridY;
    public int g, h;
    public bool _walkable = false;
    public Vector3 _worldPos;

    public Node(int gridX, int gridY, bool walkable, Vector3 worldPos)
    {
        this._gridX = gridX;
        this._gridY = gridY;
        this._walkable = walkable;
        this._worldPos = worldPos;    
    }

    public int F { get => g + h; }
}
