using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour {

	[SerializeField]
	private Player player;
	private Text text;

	protected virtual void Start() {
		text = GetComponent<Text>();
	}

	protected virtual void Update() {
		text.text = "score: " + ((int)player.score).ToString();
	}
}
