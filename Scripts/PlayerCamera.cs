using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

  [HideInInspector]
  public Vector2 mPos;
  private Camera cam;

  private float oZoom; // original zoom
  private float pZoom; // zoom added from pulsing
  [SerializeField]
  private float mZoom; // multiplier for zoom added from moving

  private Vector2 jPos;

  [SerializeField]
  private float pTime = 0.5f;
  [SerializeField]
  private AnimationCurve pulse;

  [SerializeField]
  private float jTime = 0.3f;
  [SerializeField]
  private AnimationCurve jolt;

  public void Pulse(float mag) {
    StartCoroutine(CoruPulse(mag));
  }

  private IEnumerator CoruPulse(float mag) {
    float dTime = 0f;
    while (dTime <= pTime) {
      pZoom = pulse.Evaluate(dTime / pTime) * mag;
      yield return new WaitForFixedUpdate();
      dTime += Time.fixedDeltaTime;
    }
    pZoom = 0f;
  }

  public void Jolt(Vector2 dir) {
    StartCoroutine(CoruJolt(dir));
  }

  private IEnumerator CoruJolt(Vector2 dir) {
    float dTime = 0f;
    while (dTime <= jTime) {
      jPos = jolt.Evaluate(dTime / jTime) * dir.normalized;
      yield return new WaitForFixedUpdate();
      dTime += Time.fixedDeltaTime;
    }
    jPos = Vector2.zero;
  }

  public void Slow() {

  }

  private void Awake() {
    cam = GetComponent<Camera>();
    oZoom = cam.orthographicSize;
  }

  private void FixedUpdate() {
    mPos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
    transform.position = (Vector3)jPos + Pool.player.transform.position + new Vector3(0f, 0f, -10f);
    cam.orthographicSize = oZoom + pZoom + mZoom * Pool.player.rb.velocity.magnitude;
  }
}
