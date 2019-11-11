using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour {

  private void OnTriggerEnter2D() {
    Loader.Instance.LoadLevel();
  }
}
