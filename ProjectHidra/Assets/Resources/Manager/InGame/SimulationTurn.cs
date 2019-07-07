using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SimulationTurn : GameStateMachine
{
    private int time = 0;
    public SimulationTurn()
    {
        Time.timeScale = 1.0f;

        GameManager.Instance.ResourcesCanvas.ChangeMessageText("공격 시간입니다!", 2.0f);

        GameManager.Instance.Timer = 20;

        GameManager.Instance.turnStartEvent.Invoke();

        time = 20;

        GameManager.Instance.StartCoroutine(TimeCoroutine());
    }

    void GameStateMachine.Update()
    {
    }

    IEnumerator TimeCoroutine()
    {
        while(time >= 0)
        {
            GameManager.Instance.Timer = time;

            yield return new WaitForSeconds(1.0f);

            time -= 1;
        }
        GameManager.Instance.ChangeStateMachine(new PreparationTurn());
    }
}
