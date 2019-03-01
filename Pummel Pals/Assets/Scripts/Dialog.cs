using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour {

	public TextMeshProUGUI textdisplay;
	public string[] sentences;
	private int index;
	private float typingSpeed = .02f;
	public SpriteRenderer LeftChar;
	public SpriteRenderer RightChar;
	private bool leftcharActive = true;
	private bool allowNext = false;
	public GameObject CButton;
	public GameObject SpaceButton;

	void Start() {
		StartCoroutine(TypeText());
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			typingSpeed = 0f;
		}

		if(textdisplay.text == sentences[index]) {
			allowNext = true;
		}

		if (Input.GetKeyDown(KeyCode.C) && index == sentences.Length - 1 && allowNext) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		if (Input.GetKeyDown(KeyCode.C) && allowNext && index != sentences.Length - 1) {
			typingSpeed = 0.02f;
			allowNext = false;
			leftcharActive = !leftcharActive;
			if (index < sentences.Length - 1) {
				index++;
				textdisplay.text = "";
				StartCoroutine(TypeText());
			} else {
				textdisplay.text = "";
			}
		}

		if (allowNext) {
			CButton.SetActive(true);
			SpaceButton.SetActive(false);
		} else {
			CButton.SetActive(false);
			SpaceButton.SetActive(true);
		}

		if (leftcharActive) {
			LeftChar.color = Color.white;
			RightChar.color = new Color(0.2f, 0.2f, 0.2f);
		} else {
			LeftChar.color = new Color(0.2f, 0.2f, 0.2f);
			RightChar.color = Color.white;
		}

	}

	IEnumerator TypeText() {
		foreach(char letter in sentences[index].ToCharArray()) {
			textdisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}

	}

}
