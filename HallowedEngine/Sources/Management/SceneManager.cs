using System.Collections.Generic;
using Hallowed.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Management;

public static class SceneManager
{

  public static IScene CurrentScene => Stack.Peek();
  
  private static readonly Stack<IScene> Stack = new(); 
  
  public static Game GameProcess {get; private set; }
  public static ContentManager Content { get; private set; }
  /// <summary>
  /// the shared spritebatch for every scene in the game. It can however not be used.
  /// </summary>
  public static SpriteBatch SharedSpriteBatch { get; private set; }

  public static bool IsExisting { get; private set; } = false;

  public static void Run(Game gameProcess)
  {
    GameProcess = gameProcess;
    GameProcess.Run();
  }
  public static void Run(Game gameProcess, GameRunBehavior gameRunBehavior)
  {
    GameProcess = gameProcess;
    GameProcess.Run(gameRunBehavior);
  }

  public static void Initialize()
  {
    Content = GameProcess.Content;
    Content.RootDirectory = "Content";
    SharedSpriteBatch = new SpriteBatch(GameProcess.GraphicsDevice);
    CurrentScene.Initialize();
  }
  // clear the stack and go to the next scene without keeping the stack.
  public static void Goto(IScene scene)
  {
    IsExisting = true;
    Stack.Clear();
    Stack.Push(scene);
  }

  // in this case we pushing the new scene without clearing the stack meaning it can be popped.
  // usage can be when you want to do a menu or a temporary scene but dont want to have to unload the content.
  public static void Push(IScene scene)
  {
    CurrentScene.Pause();
    Stack.Push(scene);
  }
  
  public static void Pop()
  {
    IsExisting = false;
    Stack.Pop();
    if (Stack.Count == 0) GameProcess.Exit();
  }

  private static void Terminate()
  {
    // change the class to allow unload and such.
    CurrentScene?.Dispose();
  }

  public static void Load()
  {
    CurrentScene?.LoadContent();
  }
  
  public static void Unload()
  {
    CurrentScene?.UnloadContent();
    CurrentScene?.Dispose();
    Content?.Unload();
  }

  public static void Dispose()
  {
    CurrentScene?.Dispose();
    Content?.Dispose();
    Stack.Clear();
  }
  public static void Update(GameTime gameTime)
  {
    if (IsExisting)
    {
      // do my unloading here
    }
    // here we check if a scene is ready to transsition etc but ill do it after.
   // CurrentScene?.Update(gameTime);
  }
  
  public static void Draw(GameTime gameTime)
  {
    CurrentScene?.Draw(gameTime);
  }
}
