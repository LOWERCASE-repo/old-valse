using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnMap : MonoBehaviour {

  [SerializeField]
  private CircleSpawner circleSpawner;
  private BezierMap bm;

  public void Populate(BezierMap bm, Map map) {
    // Debug.Log("saoehusnaoeh" + map.finish);
    // Debug.Log("aoeuaoeuaeo" + bm.trunk.pivots.Length);
    Instantiate(map.finish, bm.trunk.end, Quaternion.identity, transform);
    CircleSpawner cCircleSpawner;
    int rnd;
    foreach (Bezier bez in bm.branches) {
      cCircleSpawner = Instantiate(circleSpawner.gameObject, bez.start, Quaternion.identity, transform).GetComponent<CircleSpawner>();
      rnd = (int)Mathf.Clamp((map.ambushes.Length) * Random.value, 0f, map.ambushes.Length - 1f);
      cCircleSpawner.Construct(map.ambushes[rnd]);
      Instantiate(map.rewards[(int)((map.rewards.Length - 1) * Random.value)], bez.end, Quaternion.identity, transform);
    }
  }
}
