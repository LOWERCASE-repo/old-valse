using UnityEngine;
using System.Collections;

public abstract class Enemy : Entity {

  protected enum State {Charging, Attacking, Recovering};
  protected State state;

  [SerializeField]
  protected float chargeTime = 1f;
  [SerializeField]
  protected float attackTime = 1f;
  [SerializeField]
  protected float recoverTime = 1f;

  [SerializeField]
  protected int damage = 5;
  protected Coroutine cycle;

  [SerializeField]
	protected int healthDrop;
  [SerializeField]
  protected AnimationCurve score;

  abstract protected bool TriggerAttack();
  abstract protected void Attack();

  public void Cull() {
    int cScore = (int)score.Evaluate(Random.value);
    int fives = (int)(Random.value * cScore / 5f);
    int ones = cScore - fives * 5;

    for (int i = 0; i < fives; ++i) {
      Pool.Spawn(Identity.HeavyPoint, rb.position);
    }
    for (int i = 0; i < ones; ++i) {
      Pool.Spawn(Identity.Point, rb.position);
    }
    
    Pool.Despawn(gameObject);
  }

  protected IEnumerator AttackCycle() {

    state = State.Charging;
    yield return new WaitForSeconds(chargeTime);
    while (!TriggerAttack()) {
      yield return new WaitForFixedUpdate();
    }

    state = State.Attacking;
    Attack();
    yield return new WaitForSeconds(attackTime);

    state = State.Recovering;
    yield return new WaitForSeconds(recoverTime);

    cycle = StartCoroutine(AttackCycle());
  }

  protected override void Awake() {
    base.Awake();
  }

  protected virtual void OnEnable() {
    cycle = StartCoroutine(AttackCycle());
  }

  protected override void OnDisable() {
    base.OnDisable();
    Pool.Spawn(Identity.Death, rb.position);
    StopCoroutine(cycle);
  }
}
