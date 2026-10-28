using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using static VPE.scripts.helpers.Globals;


namespace VPE.scripts;

public class MapPresenter(IMapView view) {
  private MapModel _mapModel = new();
  private IMapView _view = view;

  private async Task<ImageTexture> CreateMapTexture(Dictionary<TILES, Image> mapTileSet) {
    Image fullMapImage = Image.CreateEmpty(TOTAL_PIXELS, TOTAL_PIXELS, false, Image.Format.Rgba8);

    foreach (var (orientation, image) in mapTileSet) {
			if (image != null) {
				// Force tile image to match Rgba8
        image.Convert(Image.Format.Rgba8);
				// determine grid placement
				Vector2I offset = _mapModel.TileOffset[orientation];
				int col = offset.X + 2;
        int row = offset.Y + 2;

				Vector2I pos = new(col * TILE_SIZE, row * TILE_SIZE);
				// stitch image into the main mapImage 5x5 grid
				fullMapImage.BlitRect(image, new Rect2I(0, 0, TILE_SIZE, TILE_SIZE), pos);
      }
    }
    return ImageTexture.CreateFromImage(fullMapImage);
  }

  public async void UpdateMap() {
    Dictionary<TILES, string> tilesUrl = _mapModel.GetMapTileUrl();
    Dictionary<TILES, Image> mapTileSet = await _mapModel.FetchMapTileSet(tilesUrl);
    ImageTexture mapTexture = await CreateMapTexture(mapTileSet);
    _view.DisplayMapTexture(mapTexture);
  }
}