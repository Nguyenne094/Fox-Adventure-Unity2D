using UnityEngine;

namespace TestScene{
    public class Node
    {
        public int hCost, gCost;
        public int x, y;
        public bool _walkable = false;
        public Vector3 _worldPoint;
        public Node parent;

        public Node(bool walkable, Vector3 worldPoint, int x, int y)
        {
            this._walkable = walkable;
            this._worldPoint = worldPoint;
            this.x = x;
            this.y = y;
        }

        public int fCost {  
                get{
                return hCost + gCost;
            }
        }
    }
}