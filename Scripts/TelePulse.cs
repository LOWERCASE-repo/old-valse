using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePulse : MonoBehaviour {
  private void FixedUpdate() {
    Vector2 dir = (Vector2)transform.position - Pool.camera.mPos;
    float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
    transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
  }
}
