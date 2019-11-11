using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Identity { // enum projectiles, enemies, etc
  Puller, // womblegrum; sleepy
  Kiter, // gritterling;
  //Layer, //
  Chaser, // shrither; four legged scurrying bush
  Shooter, // tintom; clockwork arrow launcher
  Caster, //
  Bullet,
  Backfire,
  Seeker,
  Point,
  HeavyPoint,
  Burst,
  Pop,
  Grind,
  Death,
  Press,
  TeleCircle,
  TeleSummon,
  Eruption,
  Raise,
  Heart
}

public class Pool : MonoBehaviour { // needs mono for instantiate

  public static Player player;
  public static new PlayerCamera camera;
  public static List<GameObject> spawns = new List<GameObject>();
  private static Transform pool;

  [SerializeField]
  private static float poolSize;
  private static Queue<GameObject>[] pools;

  public static GameObject Spawn(Identity identity, Vector2 position, Quaternion rotation, Transform parent) {
    GameObject spawn = pools[(int)identity].Dequeue();
    spawn.transform.position = position;
    spawn.transform.rotation = rotation;
    spawn.transform.SetParent(parent);
    spawn.SetActive(true);
    pools[(int)identity].Enqueue(spawn);
    spawns.Add(spawn);
    return spawn;
  }
  public static GameObject Spawn(Identity identity, Vector2 position, float rotation, Transform parent) {
    return Spawn(identity, position, Quaternion.AngleAxis(rotation, Vector3.forward), parent);
  }
  public static GameObject Spawn(Identity identity, Vector2 position, float rotation) {
    return Spawn(identity, position, rotation, pool);
  }
  public static GameObject Spawn(Identity identity, Vector2 position, Quaternion rotation) {
    return Spawn(identity, position, rotation, pool);
  }
  public static GameObject Spawn(Identity identity, Vector2 position, Transform parent) {
    return Spawn(identity, position, Quaternion.identity, parent);
  }
  public static GameObject Spawn(Identity identity, Vector2 position) {
    return Spawn(identity, position, Quaternion.identity, pool);
  }

  public static void Despawn(GameObject spawn) {
    spawn.SetActive(false);
    spawn.transform.SetParent(pool);
    // spawns.Remove(spawn);
  }

  private static void LoadPools() {
    int[] identities = (int[])Enum.GetValues(typeof(Identity));
    pools = new Queue<GameObject>[identities.Length];
    GameObject spawn;
    foreach (Identity identity in identities) {
      pools[(int)identity] = new Queue<GameObject>();
      for (int j = 0; j < poolSize; ++j) {
        spawn = (GameObject)Resources.Load(identity.ToString(), typeof(GameObject));
        pools[(int)identity].Enqueue(Instantiate(spawn, pool));
      }
    }
  }

  private void Awake() {
    poolSize = 100;
    player = GameObject.FindWithTag("Player").GetComponent<Player>();
    camera = GameObject.FindWithTag("MainCamera").GetComponent<PlayerCamera>();
    pool = GameObject.FindWithTag("Pool").transform;
    LoadPools();
  }

  // private void FixedUpdate() {
  //   for (int i = spawns.Count - 1; i >= 0; --i) {
  //     if (!spawns[i].activeInHierarchy) {
  //       Despawn(spawns[i]);
  //     }
  //   }
  // }
}
