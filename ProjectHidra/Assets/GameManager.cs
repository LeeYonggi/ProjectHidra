using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public UnityEvent turnEndEvent;

    [SerializeField]
    private ResourcesStatus resourceStatus = null;

    public static GameManager Instance { get => instance; }
    public ResourcesStatus ResourceStatus { get => resourceStatus; set => resourceStatus = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}


public interface GameStateMachine
{

}
