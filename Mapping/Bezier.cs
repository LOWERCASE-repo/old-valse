using UnityEngine;
using System;
using System.Collections;

public class Bezier {

  public Vector2[] pivots;
  public Vector2 start;
  public Vector2 end;

  public Bezier(Vector2[] pivots) {
    this.pivots = pivots;
    start = pivots[0];
    end = pivots[pivots.Length - 1];
    // length = (pivots[pivots.Length - 1] - start).magnitude;
  }

  public Vector2 Eval(double time) {
    return EvalRec(time, pivots);
  }

  private Vector2 EvalRec(double time, Vector2[] pivots) {
    if (pivots.Length == 1) {
      return pivots[0];
    }

    Vector2[] cPivots = new Vector2[pivots.Length - 1];
    Array.Copy(pivots, 0, cPivots, 0, pivots.Length - 1);
    Vector2 start = EvalRec(time, cPivots);
    Array.Copy(pivots, 1, cPivots, 0, pivots.Length - 1);
    Vector3 end = EvalRec(time, cPivots);

    return Vector2.Lerp(start, end, (float)time);
  }
}
