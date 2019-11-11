using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Map")]
public class Map : ScriptableObject {

  [Header("Paths")]
  public int wallWidth = 3;
  public int precision = 100;

  [Header("Trunk")]
  public float trunkLength = 80f;
  public int trunkPivotCount = 8;
  public int trunkWidth = 2;

  [Header("Branches")]
  public float branchLength = 20f;
  public int branchPivotCount = 3;
  public int branchWidth = 1;
  public int branchCount = 4;

  [Header("Rooms")]
  public int rootSize = 2;
  public int crownSize = 2;
  public int graftSize = 3;
  public int leafSize = 3;

  [Header("Tiles")]
  public TileBase floorTile;
  public TileBase[] wallTiles = new TileBase[5];
  public TileBase[] shadowTiles = new TileBase[3];

  [Header("Spawns")]
  public Ambush[] ambushes;
  public GameObject finish;
  public GameObject[] rewards;



  // [Header("Rewards")]
  // point mines
  // health drops
  // temp artifacts
  // merchants are on seperate floors

}
