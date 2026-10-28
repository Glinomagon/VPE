using System;
using Godot;

namespace VPE.scripts.helpers;

// USE (LAT, LON) FOR COORDINATES. THESE FUNCTIONS ASSUME THEY ARE WRITTEN THAT WAY.
public partial class MapHelpers : Node {
  public static (int x, int y) GetTileCoord(double lat, double lon, int zoom) {
    double n = Math.Pow(2.0, zoom); // zoom factor

    int x = (int)Math.Floor((lon + 180.0) / 360.0 * n);

    /*  Mercator thingy for latitude lines.
        I don't truly understand why it works, but it works. */
    double latRad = lat * Math.PI / 180.0;
    int y = (int)Math.Floor((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * n);

    // handle edge cases
    if (x < 0) x = 0;
    if (x >= n) x = (int) n - 1;
    if (y < 0) y = 0;
    if (y >= n) y = (int) n - 1;

    return (x, y);
  }

  public static string GetOsmTileUrl(int x, int y, int zoom) {
    return $"https://tile.openstreetmap.org/{zoom}/{x}/{y}.png";
  }

  public static (double north, double south, double east, double west) GetOsmTileBounds(int tileX, int tileY, int zoom) {
    double n = Math.Pow(2.0, zoom);

    // Convert tile X to West and East edges
    double west = (tileX / n) * 360.0 - 180.0;
    double east = ((tileX + 1) / n) * 360.0 - 180.0;

    // Convert tile Y to  North and South edges (inverse of Mercator thingy apparently)
    double nTop = Math.PI - (2.0 * Math.PI * tileY) / n;
    double north = Math.Atan(Math.Sinh(nTop)) * (180.0 / Math.PI);

    double nBottom = Math.PI - (2.0 * Math.PI * (tileY + 1)) / n;
    double south = Math.Atan(Math.Sinh(nBottom)) * (180.0 / Math.PI);

    return (north, south, east, west);
  }
}