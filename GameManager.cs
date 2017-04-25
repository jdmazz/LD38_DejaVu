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

	Text levelText;
	GameObject levelImage;
	Text dejavuText;
	Text timerText;
	GameObject player;
	bool start = false;
	Camera cam;

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
	}

	void Update ()
	{
		if (!start) {
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
		Transform stageTrans = Instantiate(stage);
		stageTrans.localPosition = new Vector3(stageX,stageY,0);
		stage.GetComponent<MazeMaker>().rows = stageRows;
		stage.GetComponent<MazeMaker>().cols = stageCols;
	}

	void MakeCanvas() {
		Instantiate(canvas);
	}
}
