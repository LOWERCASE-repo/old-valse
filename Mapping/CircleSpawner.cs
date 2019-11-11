using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleSpawner : MonoBehaviour {

  private float triggerRange;
  private float spawnRange;
  private Identity[] spawns;

  private CircleCollider2D cc;

  private GameObject[] telegraphs;

  public void Construct(Ambush ambush) {
    this.triggerRange = ambush.triggerRange;
    this.spawnRange = ambush.spawnRange;
    this.spawns = ambush.spawns;
    this.telegraphs = new GameObject[spawns.Length];

    cc.radius = triggerRange;
    cc.enabled = true;
  }

  private IEnumerator TeleSpawn() {
    List<Vector2> spawned = new List<Vector2>();
    bool allSpawned = false;
    while (!allSpawned) {
      allSpawned = true;
      for (int i = 0; i < spawns.Length; ++i) {
        if (telegraphs[i] != null && !telegraphs[i].activeSelf) {
          Pool.Spawn(spawns[i], telegraphs[i].transform.position, transform);
          telegraphs[i].SetActive(false);
        } else {
          allSpawned = false;
        }
      }
      yield return new WaitForFixedUpdate();
    }
    while (transform.childCount > 0) {
      yield return new WaitForFixedUpdate();
    }
    Destroy(gameObject);
  }

  private void Awake() {
    cc = GetComponent<CircleCollider2D>();
  }

  private void OnTriggerEnter2D() {
    cc.enabled = false;
    float rAng = Random.value * 360f;
    float dAng = 360f / spawns.Length;
    Vector2 sPos;
    for (int i = 0; i < spawns.Length; ++i) {
      sPos = transform.position + Quaternion.AngleAxis(dAng * i + rAng, Vector3.forward) * Vector3.up * spawnRange;
      telegraphs[i] = Pool.Spawn(Identity.TeleSummon, sPos, transform);
    }
    StartCoroutine(TeleSpawn());
  }

  private void OnDisable() {
    for (int i = 0; i < transform.childCount; ++i) {
      Pool.Despawn(transform.GetChild(i).gameObject);
    }
  }
}
