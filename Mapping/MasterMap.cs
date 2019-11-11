using UnityEngine;

public class MasterMap : MonoBehaviour {

  private BezierMap bm;
  private GridMap gm;
  private SpawnMap sm;

  public static MasterMap Instance;

  public void Generate(Map map) {
    bm = new BezierMap(map);
    gm.Generate(bm, map);
    sm.Populate(bm, map);
  }

  private void Awake() {
    gm = transform.GetChild(0).GetComponent<GridMap>();
    sm = transform.GetChild(1).GetComponent<SpawnMap>();
    Instance = this;
  }
}
