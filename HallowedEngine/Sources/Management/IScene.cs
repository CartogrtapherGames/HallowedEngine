using System;
using Microsoft.Xna.Framework;

namespace Hallowed.Management;

public interface IScene
{
  public void Initialize();
  public void LoadContent();
  public void Update(GameTime gameTime);
  public void Draw(GameTime gameTime);

  public void UnloadContent();
  public void Pause();
  public void Resume();
}
