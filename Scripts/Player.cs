using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(ParticleSystem))]
public class Player : Entity {

  [SerializeField]
  private float pulseMag = 1.5f;
  [SerializeField]
  private float pulseCharge = 0.5f;
  [SerializeField]
  private float pulseZoom = 0.8f;

  [SerializeField]
  public int maxHealth = 20;
  private int health;
  public int Health {
    get { return health; }
		set {
			health = value;
      if (health > maxHealth) {
        score += health - maxHealth;
      }
      health = Mathf.Clamp(health, 0, maxHealth);
			if(health == 0) Debug.Log("player dead");
		}
  }
  public int score = 0;


  private IEnumerator Backfire() {

    yield return new WaitForSeconds(pulseCharge);
    while (!Input.GetButton("Backfire")) {
      yield return new WaitForFixedUpdate();
    }
    Vector2 dir = (Pool.camera.mPos - (rb.position + Vector2.up)).normalized;
    rb.velocity = dir * speed * pulseMag;
    Pool.Spawn(Identity.Backfire, rb.position, GetRotation(dir + rb.position));
    Pool.camera.Pulse(pulseZoom);

    StartCoroutine(Backfire());
  }

  protected override void Awake() {
    base.Awake();
    health = maxHealth;
  }

  private void OnEnable() {
    StartCoroutine(Backfire());
  }

  private void FixedUpdate() {
    Vector2 moveDir = Vector2.zero;

    if (Input.GetButton("MoveToCursor")) {
      moveDir = Pool.camera.mPos + Vector2.down;
    } else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
      moveDir.x = Input.GetAxisRaw("Horizontal");
      moveDir.y = Input.GetAxisRaw("Vertical");
      moveDir += rb.position;
    }

    if (!moveDir.Equals(Vector2.zero)) {
      Rush(moveDir);
    }
  }
}
