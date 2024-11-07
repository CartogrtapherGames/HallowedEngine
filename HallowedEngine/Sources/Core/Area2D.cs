using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

#nullable disable
namespace Hallowed.Core;

/// <summary>
/// The data class that represent an area of an object without its X and Y coordinates
/// </summary>
[DataContract]
[DebuggerDisplay("{DebugDisplayString,nq}")]
public class Area2D: IEquatable<Area2D>
{

  private static readonly Area2D ZeroArea2D = new Area2D(0, 0);

  [DataMember]
  public int Width;
  [DataMember]
  public int Height;

  public static Area2D Zero => Area2D.ZeroArea2D;

  internal string DebugDisplayString => this.Width.ToString() + " " + this.Height.ToString();

  public Area2D(int width, int height)
  {
    this.Width = width;
    this.Height = height;
  }

  public Area2D(int value)
  {
    this.Width = value;
    this.Height = value;
  }

  public static Area2D operator +(Area2D value1, Area2D value2)
  {
    return new Area2D(value1.Width + value2.Width, value1.Height + value2.Height);
  }

  public static Area2D operator -(Area2D value1, Area2D value2)
  {
    return new Area2D(value1.Width - value2.Width, value1.Height - value2.Height);
  }

  public static Area2D operator *(Area2D value1, Area2D value2)
  {
    return new Area2D(value1.Width * value2.Width, value1.Height * value2.Height);
  }
  
  public static Area2D operator /(Area2D source, Area2D divisor)
  {
    return new Area2D(source.Width/ divisor.Width, source.Height / divisor.Height);
  }

  public static bool operator ==(Area2D a, Area2D b) => a.Equals(b);

  public static bool operator !=(Area2D a, Area2D b) => !a.Equals(b);

  public override bool Equals(object obj) => obj is Area2D other && this.Equals(other);

  public bool Equals(Area2D other) => this.Width == other.Width && this.Height == other.Height;

  public override int GetHashCode()
  {
    return (17 * 23 + this.Width.GetHashCode()) * 23 + this.Height.GetHashCode();
  }

  public override string ToString()
  {
    return "{Width:" + this.Width.ToString() + " Height:" + this.Height.ToString() + "}";
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Vector2 ToVector2() => new Vector2((float)this.Width, (float)this.Height);

  public void Deconstruct(out int width, out int height)
  {
    width = this.Width;
    height = this.Height;
  }
}