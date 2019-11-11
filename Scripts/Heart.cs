using UnityEngine;

public class Heart : Entity {

  [SerializeField]
  private int health = 3;
  [SerializeField]
  private float startForce = 10f;
  [SerializeField]
  private float homeDelay = 0.5f;
  private bool homing;

  private void EnableHoming() {
    homing = true;
    cc.enabled = true;
  }

  protected override void Awake() {
    base.Awake();
  }

  private void OnEnable() {
    rb.velocity = Random.insideUnitCircle * startForce;
    Invoke("EnableHoming", homeDelay);
  }

  private void FixedUpdate() {
    if (homing) {
      Move(Predict(Pool.player.rb));
    }
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      Pool.player.Health += health;
      Pool.Despawn(gameObject);
    }
  }

  protected override void OnDisable() {
    homing = false;
    cc.enabled = false;
  }
}
