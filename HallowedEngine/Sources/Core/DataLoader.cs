using System;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Hallowed.Core;

/// <summary>
/// the static class that allow loading and serializing files into multiple format such as JSON
/// </summary>
public static class DataLoader
{
  public static string RootDirectory = string.Empty;

  public static T LoadJson<T>(string fileName)
  {
    using var stream = TitleContainer.OpenStream(RootDirectory + "/" + fileName);
    using var reader = new StreamReader(stream);
    var jsonString = reader.ReadToEnd();
    var result = JsonConvert.DeserializeObject<T>(jsonString);
    return result;
  }

  // todo: implement it
  public static void SaveToJson<T>(T data, string fileName)
  {
  }
}
