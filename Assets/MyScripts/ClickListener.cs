using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ClickListener : MonoBehaviour {

	static GameObject rootCanvas;
	static GameObject basePanel;
	static GameObject infoPanel;
	static GameObject completedPanel;
	static GameObject loadingPanel;
	static GameObject rulesPanel;
	static GameObject cheatPanel;
	static GameObject currentStatus;
	static GameObject result;
	static GameObject error;

	static bool initialized = false;
	static string prefsKey = "ANSWERED";
	static char[] separator = {'<'};

	static string PASSWORD = "password";

	public static HashSet<string> answers = new HashSet<string>();
	static int totalWords;
	static int maxWordsToUnlock = 20;

	void Start() {
		if (initialized)
			return;

		rootCanvas = GameObject.Find ("RootCanvas");
		basePanel = GameObject.Find ("BasePanel");
		infoPanel = GameObject.Find ("InfoPanel");
		loadingPanel = GameObject.Find ("LoadingPanel");
		completedPanel = GameObject.Find ("CompletedPanel");
		rulesPanel = GameObject.Find ("RulesPanel");
		cheatPanel = GameObject.Find ("CheatPanel");

		currentStatus = GameObject.Find ("CurrentStatus");
		result = GameObject.Find ("Result");
		error = GameObject.Find ("Error");

		infoPanel.SetActive (false);
		loadingPanel.SetActive (false);
		completedPanel.SetActive (false);
		rulesPanel.SetActive (false);
		cheatPanel.SetActive (false);
	
		InitializeAnswers ();
		totalWords = answers.Count;

		UpdateCurrentStatus ();
		initialized = true;
	}

	private void UpdateCurrentStatus() {
		int count = GetPlayerAnswers ().Length;
		count = count > 0 ? count - 1 : count;
		currentStatus.GetComponent<Text> ().text = "You got " + count + "/" + maxWordsToUnlock + " words correct";
	}

	public void OnSubmitClick () {
		InputField field = basePanel.GetComponentInChildren<InputField> ();
		string currAnswer = field.text.ToLower();
		Debug.LogError ("Submit clicked with text " + currAnswer);
		string[] playerAnswers = GetPlayerAnswers ();
		string answerString = GetAnswerString ();

		if (answers.Contains (currAnswer)) {
			foreach (string playerAnswer in playerAnswers) {
				if (playerAnswer.Equals(currAnswer)) {
					Debug.LogError("You have already answered this.");
					return;
				}
			}

			if (playerAnswers.Length == 0)
				answerString = currAnswer;
			else 
				answerString += separator[0] + currAnswer;
			
			PlayerPrefs.SetString(prefsKey, answerString);

			UpdateCurrentStatus ();
			if (playerAnswers.Length >= maxWordsToUnlock) {
				basePanel.gameObject.SetActive (false);
				TransitionToPhotos();
			} else {
				StartCoroutine(ShowAnswerRight());
				Debug.LogError("Congratulations, you found one more word. Keep Going!!");
			}


			
			
		} else {
			StartCoroutine(ShowAnswerWrong());
			Debug.LogError("Oops!! Keep trying");
		}
					
	}

	private IEnumerator ShowAnswerWrong() {
		result.SetActive (true);
		result.GetComponent<Text>().text = "Oops!";
		result.GetComponent<Text> ().color = Color.red;
		yield return new WaitForSeconds(2);

		result.SetActive (false);
	}

	private IEnumerator ShowAnswerRight() {
		result.SetActive (true);
		result.GetComponent<Text>().text = "Awesome!";
		result.GetComponent<Text> ().color = Color.green;
		yield return new WaitForSeconds(2);
		
		result.SetActive (false);
	}

	private IEnumerator ShowPasswordWrong() {
		error.SetActive (true);
		error.GetComponent<Text>().text = "Wrong password. Try again";
		error.GetComponent<Text> ().color = Color.red;
		yield return new WaitForSeconds(2);
		
		error.SetActive (false);
	}
	
	private string[] GetPlayerAnswers() {
		return GetAnswerString().Split(separator);

	}

	private string GetAnswerString() {
		return PlayerPrefs.GetString(prefsKey, string.Empty);	
	}

	private void TransitionToPhotos() {
		loadingPanel.gameObject.SetActive (true);
		Application.LoadLevel(1);
	}
	public void OnInfoClick () {
		basePanel.gameObject.SetActive (false);
		infoPanel.gameObject.SetActive (true);
	}

	public void OnInfoCloseClick () {
		basePanel.gameObject.SetActive (true);
		infoPanel.gameObject.SetActive (false);
	}

	public void OnCompleteClick () {
		basePanel.gameObject.SetActive (false);
		completedPanel.gameObject.SetActive (true);

		GameObject wordList = GameObject.Find ("WordList");
		Text text = wordList.GetComponent<Text> ();
		text.text = GetAnswerString ();
	}

	
	public void OnCompleteCloseClick () {
		basePanel.gameObject.SetActive (true);
		completedPanel.gameObject.SetActive (false);
	}

	public void OnRulesClick() {
		basePanel.gameObject.SetActive (false);
		rulesPanel.gameObject.SetActive (true);
	}

	public void OnRulesCloseClick() {
		basePanel.gameObject.SetActive (true);
		rulesPanel.gameObject.SetActive (false);

	}

	public void OnCheatClick() {
		basePanel.gameObject.SetActive (false);
		cheatPanel.gameObject.SetActive (true);
	}

	public void OnCheatCloseClick() {
		basePanel.gameObject.SetActive (true);
		cheatPanel.gameObject.SetActive (false);
	}

	public void OnCheatSubmitClick() {
		InputField field = cheatPanel.GetComponentInChildren<InputField> ();
		if (field.text.Equals (PASSWORD)) {
			cheatPanel.gameObject.SetActive(false);
			TransitionToPhotos ();
		} else {
			StartCoroutine(ShowPasswordWrong());
			Debug.LogError("Incorrect password");
		}

	}
	
	void InitializeAnswers () {
		answers.Add ("1");
		answers.Add ("2");
		answers.Add ("3");

	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}
	
	
}

//TODO:
//Imp: Test correctness of max unlock words
//Back button handling

