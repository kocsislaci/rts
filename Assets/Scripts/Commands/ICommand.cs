using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public void Execute();
}
public class MoveCommand : ICommand
{
    public void Execute()
    {
        throw new System.NotImplementedException();
    }
}
public class AttackCommand : ICommand
{
    public void Execute()
    {
        throw new System.NotImplementedException();
    }
}
public class GatherCommand : ICommand
{
    public void Execute()
    {
        throw new System.NotImplementedException();
    }
}