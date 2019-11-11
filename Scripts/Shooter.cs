using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooter : Enemy {

  [SerializeField]
  private float shotSpeed = 4f;
  [SerializeField]
  private float spread = 15f;
  private Vector2 pPos;

  protected override bool TriggerAttack() { // raycast
    return true;
  }

  protected override void Attack() {
    Pool.Spawn(Identity.Bullet, transform.position, GetRotation(pPos));
  }

  protected override void Awake() {
    base.Awake();
  }

  protected override void OnEnable() {
    base.OnEnable();
  }

  private void FixedUpdate() {
    pPos = Predict(Pool.player.rb, shotSpeed);
  }

  protected override void OnDisable() {
    base.OnDisable();
  }
}
