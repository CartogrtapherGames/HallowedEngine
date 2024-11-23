using System;
using System.Collections.Generic;
using Hallowed.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hallowed.Display;

/// <summary>
/// the class that handles a group of textures all bundled in one large atlas and
/// allow them to be easily picked via a dictionary.
/// </summary>
public class TextureAtlas : IDisposable
{
  
  /// <summary>
  /// the atlas texture
  /// </summary>
  public Texture2D Texture { get; set; }
  
  /// <summary>
  /// the content manager that is assigned for all the texture atlas.
  /// It has to be assigned or else it will cause an error.
  /// </summary>
  public static ContentManager Content { private get; set; } = null;
  
  private readonly Dictionary<string, Rectangle> _textureRegions = new();
  private readonly Texture2D _texture;
  
  /// <summary>
  /// the collections of texture regions grouping all the compiled textures.
  /// </summary>
  public Dictionary<string, Rectangle> TextureRegions => _textureRegions;
  
  /// <summary>
  /// create a new instance of the texture atlas without any assigned region.
  /// </summary>
  /// <param name="texture">the 2D texture to load </param>
  public TextureAtlas(Texture2D texture)
  {
    _texture = texture;
  }

  /// <summary>
  /// create a new instance of the texture atlas with all the required info such as the region and texture.
  /// </summary>
  /// <param name="atlasInfo">the atlas info to read </param>
  public TextureAtlas(TextureAtlasInfo atlasInfo)
  {
    _texture = Content.Load<Texture2D>(atlasInfo.Texture);
    _textureRegions = atlasInfo.TexturesDict;
  }

  /// <summary>
  /// create and load an atlas from a JSON string data and return a texture atlas instance.
  /// </summary>
  /// <param name="data">the JSON string to load</param>
  /// <returns>the atlas texture instance</returns>
  public static TextureAtlas From(string data)
  {
    var result = DataLoader.LoadJson<TextureAtlasInfo>(data);
    return new TextureAtlas(result);
  }
  
  /// <summary>
  /// return the source rectangle region for a specific texture located inside the texture atlas
  /// </summary>
  /// <param name="key">the region key</param>
  /// <returns>the source rectangle</returns>
  /// <exception cref="Exception"></exception>
  public Rectangle Get(string key)
  {
    if(!_textureRegions.TryGetValue(key, out Rectangle value))
      throw new Exception("no such entry for the texture");
    return value;
  }
  
  /// <summary>
  /// create a region in the texture atlas
  /// </summary>
  /// <param name="key">the region key</param>
  /// <param name="value">the source rectangle </param>
  public void Set(string key, Rectangle value)
  {
    _textureRegions[key] = value;
  }
  
  /// <summary>
  /// dispose the texture
  /// </summary>
  public void Dispose()
  {
    _texture?.Dispose();
  }
}

/// <summary>
/// the texture atlas JSON info
/// </summary>
[Serializable]
public struct TextureAtlasInfo
{
  public string Texture;
  public Dictionary<string, Rectangle> TexturesDict;
}
