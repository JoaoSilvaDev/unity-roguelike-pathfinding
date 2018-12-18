using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

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
            {
                //print("current is end");
                break;
            }

            currentNode.FindNeighbours();

            for (int i = 0; i < currentNode.neighbours.Count; i++)
            {
                //print("neighbours " + currentNode.neighbours.Count);

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
            //print("I AM INSIDE THE WHILE");

            path.Add(end);
            Node currentPathNode = end.GetComponent<Node>();

            //print("I ADDED THE END TO THE PATH");

            while (currentPathNode.fromNode != start)
            {
                path.Add(currentPathNode.fromNode);
                currentPathNode = currentPathNode.fromNode.GetComponent<Node>();
            }

            //print("IM FINISHED ADDING NODES TO THE PATH");

            path.Reverse();

            DrawLine();

            //print("FINISHED PATHFINDING PATH IS");

            return path;
        }
        else
            return null;
    }    

    public void DrawLine()
    {
        line.positionCount = 0;
        line.positionCount = path.Count+1;

        Vector3[] positions = new Vector3[path.Count+2];

        line.transform.position = player.transform.position;

        positions[0] = Vector3.zero;

        for (int i = 0; i < path.Count; i++)
        {
            positions[i+1] = path[i].transform.position - transform.position;
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
