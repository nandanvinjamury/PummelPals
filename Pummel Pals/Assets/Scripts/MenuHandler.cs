using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public EventTrigger[] et;
    private GameManager gm;
    public string whereToGo;
    public GameObject exitPanel;
    public GameObject startPanel;

	public void Awake() {
		gm = GameObject.FindObjectOfType<GameManager>();
        whereToGo = "";
	}

	public void NextLevel() {
        gm.NextLevel();
	}

	public void PrevLevel() {
		gm.PreviousLevel();
	}

    public void ButtonZeroClicked()
    {
        et[0].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f);
    }

    public void ButtonOneClicked()
    {
        et[1].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f);
    }

    public void ButtonTwoClicked()
    {
        et[2].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f);
    }

    public void ButtonThreeClicked()
    {
        et[3].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f);
    }

    public void ButtonFourClicked()
    {
        et[4].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f);
    }

    public void ButtonFiveClicked()
    {
        et[5].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f);
    }

    public void OptionsMenu()
    {
        et[1].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        whereToGo = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ExitDialog()
    {
        exitPanel.SetActive(true);
        et[2].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
    }

    public void Exit()
    {
        et[3].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        Application.Quit();
    }

    public void No()
    {
        et[4].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        exitPanel.SetActive(false);
    }

    public void GoToStartMenu()
    {
        et[0].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        SceneManager.LoadScene("StartMenu");
    }

    public void PlayLevel()
    {
        et[4].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        NextLevel(); //whatever level was chosen, run based on level title
    }

    public void StartDialog()
    {
        startPanel.SetActive(true);
        et[2].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
    }

    public void Return()
    {
        et[5].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        startPanel.SetActive(false);
    }

    public void OpenStory()
    {
        et[0].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
    }

    public void OpenSouls()
    {
        et[1].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        SceneManager.LoadScene("Souls");
    }

    public void GoToMainMenu()
    {
        et[3].gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
        PrevLevel();
    }

}
