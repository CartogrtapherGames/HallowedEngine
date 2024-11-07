using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

public class Container : IRenderable
{

  protected List<IRenderable> Children;
  
  private Vector2 _position;
  private Vector2 _scale;

  public event Action<IRenderable> OnTransformChanged;
  
  // in this case we making this flag so we allow 
  private bool _isDirty;
  public bool Enabled { get; set; }

  public Container()
  {
    
  }

  public virtual void AddChild(IRenderable child)
  {
    Children.Add(child);
    UpdateTransform();
  }
  
  public virtual void Update(GameTime delta)
  {
    throw new System.NotImplementedException();
  }

  public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    throw new System.NotImplementedException();
  }

  public virtual void Dispose()
  {
    throw new System.NotImplementedException();
  }

  protected void UpdateTransform()
  {
    
  }
  #region Accessor
  public float X {get; set;}
  public float Y {get; set;}
  
  public int Width {get; set;}
  public int Height {get; set;}
  
  #endregion



}
