using Godot;

namespace VPE.scripts.helpers;

public partial class Globals : Node {
  public const int MAP_ZOOM = 19;
  public const int TILE_SIZE = 256;
	public const int GRID_SIZE = 5;
	public const int TOTAL_PIXELS = TILE_SIZE * GRID_SIZE;
  public const string USER_AGENT = ""; // Add user agent here

  // DRAW ORDER IS HARD CODED
  public enum TILES {    
    // Top row
    TOP_LEFT, TOP_MID_LEFT, TOP_CENTER, TOP_MID_RIGHT, TOP_RIGHT,
    // Top mid row
    MID_TOP_LEFT, MID_TOP_MID_LEFT, MID_TOP_CENTER, MID_TOP_MID_RIGHT, MID_TOP_RIGHT,
    // Mid row
    LEFT, MID_LEFT, CENTER, MID_RIGHT, RIGHT,
    // Bot mid row
    MID_BOT_LEFT, MID_BOT_MID_LEFT, MID_BOT_CENTER, MID_BOT_MID_RIGHT, MID_BOT_RIGHT,
    // Bot row
    BOT_LEFT, BOT_MID_LEFT, BOT_CENTER, BOT_MID_RIGHT, BOT_RIGHT,
  };
}