using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesStatus : MonoBehaviour
{
    private int mineral = 0;
    private int maxStructure = 0;
    private int nowStructure = 0;

    public int Mineral
    {
        get => mineral;
        set
        {
            if (value >= 0)
                mineral = value;
        }
    }
    public int MaxStructure
    {
        get => maxStructure;
        set
        {
            if(value >= 0)
                maxStructure = value;
        }
    }

    public int NowStructure
    {
        get => nowStructure;
        set
        {
            if(value >= 0)
                nowStructure = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
