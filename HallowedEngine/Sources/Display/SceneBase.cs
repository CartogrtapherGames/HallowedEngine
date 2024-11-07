using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

/// <summary>
/// the abstract class that offers a simple and easy workflow for your rendering.
/// </summary>
public abstract class SceneBase : Game
{
  protected static SceneBase Instance = null;
  protected readonly List<IRenderable> Children = [];
  protected SpriteBatch SpriteBatch;

  protected SceneBase()
  {
    Instance = this;
    // init the graphics;
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
