using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance { get => instance; }
    

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
