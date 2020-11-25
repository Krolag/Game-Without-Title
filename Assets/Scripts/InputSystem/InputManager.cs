using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActionsMap _actions;
    /// <summary>
    /// Singleton
    /// </summary>
    public static InputManager Instance { get; private set; }

    private InputActions _currentActions = InputActions.nullAction;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        _actions = new InputActionsMap();
        _actions.Enable();
    }

    private void Start()
    {
        SwitchInputActions(InputActions.Player);
    }
    /// <summary>
    /// Enable receiving callbacks from input system for instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance">Instance that should reveive callbacks</param>
    public void SetCallbacks<T>(T instance)
    {
        if (instance is InputActionsMap.IPlayerActions)
        {
            _actions.Player.SetCallbacks(instance as InputActionsMap.IPlayerActions);
        }
    }
    /// <summary>
    /// Change to another input action map, disable all others
    /// </summary>
    /// <param name="inputActions">Input action map to enable</param>
    public void SwitchInputActions(InputActions inputActions)
    {
        if (_currentActions == inputActions)
            return;
        foreach (InputActionMap actionMap in _actions.asset.actionMaps.ToArray())
        {
            actionMap.Disable();
        }

        switch (inputActions)
        {
            case InputActions.nullAction:
                break;
            case InputActions.Player:
                _actions.Player.Enable();
                break;
            default:
                break;
        }
        _currentActions = inputActions;
    }

}

public enum InputActions
{
    nullAction,
    Player
}
