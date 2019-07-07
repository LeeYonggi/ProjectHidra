using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField]
    private ResourcesStatus resourceStatus = null;

    [SerializeField]
    private ResourcesCanvas resourcesCanvas = null;

    [SerializeField]
    private GameObject layOut = null;

    public UnityEvent turnStartEvent = null;
    public UnityEvent turnEndEvent = null;
    
    private GameStateMachine stateMachine = null;

    public int Timer { get; set; } = 20;

    public static GameManager Instance { get => instance; }
    public ResourcesStatus ResourceStatus { get => resourceStatus; set => resourceStatus = value; }
    public GameStateMachine StateMachine { get => stateMachine; }
    public ResourcesCanvas ResourcesCanvas { get => resourcesCanvas; set => resourcesCanvas = value; }
    public GameObject LayOut { get => layOut; set => layOut = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        resourceStatus.Mineral = 10;
        resourceStatus.MaxStructure = 5;
        resourceStatus.NowStructure = 0;

        ChangeStateMachine(new PreparationTurn());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
    
    public void ChangeStateMachine(GameStateMachine state)
    {
        stateMachine = state;
    }
}

