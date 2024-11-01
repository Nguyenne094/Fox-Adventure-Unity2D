using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int _width, _height;
    private int _worldPosX, _worldPosY;
    private Vector2 nodeSize;
    private Node[,] grid;

    public LayerMask unwalkableLayer;

    private void Start() {
        nodeSize = Vector2.one;
        _worldPosX = Mathf.RoundToInt(transform.position.x);
        _worldPosY = Mathf.RoundToInt(transform.position.y);
        InitGrid();
    }

    private void Update() {
        InitGrid();
    }

    private void InitGrid()
    {
        grid = new Node[_width, _height];

        bool walkable = default;
        Vector3 worldPos = default;
        Vector3 worldBottomLeft = new Vector3(_worldPosX - (_width/2), _worldPosY - (_height/2), 1f);
        
        for(int i = 0; i < _width; i++){
            for(int j = 0; j < _height; j++){
                worldPos = worldBottomLeft + Vector3.right * (i * nodeSize.x + nodeSize.x/2) + Vector3.up * (j * nodeSize.y + nodeSize.y/2) + new Vector3(0,0,-1f);
                walkable = !(Physics2D.OverlapBox(worldPos, nodeSize, unwalkableLayer));
                grid[i, j] = new Node(i, j, walkable, worldPos);
                
            }
        }
    }

    private void OnDrawGizmos() {   
        if(grid != null){
            foreach(Node node in grid){
                Gizmos.color = node._walkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(node._worldPos, nodeSize - new Vector2(0.1f, 0.1f));
            }
        }
    }
}
