using System;
using System.Collections.Generic;
using System.Linq;
using Hallowed.Display;
using Hallowed.Entity.Experimental;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Entity;

/// <summary>
/// the abstract class that define the shape of a game object.
/// </summary>
public abstract class GameObject : IRenderable, IDestroyable
{
  
  private readonly List<IComponent> _components = [];
  private readonly Dictionary<Type, IComponent> _componentsDict = new();
  private Vector2 _pivot;

  // in this case we dont really need this for an Game Object?? not sure yet
  //public event Action<IRenderable> OnTransformChanged;

  private Vector3 _transform;
  public string GroupId; // in this case allows us to get a group of all the game objects 

  public string Id; // in this case we could easily do lookup etc

  protected GameObject()
  {
    _transform = Vector3.Zero;
    _pivot = Vector2.Zero;
  }

  public virtual void Update(GameTime delta)
  {
    if (HasComponents())
    {
      foreach (var component in _components.Where(component => component.Enabled))
      {
        component.Update(delta);
      }
    }
  }

  public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    if (HasComponents())
    {
      foreach (var component in _components)
      {
        if (component is IRenderable componentRenderable)
        {
          componentRenderable.Draw(spriteBatch, gameTime);
        }
      }
    }
  }

  public virtual void Dispose()
  {
    if (HasComponents())
    {
      foreach (var component in _components)
      {
        if (component is IDisposable disposable)
        {
          disposable.Dispose();
        }
      }
    }
  }


  protected void RefreshTransform()
  {
    if (HasComponents())
    {
      foreach (var component in _components)
      {
        if (component is IRenderable componentRenderable)
        {
          componentRenderable.X = _transform.X;
          componentRenderable.Y = _transform.Y;
        }
      }
    }
  }

  protected void AddComponent<T>(IComponent component) where T : class, IComponent
  {
    _components.Add(component);
    _componentsDict[typeof(T)] = component;
  }

  protected T GetComponent<T>() where T : class, IComponent
  {
    if (_componentsDict.ContainsKey(typeof(T)))
    {
      return _componentsDict[typeof(T)] as T;
    }

    return null;
  }

  private bool HasComponents()
  {
    return _components.Count != 0;
  }

  #region GetSetters

  public bool Enabled { get; set; }

  public Vector3 Transform
  {
    get => _transform;
    set
    {
      _transform = value;
      RefreshTransform();
    }
  }

  public Vector2 Pivot
  {
    get => _pivot;
    set
    {
      var x = Math.Clamp(value.X, 0f, 1f);
      var y = Math.Clamp(value.Y, 0f, 1f);
      _pivot = new Vector2(x, y);
      RefreshTransform();
    } 
  }

  public float X
  {
    get => _transform.X;
    set
    {
      _transform.X = value;
      RefreshTransform();
    } 
  }

  public float Y
  {
    get => _transform.Y;
    set
    {
      _transform.Y = value;
      RefreshTransform();
    } 
  }

  public int Width { get; }
  public int Height { get; }

  #endregion

  public void Destroy()
  {
    IsDestroyed = true;
  }

  public bool IsDestroyed { get; set; } = false;
}
