using System;
using System.Collections.Generic;
using Hallowed.Core;
using Hallowed.Display.Struct;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

public class AnimatedSprite : Sprite
{

  public event Action Completed;
  private readonly HashSet<Action> _handlers = [];
  
  private readonly Dictionary<string , Animations> _animations = new();
  private readonly Area2D _frameSize;
  private readonly int _framerate; // TODO : I dont really need that anymore since framerate is independant to each animations??
  private int _frame;
  private bool _isPlaying;
  private bool _isCompleted;
  private string _currentAnimation;
  private float _animationTimer;
  
  public sealed override Rectangle SourceRect { get; protected set; }

  public override int Width
  {
    get
    {
      var scaleX = Math.Abs(Scale.X);
      var result = scaleX * _frameSize.Width;
      return (int)result;;
    }
  }
  public override int Height
  {
    get
    {
      var scaleY = Math.Abs(Scale.Y);
      var result = scaleY * _frameSize.Height;
      return (int)result;
    }
  }
  protected override Vector2 Origin => new(SourceRect.Width * Anchor.X, SourceRect.Height * Anchor.Y);

  public AnimatedSprite(Spritesheet spritesheet, Point firstFrame)
  {
    Texture = spritesheet.Texture;
    _frameSize = spritesheet.FrameSize;
    int x = _frameSize.Width * firstFrame.X;
    int y = _frameSize.Height * firstFrame.Y;
    SourceRect = new Rectangle(x, y, _frameSize.Width, _frameSize.Height);
    
    _currentAnimation = "";
    _frame = 0;
    _isPlaying = false;
    _animationTimer = 0f;
  }
  
  public Area2D FrameSize => _frameSize;

  public void AddAnimation(string name, int row, int column, int frameCount, int frameRate = 8, bool loop = false)
  {
    if (_animations.ContainsKey(name))
    {
      throw new Exception("Animation already added");
    }

    var data = new Animations()
    {
      Row = row,
      Column = column,
      FrameCount = frameCount,
      Framerate = frameRate,
      Loop = loop
    };
    _animations.Add(name, data);
  }

  public void AddAnimation(string name, Animations animations)
  {
    if (_animations.ContainsKey(name))
    {
      throw new Exception("Animation already added");
    }
    _animations.Add(name, animations);
  }

  private void Reset()
  {
    _frame = 0;
    _currentAnimation = "";
    _isPlaying = false;
    _animationTimer = 0;
  }

  public AnimatedSprite Play(string name)
  {
    if (_currentAnimation == name) return this;
    if (!_animations.ContainsKey(name)) throw new Exception("Animation not found");
    Reset();
    _currentAnimation = name;
    _isPlaying = true;
    return this;
  }

  public AnimatedSprite OnCompleted(Action completedAction)
  {
    if (_handlers.Add(completedAction))
    {
      Completed += completedAction;
    }
    _isCompleted = true;
    Reset();
    return this;
  }

  public void Stop(bool reset = false)
  {
    if (reset)
    {
      _isCompleted = true;
      Completed?.Invoke();
    }
    _isPlaying = false;
  }

  public void Resume()
  {
    _isPlaying = true;
  }

  public bool IsPlaying()
  {
    return _isPlaying;
  }

  public bool IsCompleted()
  {
    return _isCompleted;
  }

  public override void Draw(SpriteBatch batch, GameTime gameTime)
  {
    SpriteEffects effects = GetSpriteEffects();
    batch.Draw(
      texture: Texture,
      destinationRectangle: Rect,
      sourceRectangle: SourceRect,
      color: Color,
      rotation: Rotation,
      origin: Origin,
      effects: effects,
      layerDepth: 0f
    );
  }

  public override void Dispose()
  {
    base.Dispose();
    // in this case were unsubscribing the clear the event
    foreach (var handle in _handlers)
    {
      Completed -= handle;
    }
    
    _handlers.Clear();
  }

  public override void Update(GameTime gameTime)
  {
    if(!IsPlaying()) return;
    var anim = Animation(_currentAnimation);
    var sequence = BuildFrameSequence(anim);
    if (anim.Loop)
    {
      ProcessLoopingAnimation(sequence,anim.Framerate,gameTime);
    }
    else
    {
      
    }
  }

  private void ProcessLoopingAnimation(Point[] sequence, int framerate,GameTime gameTime)
  {
    float frameTotalTime = 1f / framerate;
    _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    if(!(_animationTimer >= frameTotalTime)) return;
    
    _frame = (_frame + 1) % sequence.Length;
    SourceRect = new Rectangle(sequence[_frame].X,sequence[_frame].Y, _frameSize.Width, _frameSize.Height);
    _animationTimer = 0;
  }

  private void ProcessAnimation(Point[] sequence, int framerate, GameTime gameTime)
  {
    float frameTotalTime = 1f / framerate;
    _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    if(!(_animationTimer >= frameTotalTime)) return;
    _frame = (_frame + 1);
    if (_frame >= sequence.Length)
    {
      Completed?.Invoke();
      Reset();
    }
    
    SourceRect = new Rectangle(sequence[_frame].X,sequence[_frame].Y, _frameSize.Width, _frameSize.Height);
    _animationTimer = 0;
  }

  private Animations Animation(string name)
  {
    if (!_animations.ContainsKey(name)) throw new Exception("Animation not found");
    return _animations[name];
  }

  private Point[] BuildFrameSequence(Animations anim)
  {
    var sequence = new Point[anim.FrameCount];
    for (int i = 0; i < anim.FrameCount; i++)
    {
      int row = anim.Row * _frameSize.Height;
      int column = (anim.Column + i) * _frameSize.Width;
      sequence[i] = new Point(column, row);
    }
    return sequence;
  }
}
