using static VPE.scripts.helpers.Globals;
using System;
using System.Collections.Generic;
using Godot;
using static VPE.scripts.helpers.MapHelpers;
using System.Threading.Tasks;

namespace VPE.scripts;

public class MapModel {
  // Temporary until we get geolocation + dynamic map loading working
	private readonly (double Lat, double Lon) userPosition = (-36.853977065698814, 174.76731585123864);
  public int MapTileZoom { get; private set; } = 19;

  // ordered in draw order
	public IReadOnlyDictionary<TILES, Vector2I> TileOffset { get; } = new Dictionary<TILES, Vector2I>() {
		// Top
		{TILES.TOP_LEFT, 			new Vector2I(-2, -2)},
		{TILES.TOP_MID_LEFT, 	new Vector2I(-1, -2)},
		{TILES.TOP_CENTER, 		new Vector2I(0, -2)},
		{TILES.TOP_MID_RIGHT, new Vector2I(1, -2)},
		{TILES.TOP_RIGHT, 		new Vector2I(2, -2)},
		// Top mid
		{TILES.MID_TOP_LEFT, 			new Vector2I(-2, -1)},
		{TILES.MID_TOP_MID_LEFT, 	new Vector2I(-1, -1)},
		{TILES.MID_TOP_CENTER, 		new Vector2I(0, -1)},
		{TILES.MID_TOP_MID_RIGHT, new Vector2I(1, -1)},
		{TILES.MID_TOP_RIGHT, 		new Vector2I(2, -1)},
		// Mid
		{TILES.LEFT, 			new Vector2I(-2, 0)},
		{TILES.MID_LEFT, 	new Vector2I(-1, 0)},
		{TILES.CENTER, 		new Vector2I(0, 0)},
		{TILES.MID_RIGHT, new Vector2I(1, 0)},
		{TILES.RIGHT, 		new Vector2I(2, 0)},
		// Bot mid
		{TILES.MID_BOT_LEFT, 			new Vector2I(-2, 1)},
		{TILES.MID_BOT_MID_LEFT, 	new Vector2I(-1, 1)},
		{TILES.MID_BOT_CENTER, 		new Vector2I(0, 1)},
		{TILES.MID_BOT_MID_RIGHT, new Vector2I(1, 1)},
		{TILES.MID_BOT_RIGHT, 		new Vector2I(2, 1)},
		// Bot
		{TILES.BOT_LEFT, 			new Vector2I(-2, 2)},
		{TILES.BOT_MID_LEFT, 	new Vector2I(-1, 2)},
		{TILES.BOT_CENTER, 		new Vector2I(0, 2)},
		{TILES.BOT_MID_RIGHT, new Vector2I(1, 2)},
		{TILES.BOT_RIGHT, 		new Vector2I(2, 2)},
	};

	public Dictionary<TILES, string> GetMapTileUrl() {
    Dictionary<TILES, string> tilesUrl = [];
		var (centerX, centerY) = GetTileCoord(userPosition.Lat, userPosition.Lon, MapTileZoom);
		tilesUrl.Add(TILES.CENTER, GetOsmTileUrl(centerX, centerY, MapTileZoom));
 
		foreach (var (orientation, offset) in TileOffset) {
			if (orientation == TILES.CENTER) continue;

			int tileX = centerX + (int)offset[0];
			int tileY = centerY + (int)offset[1];
			tilesUrl.Add(orientation, GetOsmTileUrl(tileX, tileY, MapTileZoom));
		}
    return tilesUrl;
	}

  public async Task<Dictionary<TILES, Image>> FetchMapTileSet(Dictionary<TILES, string> tilesUrl) {
    Dictionary<TILES, Image> mapTileSet = [];

    Dictionary<string, string> headers = new() {
			{"User-Agent", USER_AGENT},
		};

		foreach(TILES orientation in Enum.GetValues<TILES>()) {
			byte[] imgData =  await ApiCaller.GetByteArrayAsync(tilesUrl[orientation], headers);
		  Image img = new();
			Error err = img.LoadPngFromBuffer(imgData);
			if (err == Error.Ok) {
				mapTileSet.Add(orientation, img);
			} else {
				mapTileSet.Add(orientation, null);
				GD.PrintErr($"Failed to load {orientation} image from byte array!");
			}
		}
    return mapTileSet;
  }
}