using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraManager : MonoBehaviour
{
    private static DijkstraManager instance = null;

    public static DijkstraManager Instance { get => instance; set => instance = value; }

    public FieldManager field = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}


public struct TileNode
{
    public GameObject tile;
    public List<TileNode> childNodes;
    public List<int> nodeDistances;
}