using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public interface ICommand
{
    // public void Execute(GameObject executor);
}
public class MoveCommand : ICommand
{
    public Vector3 _targetPos;
    public MoveCommand(Vector3 clickPos)
    {
        _targetPos = clickPos;
    }
}
public class FollowCommand : ICommand
{
    public GameObject _targetUnit;
    public FollowCommand(GameObject targetUnit)
    {
        _targetUnit = targetUnit;
    }
}
// public class AttackCommand : ICommand
// {
//     public void Execute()
//     {
//         throw new System.NotImplementedException();
//     }
// }
// public class GatherCommand : ICommand
// {
//     public void Execute()
//     {
//         throw new System.NotImplementedException();
//     }
// }