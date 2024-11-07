using System;
using System.Collections.Generic;
using Hallowed.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

/// <summary>
/// the class that handles a group of texture all bundled in one.
/// </summary>
public class TextureAtlas : IDisposable
{
  
  public Texture2D Texture { get; set; }
  public static ContentManager Content { private get; set; } = null;
  
  private Dictionary<string, Rectangle> _texturesDict = new();
  private Texture2D _texture;
  
  public TextureAtlas(Texture2D texture)
  {
    _texture = texture;
  }

  public TextureAtlas(TextureAtlasInfo atlasInfo)
  {
    _texture = Content.Load<Texture2D>(atlasInfo.Texture);
    _texturesDict = atlasInfo.TexturesDict;
  }

  public static TextureAtlas From(string data)
  {
    var result = DataLoader.LoadJson<TextureAtlasInfo>(data);
    return new TextureAtlas(result);
  }
  
  public Rectangle Get(string key)
  {
    if(!_texturesDict.TryGetValue(key, out Rectangle value))
      throw new Exception("no such entry for the texture");
    return value;
  }

  // to allow to manually set textureInfo
  public void Set(string key, Rectangle value)
  {
    _texturesDict[key] = value;
  }

  public void Dispose()
  {
    _texture?.Dispose();
  }
}

[Serializable]
public struct TextureAtlasInfo
{
  public string Texture;
  public Dictionary<string, Rectangle> TexturesDict;
}
