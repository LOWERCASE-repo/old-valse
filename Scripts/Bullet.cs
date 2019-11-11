using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

  [SerializeField]
  private float speed = 3f;
  [SerializeField]
  private int damage = 5;

  private Rigidbody2D rb;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
  }

  private void OnEnable() {
    rb.velocity = transform.up * speed;
    rb.rotation = 0f;
  }

  private void FixedUpdate() {
    rb.angularVelocity = rb.velocity.sqrMagnitude + speed;
  }

  private void OnCollisionEnter2D(Collision2D col) {

    Vector2 cPos = col.GetContact(0).point;
    Vector2 rPos = cPos - rb.position;
    float ang = Mathf.Atan2(rPos.y, rPos.x) * Mathf.Rad2Deg - 90f;
    Pool.Spawn(Identity.Pop, new Vector2(cPos.x, cPos.y + 1), ang);
    
    Player player = col.gameObject.GetComponent<Player>();
    if (player != null) {
      player.Health -= damage;
      Pool.camera.Jolt(rPos);
    }
    Pool.Despawn(gameObject);
  }
}
