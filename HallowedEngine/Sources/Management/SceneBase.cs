using System;
using System.Collections.Generic;
using System.Linq;
using Hallowed.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Management;


/// <summary>
/// the abstract class that give a context and structure to a scene.
/// it offers a flexible and optional tree list children system.
/// </summary>
public abstract class SceneBase : IScene, IDisposable
{

  protected ContentManager Content;
  protected SpriteBatch SpriteBatch;
  
  protected readonly List<IRenderable> Children = [];
  

  public virtual void Initialize()
  {
    Content = SceneManager.Content;
    SpriteBatch = SceneManager.SharedSpriteBatch;
  }

  public virtual void LoadContent()
  {
    throw new System.NotImplementedException();
  }

  public virtual void Update(GameTime gameTime)
  {
    foreach (var child in Children.Where(child => child.Enabled))
    {
      child.Update(gameTime);
    }
  }

  public virtual void Draw(GameTime gameTime)
  {
    if (Children.Count != 0)
    {
      SpriteBatch.Begin();
      foreach (var child in Children.Where(child => child.Enabled))
      {
        child.Draw(SpriteBatch, gameTime);
      }
      SpriteBatch.End();
    }
  }

  public virtual void UnloadContent()
  {
    throw new System.NotImplementedException();
  }

  public  virtual void Pause()
  {
    throw new System.NotImplementedException();
  }

  public virtual void Resume()
  {
    throw new System.NotImplementedException();
  }

  protected void AddChild(IRenderable child)
  {
    Children.Add(child);
  }
  
  protected void AddChild(IRenderable[] children)
  {
    Children.AddRange(children);
  }
  
  protected void RemoveChild(IRenderable child)
  {
    Children.Remove(child);
    child.Dispose();
  }
  
  public void Dispose()
  {
    // TODO release managed resources here
  }

}
