using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScene{
    public class A_start_PathFinding : MonoBehaviour
    {
        public Transform seeker, target;
        Grid grid;

        void Awake()
        {
            grid = GetComponent<Grid>();
        }

        private void Update() {
            Pathfinding(seeker.position, target.position);
        }

        //A* Pathfinding Algorithm
        private void Pathfinding(Vector3 startPos, Vector3 targetPos){
            Node startNode = grid.NodeFormWorldPoint(startPos);
            Node targetNode = grid.NodeFormWorldPoint(targetPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while(openSet.Count > 0){
                Node currentNode = openSet[0];
                for(int i = 1; i < openSet.Count; i++){
                    if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost){
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if(currentNode == targetNode){
                    Retrace(startNode, targetNode);
                    return;
                }

                foreach(Node neighbour in grid.GetNeighbours(currentNode)){
                    if(!neighbour._walkable || closedSet.Contains(neighbour)){
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if(!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        private int GetDistance(Node nodeA, Node nodeB){
            int dstX = Mathf.Abs(nodeA.x - nodeB.x);
            int dstY = Mathf.Abs(nodeA.y - nodeB.y);

            if(dstX > dstY){
                return (14 * dstY + 10*(dstX-dstY));
            }
            return (14 * dstX + 10*(dstY-dstX));
        }
    
        private void Retrace(Node startNode, Node tartgetNode){
            List<Node> path = new List<Node>();
            Node currentNode = tartgetNode;

            while(currentNode != startNode){
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            grid.path = path;
        }
    }
}