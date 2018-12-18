using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private Transform fromNode = null;
    [SerializeField] private List<Transform> toNodes;
}
