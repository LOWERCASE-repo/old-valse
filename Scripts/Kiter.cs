using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kiter : Enemy {

  [SerializeField]
  private float shotSpeed = 4f;
  [SerializeField]
  private float kiteDist = 7f;
  private Vector2 pPos;

  protected override bool TriggerAttack() { // raycast
    return true;
  }

  protected override void Attack() {
    float ang = GetRotation(Pool.player.rb.position);
    Pool.Spawn(Identity.Bullet, rb.position, ang);
    Pool.Spawn(Identity.Bullet, rb.position, ang + 15f);
    Pool.Spawn(Identity.Bullet, rb.position, ang - 15f);
  }

  protected override void Awake() {
    base.Awake();
  }

  protected override void OnEnable() {
    base.OnEnable();
  }

  private void FixedUpdate() {
    pPos = Predict(Pool.player.rb, shotSpeed);

    if (state == State.Attacking) {
      rb.velocity = Vector2.zero;
    } else if (state == State.Charging) {
      Move(Kite(pPos, kiteDist));
    } else {
      Grace(rb.position);
    }
  }

  protected override void OnDisable() {
    base.OnDisable();
  }
}
