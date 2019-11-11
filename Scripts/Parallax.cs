using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

  private Vector3 oPos;

  [SerializeField]
  [Range (0f, 1f)]
  private float scrollSpeed = 0.2f;

  private void Start() {
    oPos = Pool.camera.transform.position;
  }

  private void Update() {
    transform.position += (Pool.camera.transform.position - oPos) * scrollSpeed;
    oPos = Pool.camera.transform.position;
  }
}
