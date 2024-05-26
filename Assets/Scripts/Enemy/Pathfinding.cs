using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    public GridData gridData;
    public GameObject gridDataObject;
    private Node targetNode;

    void Awake()
    {
        Instance = this;
    }

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        Node startNode = new Node(startPos);
        targetNode = new Node(targetPos);

        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();
        int count = 0;

        while (openSet.Count > 0 && count < 50)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || 
                    (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Equals(targetNode))
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                float newMovementCostToNeighbour = currentNode.gCost + Vector3Int.Distance(currentNode.position, neighbour.position);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = Vector3Int.Distance(neighbour.position, targetNode.position);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    } 
                    if(neighbour.hCost == 0) {
                        // If the cost is 0 then this neighbour is the target and we can just set it
                        // so we can still have access to parent for retracing steps
                        targetNode = neighbour;
                    }
                }
            }

            count++;
        }

        return null; // Return null if no path is found
    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        Vector3Int[] neighbourOffsets = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(0, 0, -1),
            new Vector3Int(1, 0, 1),
            new Vector3Int(-1, 0, -1),
            new Vector3Int(-1, 0, 1),
            new Vector3Int(1, 0, -1)
        };

        foreach (Vector3Int offset in neighbourOffsets)
        {
            Vector3Int neighbourPos = node.position + offset;
            if (IsWalkable(neighbourPos))
            {
                neighbours.Add(new Node(neighbourPos));
            }
        }

        return neighbours;
    }

    bool IsWalkable(Vector3Int position)
    {
        // Implement your logic to check if the position is walkable
        bool isOccupied = gridData.isPositionOccupied(new Vector2Int(position.x, position.z));
        if(isOccupied && new Node(position).Equals(targetNode)) {
            // The next loop iteration will handle not moving to this spot
            return true;
        }
        return !isOccupied;
    }

    List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode.position != startNode.position)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    public class Node
    {
        public Vector3Int position;
        public float gCost;
        public float hCost;
        public Node parent;

        public float FCost => gCost + hCost;

        public Node(Vector3Int position)
        {
            this.position = position;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Node other = (Node)obj;
            return position.Equals(other.position);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
