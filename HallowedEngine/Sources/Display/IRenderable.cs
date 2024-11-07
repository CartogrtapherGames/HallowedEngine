using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

/// <summary>
/// the interface that offers a contract approach for the tree rendering system.
/// </summary>
public interface IRenderable
{
 // public event Action<IRenderable> OnTransformChanged;
  public void Update(GameTime delta);
  public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
  public void Dispose();
  public bool Enabled { get; set; }
  public float X { get; set; }
  public float Y { get; set; }
  public int Width { get; }

  public int Height { get; }
}
