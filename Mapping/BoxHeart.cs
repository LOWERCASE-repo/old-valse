using UnityEngine;

public class BoxHeart : MonoBehaviour {

  private void OnTriggerEnter2D() {
    for (int i = 0; i < 3; ++i) {
      Pool.Spawn(Identity.Heart, transform.position);
    }
    Destroy(gameObject);
  }
}
