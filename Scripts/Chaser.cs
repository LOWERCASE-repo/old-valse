using UnityEngine;
using System.Collections;

public class Chaser : Enemy {

  [SerializeField]
  private float range;
  [SerializeField]
  private AnimationCurve attack;

  private float oSpeed;
  private float aSpeed;
  private Vector2 pPos;

  private IEnumerator CoruAttack() {
    float dTime = 0f;
    while (dTime <= attackTime) {
      aSpeed = attack.Evaluate(dTime / attackTime) * oSpeed;
      yield return new WaitForFixedUpdate();
      dTime += Time.fixedDeltaTime;
    }
  }

  protected override bool TriggerAttack() {
    return ((pPos - rb.position).sqrMagnitude < range) ? true : false;
  }

  protected override void Attack() {
    StartCoroutine(CoruAttack());
  }

  protected override void Awake() {
    base.Awake();
    range *= range;
    oSpeed = speed;
  }

  protected override void OnEnable() {
    base.OnEnable();
  }

  private void FixedUpdate() {
    speed = oSpeed + aSpeed;
    pPos = Predict(Pool.player.rb);
    if (state == State.Attacking) {
      Rush(Pool.player.rb.position);
    } else if (state == State.Charging) {
      Rush(pPos);
    } else {
      Grace(rb.position);
    }
  }

  protected void OnCollisionEnter2D(Collision2D col) {
    if ((state == State.Attacking || state == State.Charging) && col.gameObject.CompareTag("Player")) {
      Vector2 cPos = col.GetContact(0).point;
      Vector2 rPos = cPos - rb.position;
      float ang = Mathf.Atan2(rPos.y, rPos.x) * Mathf.Rad2Deg - 90f;
      Pool.Spawn(Identity.Grind, cPos + Vector2.up, ang);
      Pool.player.Health -= damage;
    }
  }

  protected override void OnDisable() {
    base.OnDisable();
  }
}
