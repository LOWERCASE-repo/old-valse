using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthText : MonoBehaviour {

	[SerializeField]
	private Player player;
	private Text text;

	protected virtual void Start() {
		text = GetComponent<Text>();
	}

	protected virtual void Update() {
		text.text = "health: " + ((int)player.Health).ToString();
	}
}
