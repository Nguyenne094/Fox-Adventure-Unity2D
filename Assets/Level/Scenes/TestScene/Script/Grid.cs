using System.Collections.Generic;
using UnityEngine;

namespace TestScene{
    public class Grid : MonoBehaviour
    {
        public int _width;
        public int _height;
        private Vector3 boxSize;
        private float nodeRadius = 0.5f;
        private Node[,] grid;
        public LayerMask unwalkableMask;
        
        private void Start() {
            transform.localScale = new Vector3(_width, _height, 1);
            boxSize = Vector3.one;
            DrawGrid();
        }

        private void DrawGrid()
        {
            grid = new Node[_width, _height];
            Vector3 worldButtomLeft = transform.position - transform.localScale / 2;

            for(int i = 0; i < _width; i++){
                for(int j = 0; j < _height; j++){
                    Vector3 worldPoint = worldButtomLeft + Vector3.right * (i * boxSize.x + boxSize.x / 2) + Vector3.up * (j * boxSize.y + boxSize.y / 2) + new Vector3(0, 0, -1f);
                    bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                    grid[i, j] = new Node(walkable, worldPoint, i, j);
                }
            }
        }

        //Get neighbour nodes of a node (3x3 grid size, [0,0] is the node at the center)
        public List<Node> GetNeighbours(Node node){
            List<Node> neighbours = new List<Node>();

            for(int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    if(x == 0 && y == 0){
                        continue;
                    }
                    int checkX = node.x + x;
                    int checkY = node.y + y;
                    if(checkX >= 0 && checkX < _width && checkY >= 0 && checkY < _height){
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        //Get node from world position
        public Node NodeFormWorldPoint(Vector3 worldPos){
            float percentX = (worldPos.x + _width / 2) / _width;
            float percentY = (worldPos.y + _height / 2) / _height;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_width - 1) * percentX);
            int y = Mathf.RoundToInt((_height - 1) * percentY);
            return grid[x, y];
        }

        public List<Node> path;

        //Draw Gizmos for testing
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, transform.localScale);

            if(grid != null){
                foreach(Node node in grid){
                    Gizmos.color = node._walkable ? Color.white : Color.red;
                    if(path != null){
                        if(path.Contains(node)){
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawWireCube(node._worldPoint, boxSize - new Vector3(0.1f, 0.1f, 0));
                }
            }
        }
    }
}
