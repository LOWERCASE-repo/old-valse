using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Eruption : MonoBehaviour {

  [SerializeField]
  private int damage = 8;

  private void OnTriggerEnter2D(Collider2D col) {
    Player player = col.gameObject.GetComponent<Player>();
    if (player != null) {
      player.Health -= damage;
    }
  }
}
