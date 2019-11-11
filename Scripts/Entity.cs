 using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public abstract class Entity : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D rb;
  protected CircleCollider2D cc;

	[SerializeField]
	protected float speed = 10f;

	// POSITIONAL METHODS

	/// predicts where to move or shoot to hit a target
	protected Vector2 Predict(Vector2 pos, Vector2 rVel, float iSpeed) {
		Vector2 rPos = pos - rb.position;

		float a = iSpeed * iSpeed - rVel.sqrMagnitude;
		float b = -2f * Vector2.Dot(rVel, rPos);
		float c = -rPos.sqrMagnitude;
		float determinant = b * b - 4f * a * c;

		if (determinant > -Mathf.Epsilon) {
			float time1 = (-b + Mathf.Sqrt(determinant)) / (2f * a);
			float time2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);

			if (time1 > -Mathf.Epsilon && time2 > -Mathf.Epsilon) pos += rVel * Mathf.Min(time1, time2);
			else if (time1 > -Mathf.Epsilon) pos += rVel * time1;
			else if (time2 > -Mathf.Epsilon) pos += rVel * time2;
		}

		return pos;
	}
	protected Vector2 Predict(Rigidbody2D target) { // for ramming
		return Predict(target.position, target.velocity - rb.velocity, speed);
	}
	protected Vector2 Predict(Vector2 pos) { // for landing
		return Predict(pos, -rb.velocity, speed);
	}
	protected Vector2 Predict(Rigidbody2D target, float iSpeed) { // for shooting
		return Predict(target.position, target.velocity, iSpeed);
	}

	/// seeks out the closest target
	protected GameObject Seek(string tag) { // finds closest ramming position
		return GameObject.FindGameObjectsWithTag(tag)
			.OrderBy(i => (Predict(i.GetComponent<Rigidbody2D>()) - rb.position).sqrMagnitude)
			.FirstOrDefault();
	}
	protected GameObject Seek(string tag, float iSpeed) { // finds closest shooting position
		return GameObject.FindGameObjectsWithTag(tag)
			.OrderBy(i => (Predict(i.GetComponent<Rigidbody2D>(), iSpeed) - rb.position).sqrMagnitude)
			.FirstOrDefault();
	}

	/// returns a position dist away from target
	/// used to keep at range, push and flee
	protected Vector2 Kite(Vector2 pos, float dist) { // make dist negative for charging
		Vector2 rPos = pos - rb.position;
		return pos - rPos.normalized * dist;
	}


	// MOVEMENT METHODS

	/// moves to position
	/// reliant on linear drag and mass
	protected void Move(Vector2 pos) { // apply to ang vel too?
		Vector2 dir = Vector2.ClampMagnitude((pos - rb.position), speed);
		Vector2 acc = dir * Mathf.Max(speed - rb.velocity.magnitude, 0f);
		rb.AddForce(acc);
	}

	/// moves at a speed not proportional to pos
	protected void Rush(Vector2 pos) {
		// Move(Kite(pos, -speed));
		Vector2 acc = (pos - rb.position).normalized * speed * Mathf.Max(speed - rb.velocity.magnitude, 0f);
		rb.AddForce(acc);
	}

	// protected void Turn(Vector2 pos) {
	// 	Vector2 rPos = pos - rb.position;
	// 	if (rPos.sqrMagnitude > Mathf.Epsilon) {
	// 		rb.rotation = Mathf.LerpAngle(rb.rotation, Mathf.Atan2(rPos.y, rPos.x) * Mathf.Rad2Deg - 90f, turnSmooth);
	// 	}
	// }

	protected float GetRotation(Vector2 pos) {
		//float ang = Vector2.SignedAngle(Vector2.up, rb.position - pos) + 180;
		//return Quaternion.Euler(0f, 0f, ang);
		Vector2 dir = pos - rb.position;
		float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
		return ang;
	}

	/// arrives at position without going over
	protected void Grace(Vector2 pos) {
		// if in range, move backwards?
		pos -= rb.velocity;
		Move(pos);
	}

	//protected void Wander() {}

	protected virtual void Awake() {
    rb = GetComponent<Rigidbody2D>();
    cc = GetComponent<CircleCollider2D>();
	}

	protected virtual void OnDisable() {
	}
}
