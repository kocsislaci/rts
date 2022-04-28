using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommandExecuter : MonoBehaviour
{
    private List<ICommand> _commands = new List<ICommand>();
    public List<ICommand> Commands
    {
        private get => _commands;
        set
        {
            _commands = value;
        }
    }

    private bool isExecuting = false;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Idle());
    }
    // tuskes banankigyo 
    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }
    public void ResetCommands()
    {
        _commands.Clear();
        isExecuting = false;
    }
    
    IEnumerator Idle()
    {
        while (true) // infinite loop
        {
            if (_commands.Count > 0 && !isExecuting)
            {
                ICommand command = _commands[0];
                if (command.GetType().Equals(typeof(MoveCommand)))
                {
                    StartCoroutine(Move(((MoveCommand) command)._targetPos));
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        
    }
    IEnumerator Move(Vector3 targetPos)
    {
        _navMeshAgent.SetDestination(targetPos);
        isExecuting = true;
        while (isExecuting)
        {
            yield return new WaitForSeconds(.1f);
            if ((targetPos - this.transform.position).magnitude < 0.2f)
            {
                _commands.RemoveAt(0);
                isExecuting = false;
            }
        }
    }

    
    
}
