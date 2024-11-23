using Hallowed.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

public class Iconset : Sprite
{

  private int _index;
  public Iconset(Texture2D texture, int width, int height)
  {
    Texture = texture;
    IconSize = new Area2D(width, height);
    _index = 0;
  }
  
  public int Index { get => _index; set => _index = value; }
  public Area2D IconSize { get; private set; }

  public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
  {
    SourceRect = BuildRegion();
    SpriteEffects effects = SpriteEffects.None;
    spriteBatch.Draw(
      texture: Texture,
      destinationRectangle: Rect,
      sourceRectangle: SourceRect,
      color: Color,
      rotation:Rotation,
      origin: Origin,
      effects: effects,
      layerDepth: 0f
      );
  }

  private Rectangle BuildRegion()
  {
    int row = _index * IconSize.Height;
    int column = _index * IconSize.Width;
    return new Rectangle(column, row, IconSize.Height, IconSize.Width);
  }
}
