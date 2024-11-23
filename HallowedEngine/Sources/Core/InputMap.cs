

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace Hallowed.Core;

/// <summary>
/// The wrapper class that map and bind keyboard and gamepad input into a
/// collection of actions
/// </summary>
/// <typeparam name="T">the enum where the inputs are bound too</typeparam>
public class InputMap<T> where T : System.Enum
{
  private readonly Dictionary<T, List<AbstractKey>> _actions = new();
  private GamePadState _newGamePadState;
  private KeyboardState _newState;
  private GamePadState _oldGamePadState;
  private KeyboardState _oldState;
  
  /// <summary>
  /// Access to the Actions Map so you can change them if needed at runtime
  /// </summary>
  /// <example>
  /// Change the actions' bindings at runtime such as remapping the keyboard
  /// </example>
  public Dictionary<T, List<AbstractKey>> Actions => _actions;
  
  /// <summary>
  /// Determines whether the specified action is currently being pressed.
  /// </summary>
  /// <remarks>
  /// It will handle both </remarks>
  /// <param name="name">The action to check.</param>
  /// <returns>True if the action is pressed, otherwise false.</returns>
  /// <exception cref="Exception">Thrown when the specified action does not exist.</exception>
  public bool IsPressed(T name)
  {
    if (!_actions.TryGetValue(name, out var action)) throw new Exception($"the action {name} does not exists!");
    foreach (var input in action)
    {
      switch (input.Type)
      {
        case InputType.Keyboard:
        {
          var key = (Keys)input.Index;
          if (GetState().IsKeyDown(key))
          {
            return true;
          }
          break;
        }
        case InputType.Gamepad:
        {
          var button = (Buttons)input.Index;
          if (GetGamepadState().IsButtonDown(button))
          {
            return true;
          }
          break;
        }
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    return false;
  }
  
  /// <summary>
  /// Determines whether the specified key is currently being pressed.
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  public bool IsPressed(Keys key)
  {
    return GetState().IsKeyDown(key);
  }


  /// <summary>
  /// Check whether the specified action is just released 
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  /// <exception cref="Exception"></exception>
  public bool IsUp(T name)
  {
    if (!_actions.TryGetValue(name, out List<AbstractKey> value)) throw new Exception($"the action {name} does not exists!");
    var action = value;
    foreach (var input in action)
    {
      switch (input.Type)
      {
        case InputType.Keyboard:
          var key = (Keys)input.Index;
          return GetState().IsKeyUp(key);
        case InputType.Gamepad:
          var button = (Buttons)input.Index;
          return GetGamepadState().IsButtonUp(button);
      }
    }

    return false;
  }

  /// <summary>
  /// Check whether the specified key is just released 
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  /// <exception cref="Exception"></exception>
  public bool IsUp(Keys key)
  {
    return GetState().IsKeyUp(key);
  }
  
  /// <summary>
  /// Check whether the specified action is pressed only once.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  /// <exception cref="Exception"></exception>
  public bool IsTriggered(T name)
  {
    if (!_actions.TryGetValue(name, out List<AbstractKey> value)) throw new Exception($"the action {name} does not exists!");
    foreach (var input in value)
    {
      switch (input.Type)
      {
        case InputType.Keyboard:
          var key = (Keys)input.Index;
          return (GetState().IsKeyDown(key) && _oldState.IsKeyUp(key));
        case InputType.Gamepad:
          var button = (Buttons)input.Index;
          return (GetGamepadState().IsButtonDown(button) && _oldGamePadState.IsButtonUp(button));
      }
    }

    return false;
  }

  /// <summary>
  /// Check whether the specified key is pressed only once.
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  public bool IsTriggered(Keys key)
  {
    return GetState().IsKeyDown(key) && _oldState.IsKeyUp(key);
  }

  /// <summary>
  /// Update the InputMap
  /// </summary>
  public void Update()
  {
    _newState = GetState();
    _oldState = _newState;
    _newGamePadState = GetGamepadState();
    _oldGamePadState = _newGamePadState;
  }
  
  #region Binding

  /// <summary>
  /// will bind keys to an action
  /// </summary>
  /// <param name="name">the input action</param>
  /// <param name="key"> the key to bind to the action</param>
  public void BindAction(T name, Keys key)
  {
    var input = new AbstractKey()
    {
      Type = InputType.Keyboard,
      Index = (int)key
    };

    if (_actions.ContainsKey(name))
    {
      _actions[name].Add(input);
    }
    else
    {
      var list = new List<AbstractKey>() { input };
      _actions.Add(name, list);
    }
  }

  /// <summary>
  /// bind an array of keys  to an action
  /// </summary>
  /// <param name="name">the input action</param>
  /// <param name="keys">the array of keys to bind to the action</param>
  public void BindAction(T name, Keys[] keys)
  {
    if (_actions.ContainsKey(name))
    {
      foreach (var t in keys)
      {
        var input = new AbstractKey()
        {
          Type = InputType.Keyboard,
          Index = (int)t
        };
        _actions[name].Add(input);
      }
    }
    else
    {
      var list = ConvertToList(keys, InputType.Keyboard);
      _actions.Add(name, list);
    }
  }

  /// <summary>
  /// bind a list of keys to an action
  /// </summary>
  /// <param name="name">the input action</param>
  /// <param name="keys">the list of keys to bind to the action</param>
  public void BindAction(T name, List<Keys> keys)
  {
    if (_actions.ContainsKey(name))
    {
      var list = ConvertToList(keys, InputType.Keyboard);
      var result = list.Concat(_actions[name]).ToList();
      _actions[name] = result;
    }
    else
    {
      var list = ConvertToList(keys, InputType.Keyboard);
      _actions.Add(name, list);
    }
  }

  /// <summary>
  /// bind a button to an action
  /// </summary>
  /// <param name="name"> the input action</param>
  /// <param name="button"> the button to bind to the action</param>
  public void BindAction(T name, Buttons button)
  {
    var input = new AbstractKey()
    {
      Type = InputType.Gamepad,
      Index = (int)button
    };
    if (_actions.ContainsKey(name))
    {
      _actions[name].Add(input);
    }
    else
    {
      var list = new List<AbstractKey>() { input };
      _actions.Add(name, list);
    }
  }

  /// <summary>
  /// bind an array of buttons to an action
  /// </summary>
  /// <param name="name">the input action</param>
  /// <param name="buttons">the array of buttons to bind to the action</param>
  public void BindAction(T name, Buttons[] buttons)
  {
    if (_actions.ContainsKey(name))
    {
      foreach (var button in buttons)
      {
        var input = new AbstractKey()
        {
          Type = InputType.Gamepad,
          Index = (int)button
        };
        _actions[name].Add(input);
      }
    }
    else
    {
      var list = ConvertToList(buttons, InputType.Gamepad);
      _actions.Add(name, list);
    }
  }

  /// <summary>
  /// bind a list of buttons to an action
  /// </summary>
  /// <param name="name"> the input action</param>
  /// <param name="buttons">the list of buttons to bind to the action</param>
  public void BindAction(T name, List<Buttons> buttons)
  {
    if (_actions.TryGetValue(name, out List<AbstractKey> value))
    {
      var list = ConvertToList(buttons, InputType.Gamepad);
      var result = list.Concat(value).ToList();
      _actions[name] = result;
    }
    else
    {
      var list = ConvertToList(buttons, InputType.Gamepad);
      _actions.Add(name, list);
    }
  }

  #endregion

  private static KeyboardState GetState() => PlatformGetState();
  private static GamePadState GetGamepadState() => GamePad.GetState(PlayerIndex.One);

  private static KeyboardState PlatformGetState()
  {
    return Keyboard.GetState();
  }
  
  #region ListConverter

  private static List<AbstractKey> ConvertToList(List<Keys> keys, InputType type)
  {
    var list = new List<AbstractKey>();
    foreach (var key in keys)
    {
      var input = new AbstractKey()
      {
        Type = type,
        Index = (int)key
      };
    }

    return list;
  }

  private static List<AbstractKey> ConvertToList(Keys[] keys, InputType type)
  {
    var list = new List<AbstractKey>();
    foreach (var key in keys)
    {
      var input = new AbstractKey()
      {
        Type = type,
        Index = (int)key
      };
    }

    return list;
  }

  private static List<AbstractKey> ConvertToList(List<Buttons> buttons, InputType type)
  {
    var list = new List<AbstractKey>();
    foreach (var button in buttons)
    {
      var input = new AbstractKey()
      {
        Type = type,
        Index = (int)button
      };
    }

    return list;
  }

  private static List<AbstractKey> ConvertToList(Buttons[] buttons, InputType type)
  {
    var list = new List<AbstractKey>();
    foreach (var button in buttons)
    {
      var input = new AbstractKey()
      {
        Type = type,
        Index = (int)button
      };
    }

    return list;
  }

  #endregion
}

/// <summary>
/// the struct that defines an abstract key that can act both for a keyboard and a gamepad 
/// </summary>
public struct AbstractKey
{
  public InputType Type;
  public int Index;
}

/// <summary>
/// The abstract key Input Type which is either keyboard or gamepad
/// </summary>
public enum InputType
{
  Keyboard,
  Gamepad
}
