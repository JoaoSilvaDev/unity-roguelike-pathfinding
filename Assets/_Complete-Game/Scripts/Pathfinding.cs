using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;
using TMPro;

public class Pathfinding : MonoBehaviour
{
    public Player player;
    public BoardManager boardManager;

    public List<GameObject> path = new List<GameObject>();
    public List<GameObject> open = new List<GameObject>();
    public List<GameObject> closed = new List<GameObject>();

    public GameObject current = null;
    public Node currentNode = null;

    public GameObject start = null;
    public GameObject end = null;

    public LineRenderer line;

    public List<GameObject> Dijkstra()
    {
        print("Started Dijsktra");
        if (!boardManager) boardManager = FindObjectOfType<BoardManager>();
        if (!player) player = GetComponent<Player>();

        start = boardManager.playerNode;
        end = boardManager.endNode;

        path = new List<GameObject>();
        open = new List<GameObject>();
        closed = new List<GameObject>();
        current = null;
        currentNode = null;

        open.Add(start);

        while (!(open.Count == 0))
        {
            //print("########## OPEN COUNT " + open.Count + " ##########");
            current = GetMinCostNode();
            currentNode = current.GetComponent<Node>();

            if (current == end)
                break;

            currentNode.FindNeighbours();

            for (int i = 0; i < currentNode.neighbours.Count; i++)
            {
                print("neighbours " + currentNode.neighbours.Count);

                GameObject currentNeighbour = currentNode.neighbours[i];
                Node currentNeighbourNode = currentNeighbour.GetComponent<Node>();
                int toNodeCost = currentNode.costSoFar + currentNeighbourNode.toNodeCost;

                if (closed.Contains(currentNeighbour)) continue;

                if (open.Contains(currentNeighbour))
                {
                    if (currentNeighbourNode.costSoFar <= toNodeCost) continue;
                }
                else
                {
                    open.Add(currentNeighbour);
                }

                currentNeighbourNode.costSoFar = toNodeCost;
                currentNeighbourNode.fromNode = current;
            }

            open.Remove(current);
            closed.Add(current);
        }

        //print("I AM OUTSIDE THE WHILE");

        if (current == end)
        {

            path.Add(end);
            Node currentPathNode = end.GetComponent<Node>();

            //print("I ADDED THE END TO THE PATH");

            while (currentPathNode.fromNode != start)
            {
                //print("node: " + currentPathNode + " / fromNode: " + currentPathNode.fromNode + " / fromNodefromNode: " + 
                //currentPathNode.fromNode.GetComponent<Node>().fromNode);

                path.Add(currentPathNode.fromNode);
                currentPathNode = currentPathNode.fromNode.GetComponent<Node>();

                // if the while stats an infinite loop, break it
                if (currentPathNode.gameObject == currentPathNode.fromNode.GetComponent<Node>().fromNode)
                    break;
            }

            //print("IM FINISHED ADDING NODES TO THE PATH");

            path.Reverse();

            DrawLine();

            //print("FINISHED PATHFINDING PATH IS");

            if(start != end)
            Step();

            return path;

        }
        else
            return null;
    }

    public void Step()
    {
        int currentX = (int)transform.position.x;
        int currentY = (int)transform.position.y;

        int targetX = (int)path[0].transform.position.x;
        int targetY = (int)path[0].transform.position.y;

        player.Step(targetX - currentX, targetY - currentY);
    }

    public void DrawLine()
    {
        line.positionCount = 0;
        line.positionCount = path.Count;

        Vector3[] positions = new Vector3[path.Count+1];

        line.transform.position = player.transform.position;

        for (int i = 0; i < path.Count; i++)
        {
            positions[i] = path[i].transform.position - transform.position;
        }

        positions[path.Count] = end.transform.position - transform.position;

        line.SetPositions(positions);
    }

    public GameObject GetMinCostNode()
    {
        int minCost = int.MaxValue;
        GameObject minCostNode = null;

        foreach (GameObject go in open)
        {
            Node node = go.GetComponent<Node>();

            if (node.toNodeCost < minCost)
            {
                minCost = node.toNodeCost;
                minCostNode = go;
            }
        }

        //print("########## FINAL MIN COST " + minCostNode + " ##########");

        return minCostNode;
    }
}
