using System.Collections.Generic;
using Hallowed.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display.Struct;

public struct Spritesheet
{
  public Texture2D Texture;
  public Area2D FrameSize;
  public Dictionary<string, Animations> Animations;

  public Dictionary<string, Point> Frame;
}
