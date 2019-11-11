using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Caster : Enemy {

  [SerializeField]
  private float kiteDist = 6f;
  [SerializeField]
  private float spread = 1f;
  private GameObject telegraph;

  private IEnumerator TeleAttack() {
    while (telegraph.activeSelf) {
      yield return new WaitForFixedUpdate();
    }
    Pool.Spawn(Identity.Eruption, telegraph.transform.position);
  }

  protected override bool TriggerAttack() { // far from player
    return true;
  }

  protected override void Attack() {
    rb.velocity = Vector2.zero;
    telegraph = Pool.Spawn(Identity.TeleCircle, Pool.player.rb.position + spread * Random.insideUnitCircle);
    StartCoroutine(TeleAttack());
  }

  protected override void Awake() {
    base.Awake();
  }

  protected override void OnEnable() {
    base.OnEnable();
  }

  private void FixedUpdate() {
    if (state == State.Charging) {
      Move(Kite(Pool.player.rb.position, kiteDist));
    } else {
      Grace(rb.position);
    }
  }

  protected override void OnDisable() {
    base.OnDisable();
  }
}
