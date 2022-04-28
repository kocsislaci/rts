using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExecuter : MonoBehaviour
{
    private List<ICommand> _commands = new List<ICommand>();

    public List<ICommand> Commands
    {
        get => _commands;
        set => _commands = value;
    }
}
