using Microsoft.Xna.Framework;

namespace Hallowed.Management;

public class GameProcess : Game
{

  protected override void Initialize()
  {
    base.Initialize();
    SceneManager.Initialize();
  }

  protected override void LoadContent()
  {
    base.LoadContent();
    SceneManager.Load();
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    SceneManager.Dispose();
  }

  protected override void Draw(GameTime gameTime)
  {
    SceneManager.Draw(gameTime);
  }
}
