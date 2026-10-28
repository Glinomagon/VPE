using Godot;

namespace VPE.scripts;

public interface IMapView {
	void DisplayMapTexture(ImageTexture texture);
}

public partial class MapView : Node2D, IMapView
{
	private MapPresenter _mapPresenter;
	private ImageTexture _mapTexture;
	private Sprite2D _mapSprite;

	private void Init() {
		_mapPresenter = new MapPresenter(this);
		_mapSprite = new Sprite2D { Centered = false };
		AddChild(_mapSprite);
	}

	public void DisplayMapTexture(ImageTexture texture) {
		_mapSprite.Texture = texture;
	}

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready() {
		Init();
		// update sprite map texture
		_mapPresenter.UpdateMap();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
	}
}

