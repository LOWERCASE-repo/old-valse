
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System; // only for datetime

public class GridMap : MonoBehaviour {

  private int trunkWidth;
  private int branchWidth;

  private int wallWidth;
  private int precision;

  private int graftSize;
  private int leafSize;
  private int rootSize;
  private int crownSize;

  private TileBase floorTile;
  private TileBase[] wallTiles;
  private TileBase[] shadowTiles;

  private BezierMap bm;

  private HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
  private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
  private enum Contour { Full, NE, SE, SW, NW }
  private Dictionary<Vector2Int, Contour> wallContours = new Dictionary<Vector2Int, Contour>();

  private Tilemap floor;
  private Tilemap walls;
  private Tilemap decor;

  private void BrushPos(Vector2 pos, HashSet<Vector2Int> tiles, double width, bool brush) {
    Vector2[] cTiles = new Vector2[4];
    Vector2Int tTile;
    float yCap;
    for (int x = 0; x < width; x++) {
      yCap = Mathf.Sqrt((float)(width * width - x * x));
      for (int y = 0; y < yCap; y++) {
        cTiles[0] = new Vector2(x, y);
        cTiles[1] = new Vector2(-x, y);
        cTiles[2] = new Vector2(x, -y);
        cTiles[3] = new Vector2(-x, -y);
        foreach (Vector2 cTile in cTiles) {
          tTile = Vector2Int.RoundToInt(pos + cTile);
          if (brush) tiles.Add(tTile);
          else tiles.Remove(tTile);
        }
      }
    }
  }

  private void BrushBezier(Bezier bez, HashSet<Vector2Int> tiles, double width, bool brush) {
    Vector2 bPos;
    width += 0.5;
    float presInc = 1f / precision;
    for (int i = 0; i <= precision; ++i) { // not += presInc bc float arith
      bPos = bez.Eval(i * presInc);
      BrushPos(bPos, tiles, width, brush);
    }
  }

  // note to future me: before you try to write autocellpositions again, remember that hashmaps only store their local values, not adjacencies

  private Dictionary<Vector2Int, Contour> ContourPositions(HashSet<Vector2Int> tilePositions) {

    Dictionary<Vector2Int, Contour> contours = new Dictionary<Vector2Int, Contour>();
    bool north, east, south, west;
    int adjCount;

    foreach (Vector2Int tilePos in tilePositions) {

      adjCount = 0;
      north = tilePositions.Contains(new Vector2Int(tilePos.x, tilePos.y + 1));
      east = tilePositions.Contains(new Vector2Int(tilePos.x + 1, tilePos.y));
      south = tilePositions.Contains(new Vector2Int(tilePos.x, tilePos.y - 1));
      west = tilePositions.Contains(new Vector2Int(tilePos.x - 1, tilePos.y));

      if (north) ++adjCount;
      if (east) ++adjCount;
      if (south) ++adjCount;
      if (west) ++adjCount;

      // if (adjCount <= 1 || (north && south && !east && !west) || (east && west && !north && !south)) {
      //   contours.Remove(tilePos);
      // } else
      if (adjCount >= 3) {
        contours.Add(tilePos, Contour.Full);
      } else if (north && east) {
        contours.Add(tilePos, Contour.SW);
      } else if (east && south) {
        contours.Add(tilePos, Contour.NW);
      } else if (south && west) {
        contours.Add(tilePos, Contour.NE);
      } else if (west && north) {
        contours.Add(tilePos, Contour.SE);
      }
    }

    return contours;
  }

  private void MapContours(Tilemap tm, Dictionary<Vector2Int, Contour> contours, TileBase[] tiles) {
    foreach (KeyValuePair<Vector2Int, Contour> contour in contours) {
      tm.SetTile((Vector3Int)contour.Key, tiles[(int)contour.Value]);
    }
  }

  private void MapPositions(Tilemap tm, HashSet<Vector2Int> positions, TileBase tile) {
    foreach (Vector2Int pos in positions) {
      tm.SetTile((Vector3Int)pos, tile);
    }
  }

  private void MapShadows(Tilemap tm, Dictionary<Vector2Int, Contour> contours, TileBase[] tiles) {
    TileBase tile;
    foreach (KeyValuePair<Vector2Int, Contour> contour in contours) {
      tile = null;
      switch (contour.Value) {
        case Contour.SE:
        tile = tiles[0];
        break;
        case Contour.SW:
        tile = tiles[2];
        break;
        case Contour.Full:
        if (!contours.ContainsKey(contour.Key - Vector2Int.up)) {
          tile = tiles[1];
        }
        break;
      }
      if (tile != null) {
        tm.SetTile((Vector3Int)(contour.Key - Vector2Int.up), tile);
      }
    }
  }

  private void VisualizeBezier(Bezier b, Color c, Color d) {
    Vector2 oldPos = b.Eval(0);
    for (int i = 1; i <= 100; ++i) {
      float time = i / 100f;
      Debug.DrawLine(oldPos, b.Eval(time), c);
      oldPos = b.Eval(time);
    }
  }

  public void Generate(BezierMap bm, Map map) {

    // references

    this.bm = bm;
    this.trunkWidth = map.trunkWidth;
    this.branchWidth = map.branchWidth;
    this.wallWidth = map.wallWidth;
    this.precision = map.precision;

    this.floorTile = map.floorTile;
    this.wallTiles = map.wallTiles;
    this.shadowTiles = map.shadowTiles;

    this.graftSize = map.graftSize;
    this.leafSize = map.leafSize;
    this.rootSize = map.rootSize;
    this.crownSize = map.crownSize;

    wallPositions.Clear();
    floorPositions.Clear();
    wallContours.Clear();

    walls.ClearAllTiles();
    floor.ClearAllTiles();
    decor.ClearAllTiles();

    // shaping

    BrushBezier(bm.trunk, wallPositions, trunkWidth + wallWidth, true);
    BrushBezier(bm.trunk, floorPositions, trunkWidth + wallWidth / 2, true);

    BrushPos(bm.trunk.start, wallPositions, rootSize + wallWidth, true);
    BrushPos(bm.trunk.start, floorPositions, rootSize + wallWidth / 2, true);

    BrushPos(bm.trunk.end, wallPositions, crownSize + wallWidth, true);
    BrushPos(bm.trunk.end, floorPositions, crownSize + wallWidth / 2, true);

    foreach (Bezier bez in bm.branches) {
      BrushBezier(bez, wallPositions, branchWidth + wallWidth, true);
      BrushBezier(bez, floorPositions, branchWidth + wallWidth / 2, true);

      BrushPos(bez.start, wallPositions, graftSize + wallWidth, true);
      BrushPos(bez.start, floorPositions, graftSize + wallWidth / 2, true);

      BrushPos(bez.end, wallPositions, leafSize + wallWidth, true);
      BrushPos(bez.end, floorPositions, leafSize + wallWidth / 2, true);
    }

    // paving

    BrushBezier(bm.trunk, wallPositions, trunkWidth, false);
    BrushPos(bm.trunk.start, wallPositions, rootSize, false);
    BrushPos(bm.trunk.end, wallPositions, crownSize, false);

    foreach (Bezier bez in bm.branches) {
      BrushBezier(bez, wallPositions, branchWidth, false);
      BrushPos(bez.start, wallPositions, graftSize, false);
      BrushPos(bez.end, wallPositions, leafSize, false);
    }

    wallContours = ContourPositions(wallPositions);

    MapContours(walls, wallContours, wallTiles);
    MapPositions(floor, floorPositions, floorTile);
    MapShadows(decor, wallContours, shadowTiles);
  }

  private void Awake() {
    walls = transform.GetChild(0).GetComponent<Tilemap>();
    floor = transform.GetChild(1).GetComponent<Tilemap>();
    decor = transform.GetChild(2).GetComponent<Tilemap>();
  }

  private void Update() {
    VisualizeBezier(bm.trunk, Color.blue, Color.cyan);
    foreach (Bezier b in bm.branches) {
      VisualizeBezier(b, Color.red, Color.yellow);
    }
  }
}
