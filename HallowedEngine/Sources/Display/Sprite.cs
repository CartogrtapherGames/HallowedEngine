using System;
using Hallowed.Entity.Experimental;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

/// <summary>
/// The class that represent a sprite on the screen
/// </summary>
public class Sprite : IDisposable, IRenderable, IComponent
{
  
  private Color _color;
  private Vector2 _position;
  private Vector2 _scale;
  private Texture2D _texture;

  private Vector2 _anchor;

  private readonly bool[] _mirror;
  private bool _enabled; // TODO : not useful anymore deprecated?
  private float _opacity;

  public event Action<IRenderable> OnTransformChanged;
  
  public Sprite()
  {
    _color = Color.White;
    _position = Vector2.Zero;
    _scale = Vector2.One;
    _color = Color.White;
    _anchor = Vector2.Zero;
    _mirror = [false, false];
    Rotation = 0f;
  }

  /// <summary>
  /// Move the sprite in a float coordinate.
  /// It also a shorthand function to
  /// sprite.x / sprite.y
  /// </summary>
  /// <example>
  /// <code>
  /// var sprite = new Sprite(myTexture);
  /// sprite.SetPos(10,10);
  /// //same as
  /// sprite.x = 10;
  /// sprite.y = 10;
  /// </code>
  /// </example>
  /// <param name="x"> The X coordinates in float </param>
  /// <param name="y"> The Y coordinates in float </param>
  public void SetPos(float x = 0, float y = 0)
  {
    _position.X = x;
    _position.Y = y;
  }

  /// <summary>
  /// Move the sprite to the specified position using a Vector2.
  /// It acts shorthand to sprite.x / sprite.y 
  /// </summary>
  /// <example>
  /// <code>
  /// var sprite = new Sprite(myTexture);
  /// sprite.SetPos(myVector);
  /// //same as
  /// sprite.x = 10;
  /// sprite.y = 10;
  /// </code>
  /// </example>
  /// <param name="position">The target position as a Vector2 object</param>
  public void SetPos(Vector2 position)
  {
    _position = position;
  }

  /// <summary>
  /// shorthand function to set the sprite scale
  /// calling the function without parameters will reset its scale
  /// </summary>
  /// <param name="x">The scale factor for the X axis</param>
  /// <param name="y">The scale factor for the Y axis</param>
  public void SetScale(float x = 0, float y = 0)
  {
    _scale.X = x;
    _scale.Y = y;
  }

  /// <summary>
  /// shorthand function to set the sprite scale
  /// calling the function without parameters will reset its scale
  /// </summary>
  /// <param name="scale"> the scale factor for both axis</param>
  public void SetScale(Vector2 scale)
  {
    _scale = scale;
  }

  /// <summary>
  /// shorthand function to set the sprite anchor.
  /// calling the function without parameters will reset its anchor to (0,0)
  /// </summary>
  /// <remarks>
  /// SetAnchor(0.5f,0.5f) will set the anchor to the middle of the texture
  /// </remarks>
  /// <param name="x"> the anchor x-coordinates clamped from 0 to 1</param>
  /// <param name="y">the anchor y-coordinates clamped from 0 to 1</param>
  public void SetAnchor(float x = 0, float y = 0)
  {
    _anchor.X = x;
    _anchor.Y = y;
  }

  /// <summary>
  /// shorthand function to set the sprite anchor.
  /// calling the function without parameters will reset its anchor to (0,0)
  /// </summary>
  /// <remarks>
  /// SetAnchor(0.5f,0.5f) will set the anchor to the middle of the texture
  /// </remarks>
  /// <param name="anchor"> the anchor Vector2 clamped from 0 to 1 respectively</param>
  public void SetAnchor(Vector2 anchor)
  {
    _anchor = anchor;
  }

  /// <summary>
  /// Shorthand function for mirroring the sprite.
  /// </summary>
  /// <param name="horizontal"> if true, will mirror the sprite horizontally</param>
  /// <param name="vertical"> if true, will mirror the sprite vertically</param>
  public void Flip(bool horizontal = false, bool vertical = false)
  {
    _mirror[0] = horizontal;
    _mirror[1] = vertical;
  }
  
  
  /// <summary>
  /// Draw the sprite on the screen.
  /// It's important to call this in between a spritebatch in the main class.
  /// </summary>
  /// <param name="spriteBatch"></param>
  /// <param name="gameTime"></param>
  public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    SpriteEffects effects = GetSpriteEffects();
    spriteBatch.Draw(
      texture: _texture,
      destinationRectangle: Rect,
      sourceRectangle: null,
      color: GetColor(),
      rotation: Rotation,
      origin: Origin,
      effects: effects,
      layerDepth: 0f
    );
  }


  /// <summary>
  /// helper function to calculate the opacity of 
  /// </summary>
  /// <returns></returns>
  protected Color GetColor()
  {
    // in this we are getting the color and the opacity value and multiply it;
    return _color * Opacity;
  }

  public GameComponent GameComponent { get; }
  public string Name { get; set; }

  /// <summary>
  /// Update the sprite movement and such.
  /// </summary>
  /// <param name="gameTime"> the game time </param>
  public virtual void Update(GameTime gameTime)
  {
  }

  /// <summary>
  /// dispose of the sprite and its texture.
  /// </summary>
  public virtual void Dispose()
  {
    _texture?.Dispose();
  }
  
  /// <summary>
  /// return the sprite effects.
  /// </summary>
  /// <returns> the sprite effects</returns>
  protected SpriteEffects GetSpriteEffects()
  {
    if (_mirror[0] && _mirror[1])
    {
      return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
    }

    if (_mirror[0])
    {
      return SpriteEffects.FlipHorizontally;
    }

    if (_mirror[1])
    {
      return SpriteEffects.FlipVertically;
    }
    return SpriteEffects.None;
  }

  #region accessor

  /// <summary>
  /// return whether the sprite is enabled or not. if It's not enabled, it won't be rendered.
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// the sprite texture
  /// </summary>
  public Texture2D Texture { get => _texture; set => _texture = value; }
  
  /// <summary>
  /// The sprite coordinates
  /// </summary>
  public Vector2 Position { get => _position; set => _position = value; }

  /// <summary>
  /// The sprite width scaling with the scale factor.
  /// </summary>
  public virtual int Width
  {
    get
    {
      var scaleX = Math.Abs(_scale.X);
      var result = scaleX * _texture.Width;
      return (int)result;
    }
  }

  /// <summary>
  /// The sprite height scaling with the scale factor.
  /// </summary>
  public virtual int Height
  {
    get
    {
      var scaleY = Math.Abs(_scale.Y);
      var result = scaleY * _texture.Height;
      return (int)result;
    }
  }
  
  /// <summary>
  /// the real width of the sprite without being affected by the scaling factor
  /// </summary>
  public int RealWidth => _texture.Width;
  
  /// <summary>
  /// the real height of the sprite without being affected by the scaling factor
  /// </summary>
  public int RealHeight => _texture.Height;
  
  /// <summary>
  /// the sprite scale factor
  /// </summary>
  public Vector2 Scale { get => _scale; set => _scale = value; }
  
  /// <summary>
  /// the sprite color
  /// </summary>
  public Color Color { get => _color; set => _color = value; }

  /// <summary>
  /// represent the rectangle of the sprite 
  /// </summary>
  public Rectangle Rect { get => new((int)_position.X, (int)_position.Y, Width, Height); }

  /// <summary>
  /// Represent the source rectangle frame.
  /// </summary>
  /// <remarks>
  /// Do take note that source rect is meant to be used as the frame rect when doing animated sprite.
  /// In the base sprite class, it's only added for convenience.
  /// </remarks>
  public virtual Rectangle SourceRect { get; protected set; }
  
  /// <summary>
  /// the sprite x-coordinates
  /// </summary>
  public float X { get => _position.X; set => _position.X = value; }
  
  /// <summary>
  /// the sprite y-coordinates
  /// </summary>
  public float Y {get => _position.Y; set => _position.Y = value; }

  /// <summary>
  /// the sprite anchor where the sprite will base its position and rotation from.
  /// </summary>
  /// <remarks>
  /// the value of the anchor is clamped from 0f to 1f and scales with the realWidth and RealHeight of the texture
  /// </remarks>
  public Vector2 Anchor
  {
    get => _anchor;
    set
    {
      var x = Math.Clamp(value.X, 0f,1f);
      var y = Math.Clamp(value.Y, 0f,1f);
      _anchor = new Vector2(x, y);
    }
  }
  
  /// <summary>
  /// if true, mirror the sprite horizontally
  /// </summary>
  public bool MirrorX { get => _mirror[0]; set => _mirror[0] = value; }
  
  /// <summary>
  /// if true, mirror the sprite vertically
  /// </summary>
  public bool MirrorY { get => _mirror[1]; set => _mirror[1] = value; }
  
  /// <summary>
  /// the sprite origin which is set via the realWidth/Height and anchor of the sprite
  /// </summary>
  protected virtual Vector2 Origin => new(RealWidth * _anchor.X, RealHeight * _anchor.Y);
  
  /// <summary>
  /// the sprite rotation in degree
  /// </summary>
  public float Rotation { get; set; }
  
  /// <summary>
  /// the sprite opacity from a range to 0 to 100 as percentile
  /// </summary>
  public int Opacity
  {
    get => (int)(_opacity * 100);
    set {
      var percent = (Math.Clamp(value, 0,100)) / 100f;
      
      _opacity = percent;
    }
  }

  #endregion
}
