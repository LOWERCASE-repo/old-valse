using UnityEngine;
using System.Collections;

public class Seeker : Entity {

  private Rigidbody2D target;
  private bool seeking;

  protected override void Awake() {
    base.Awake();
  }

  private void FixedUpdate() {
    if (seeking) {
      if (target == null || !target.gameObject.activeSelf) {
        target = Seek("Enemy").GetComponent<Rigidbody2D>(); // call only when something's spawned? and once in awake
        // or only seek for first second bc honestly it counters teleportation which is too much
      } else {
        Move(Predict(target));
      }
    }
    seeking = true;
  }

  private void OnCollisionEnter2D(Collision2D col) {
    if (col.gameObject.CompareTag("Enemy")) {
      col.gameObject.SetActive(false);
      gameObject.SetActive(false);
    }
  }
}
