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
                toNodeCost = 10;
                break;

            case "W":
                toNodeCost = 30;
                break;

            case "O":
                // the player looses 1 food for each step
                // and earn 10 food when they hit a food tile
                // this means the cost of entering food tiles
                // is 10 times the negative of entering a floor tile
                toNodeCost = 0;
                break;

            case "N":
                toNodeCost = 100;
                break;

            default:
                toNodeCost = 10;
                break;
        }
    }
}
