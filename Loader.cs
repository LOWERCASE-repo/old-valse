using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Loader : MonoBehaviour {

  public static Loader Instance;

  [SerializeField]
  private Map map;

  public void LoadLevel() {
    for (int i = 0; i < Pool.spawns.Count; ++i) {
      Pool.Despawn(Pool.spawns[i]);
    }
    Pool.spawns.Clear();
    MasterMap.Instance.Generate(map);
    Pool.player.rb.velocity = Vector2.zero;
    Pool.player.rb.position = Vector2.zero;
  }

  private void Awake() {
    Instance = this;
  }

  private void Start() {
    LoadLevel();
  }
}
