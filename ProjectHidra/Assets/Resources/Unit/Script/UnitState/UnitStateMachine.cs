using UnityEngine;
using UnityEditor;

public interface UnitStateMachine
{
    void Update(Unit unit);
    void SendMessage(GameObject obj, string message);
}