using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

public class Node : MonoBehaviour
{
    public string nodeType = "";
    public int toNodeCost = 0;
    public int costSoFar = 0;
    public GameObject fromNode = null;
    public List<GameObject> neighbours = new List<GameObject>();

    private BoardManager boardManager;

    private void Start()
    {
        CalculateCost();
    }

    public void FindNeighbours()
    {
        boardManager = FindObjectOfType<BoardManager>();
        neighbours = boardManager.GetNeighbours((int)transform.position.x, (int)transform.position.y);
    }

    public void CalculateCost()
    {
        switch (nodeType)
        {
            case "F":
                toNodeCost = 5;
                transform.localScale = Vector3.one;
                break;

            case "P":
                toNodeCost = 5;
                transform.localScale = Vector3.one * 2;
                break;

            case "E":
                toNodeCost = 5;
                transform.localScale = Vector3.one * 2;
                break;

            case "W":
                toNodeCost = 150;
                break;

            case "f":
                toNodeCost = 2;
                break;

            case "e":
                toNodeCost = 25;
                break;
        }
    }
}
