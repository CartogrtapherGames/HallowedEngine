using System.Collections.Generic;
using System.Linq;
using Hallowed.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Management;

/// <summary>
/// the abstract class that offers a simple and easy workflow for your rendering.
/// </summary>
public abstract class OldSceneBase : Game, IScene
{
  protected static OldSceneBase Instance = null;
  protected readonly List<IRenderable> Children = [];
  protected SpriteBatch SpriteBatch;

  protected OldSceneBase()
  {
    Instance = this;
    // init the graphics;
  }

  public void Initialize()
  {
    throw new System.NotImplementedException();
  }

  void IScene.LoadContent()
  {
    LoadContent();
  }

  void IScene.Update(GameTime gameTime)
  {
    Update(gameTime);
  }

  void IScene.Draw(GameTime gameTime)
  {
    Draw(gameTime);
  }

  public void UnloadContent()
  {
    throw new System.NotImplementedException();
  }

  protected override void LoadContent()
  {
    SpriteBatch = new SpriteBatch(GraphicsDevice);
    base.LoadContent();
  }

  protected override void Update(GameTime gameTime)
  {
    foreach (var child in Children.Where(child => child.Enabled))
    {
      child.Update(gameTime);
    }
    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    SpriteBatch.Begin();
    foreach (var child in Children.Where(child => child.Enabled))
    {
      child.Draw(SpriteBatch, gameTime);
    }
    SpriteBatch.End();
  }

  protected override void Dispose(bool disposing)
  {
    foreach (var child in Children)
    {
      child.Dispose();
    }
    Children.Clear(); // in this case we have disposed of all of them so we destroying all link
    base.Dispose(disposing);
  }

  public void Pause()
  {
    
  }

  public void Resume()
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

}
