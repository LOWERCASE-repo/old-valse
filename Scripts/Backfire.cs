using UnityEngine;
using System.Collections;

public class Backfire : MonoBehaviour {

  [SerializeField]
  private float range;
  [SerializeField]
  private float time;

  private Rigidbody2D rb;

  private void Disable() { // used by invoke
    Pool.Despawn(gameObject);
  }

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
  }

  private void OnEnable() {
    Invoke("Disable", time);
    Pool.Spawn(Identity.Burst, transform.position, transform.rotation);
    rb.velocity = -transform.up * range / time;
    rb.rotation = 0f;
  }

  private void OnTriggerEnter2D(Collider2D col) {
    Enemy enemy = col.gameObject.GetComponent<Enemy>();
    if (enemy != null) {
      enemy.Cull();
    } else {
      Pool.Despawn(col.gameObject);
    }
  }
}
