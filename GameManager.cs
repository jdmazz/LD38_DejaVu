/*
GameManager - destroyed on scene load. Considering that each world is completely 100% procedural,
there was no need for a persistant GameManager.
2017/4/23
@author jdmazz
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameManager gm;
	public int level;
	public string levelAfterVictory;
	public float cameraSize = 10;
	public float timeLeft = 30f;
	public Transform stage;
	public Transform canvas;
	public int stageRows = 2;
	public int stageCols = 2;
	public float stageX = 0f;
	public float stageY = 0f;
	public string taunt;

	Text levelText;
	GameObject levelImage;
	Text dejavuText;
	Text timerText;
	GameObject player;
	bool start = false;
	Camera cam;
	Text tauntText;

	[HideInInspector]
	public bool makingStage;

	void Awake() {
		InitStage();
	}

	void InitStage() {
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		cam.orthographicSize = cameraSize;
		makingStage = true;
		MakeStage();
        MakeCanvas();
        levelImage = GameObject.Find("BGLevelImage");
		levelImage.SetActive(true);
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
		levelText.text = "Small World " + level;
        dejavuText = GameObject.Find("DejaVu").GetComponent<Text>();
		player = GameObject.Find("Player");
		timerText = GameObject.Find("TimerText").GetComponent<Text>();
		tauntText = GameObject.Find("TauntText").GetComponent<Text>();
		tauntText.text = taunt;
	}

	void Update ()
	{
		if (!start) {
			// give it a little time to make the stage, but it's so fast!
			DelayedStart();
		} else if (!makingStage) {
			levelImage.SetActive (false);
			int dejavupoints = player.GetComponent<Player> ().dejavuPts;
			dejavuText.text = "Deja Vu: " + dejavupoints;
			if (dejavupoints < 1)
				LoadNextLevel ();
			timeLeft -= Time.deltaTime;
			timerText.text = "Time: " + (int)timeLeft + "s";
			if (timeLeft < 0) {
				ResetGame ();
			}
		}
	}

	void DelayedStart() {
		StartCoroutine(Delayed());

	}

	IEnumerator Delayed() {
		// Give a delay to the loading screen
		yield return new WaitForSeconds(1.5f);
		if (!makingStage && start) {
			levelImage.SetActive (false);
		}
		start = true;
	}

	void LoadNextLevel() {
		SceneManager.LoadScene(levelAfterVictory);
	}

	void ResetGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		InitStage();
		start = false;
	}

	void MakeStage() {
		// Must allocate the rows and cols before makings the stage. :-P
		stage.GetComponent<MazeMaker>().rows = stageRows;
		stage.GetComponent<MazeMaker>().cols = stageCols;
		Transform stageTrans = Instantiate(stage);
		stageTrans.localPosition = new Vector3(stageX,stageY,0);
	}

	void MakeCanvas() {
		Instantiate(canvas);
	}
}
