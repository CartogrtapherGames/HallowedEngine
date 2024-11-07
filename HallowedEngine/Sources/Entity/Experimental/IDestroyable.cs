namespace Hallowed.Entity.Experimental;

public interface IDestroyable
{
  public void Destroy();
  public bool IsDestroyed { get; set; }
}
