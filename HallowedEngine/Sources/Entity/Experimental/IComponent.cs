using Hallowed.Display;
using Microsoft.Xna.Framework;

namespace Hallowed.Entity.Experimental;

/// <summary>
/// the interface that implements the necessary methods to allow a class to be used as a component
/// </summary>
public interface IComponent
{
  public string Name { get; set; }
  public bool Enabled { get; set; }
  public void Update(GameTime gameTime);
}
