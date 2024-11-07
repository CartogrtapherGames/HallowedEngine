using Hallowed.Core;
using Microsoft.Xna.Framework;

namespace Hallowed.Display;

/// <summary>
/// the static class that manage the screen size and full screen toggling. 
/// </summary>
public static class Graphics
{

  private static GraphicsDeviceManager _graphics;
  private static Area2D _screenSize;
  private static bool _fullScreen;

  public static void Init(SceneBase instance, int width = 1920, int height = 1080, bool fullScreen = false)
  {
    _screenSize = new Area2D(width, height);
    _fullScreen = fullScreen; 
    _graphics = new GraphicsDeviceManager(instance);
    OnScreenRefresh();
  }

  public static void Init(SceneBase instance, GraphicsSystemOption option)
  {
    _screenSize = new Area2D(option.Width, option.Height);
    _fullScreen = option.FullScreen;
    _graphics = new GraphicsDeviceManager(instance);
    OnScreenRefresh();
  }

  public static void ToggleFullScreen()
  {
    _fullScreen = !_fullScreen;
    OnScreenRefresh();
  }
  
  private static void OnScreenRefresh()
  {
    _graphics.IsFullScreen = _fullScreen;
    _graphics.PreferredBackBufferWidth = _screenSize.Width;
    _graphics.PreferredBackBufferHeight = _screenSize.Height;
    _graphics.ApplyChanges();
  }
  
  public static GraphicsDeviceManager GdManager => _graphics;

  public static Area2D ScreenSize
  {
    get => _screenSize;
    set
    {
      _screenSize = value;
      OnScreenRefresh();
    }
  }
  
  public static int ScreenWidth {
    get => _screenSize.Width;
    set
    {
      _screenSize.Width = value; 
      OnScreenRefresh();
    }
  }

  public static int ScreenHeight
  {
    get => _screenSize.Height;
    set
    {
      _screenSize.Height = value;
      OnScreenRefresh();
    }
  }
}

public struct GraphicsSystemOption
{
  public bool FullScreen;
  public int Width;
  public int Height;
  public string WindowTitle;
}
