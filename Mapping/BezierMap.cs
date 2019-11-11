using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BezierMap {

  public Bezier trunk;
  public Bezier[] branches;

  private float trunkLength;
  private int trunkPivotCount;

  private float branchLength;
  private int branchPivotCount;
  private int branchCount;

  public BezierMap(Map map) {
    this.trunkLength = map.trunkLength;
    this.trunkPivotCount = map.trunkPivotCount;
    this.branchLength = map.branchLength;
    this.branchCount = map.branchCount;
    this.branchPivotCount = map.branchPivotCount;
    CreateTrunk();
    CreateBranches();
  }

  private void CreateTrunk() {
    Vector2 end = Random.insideUnitCircle.normalized * trunkLength;
    trunk = CreatePath(Vector2.zero, end, trunkPivotCount);
  }

  private void CreateBranches() {
    List<int> possibleGrafts = new List<int>();
    for (int i = 1; i < trunk.pivots.Length - 1; ++i) {
      possibleGrafts.Add(i);
    }
    Vector2 bStart, bEnd, dir; // todo dododo
    int graftIndex;
    branches = new Bezier[branchCount];
    for (int i = 0; i < branchCount; ++i) {
      graftIndex = possibleGrafts[(int)(possibleGrafts.Count * Random.value)];
      possibleGrafts.Remove(graftIndex);

      bStart = trunk.Eval((float)graftIndex / (float)trunkPivotCount);
      dir = trunk.pivots[graftIndex] - bStart;
      bEnd = bStart + dir.normalized * branchLength;
      branches[i] = CreatePath(bStart, bEnd, branchPivotCount);
    }
  }

  private Bezier CreatePath(Vector2 start, Vector2 end, int pivotCount) {
    Vector2[] pivots = new Vector2[pivotCount];

    pivots[0] = start;
    pivots[pivotCount - 1] = end;
    Vector2 pathCenter = (start + end) / 2;
    float length = (end - start).magnitude;

    float nAng = Mathf.Atan2(end.y, -end.x) * Mathf.Rad2Deg - 90f;
    float oAng = Mathf.Atan2(end.y, end.x) * Mathf.Rad2Deg - 90f;

    for (int i = 1; i < pivotCount - 1; ++i) {
      pivots[i] = pathCenter + Random.insideUnitCircle * length / 2;
      pivots[i] = Quaternion.AngleAxis(nAng, Vector3.forward) * pivots[i];
    }

    pivots.OrderBy(i => i.y);

    for (int i = 1; i < pivotCount - 1; ++i) {
      pivots[i] = Quaternion.AngleAxis(oAng, Vector3.forward) * pivots[i];
    }

    return new Bezier(pivots);
  }
}
