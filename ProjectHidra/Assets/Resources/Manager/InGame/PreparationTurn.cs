using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PreparationTurn : GameStateMachine
{
    const float preparationTime = 30.0f;
    public PreparationTurn()
    {
        Time.timeScale = 0.0f;
        GameManager.Instance.StartCoroutine(TimerCoroutine());

        GameManager.Instance.ResourcesCanvas.ChangeMessageText("배치 시간입니다!");

        GameManager.Instance.LayOut.SetActive(true);
    }

    void GameStateMachine.Update()
    {

    }

    public IEnumerator TimerCoroutine()
    {
        float time = preparationTime;

        while (time >= 0)
        {
            GameManager.Instance.Timer = (int)time;
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        GameManager.Instance.LayOut.SetActive(false);

        GameManager.Instance.ChangeStateMachine(new SimulationTurn());

        yield return null;
    }
}
