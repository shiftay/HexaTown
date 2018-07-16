using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum STATES { MAINMENU, PREGAME, COLLECTION, GAME, OPTIONS}

public class Deck {
	public List<int> cards = new List<int>();
	public string name;
	public int imageNumber;

	public void SetDeck(List<int> decks, string name, int img) {
		cards = decks;
		this.name = name;
		imageNumber = img;
	}
}

public class SavedGame {
	public List<int> tileSpace = new List<int>();

	public int objectiveVal, populationVal, happinessVal;
	public List<int> prevObjective = new List<int>();
	public List<int> prevPopulation = new List<int>();
	public List<int> prevHappiness = new List<int>();
}

public class BackEndManager : MonoBehaviour {

	public static BackEndManager instance;
	string SAVEPATH = "decks.txt";
	string GAMEPATH = "lastGame";
	string SETTINGSPATH = "settings.txt";
	char DELIMITER = '/';
	public List<Deck> decks = new List<Deck>();
	public int currentState;
	public int prvState;
	public List<GameObject> states = new List<GameObject>();
	public bool deleteFiles = false;
	public SavedGame sGame;
	public float currentVolume = 0.5f;
	public float mutedVolume = 0f;
	public bool mutedMusic = false;
	public bool mutedSFX = false;
	public float currentSFX = 0.5f;
	public float mutedSFXVol = 0f;

	public bool editDeck = false;
	public int deckToEdit = 0;
	public int deckChoice = -1;
	public FadeOut fo;
	
	// Use this for initialization
	void Start () {
		instance = this;
		for(int i = 0; i < transform.childCount; i++) {
			states.Add(transform.GetChild(i).gameObject);
		}

		ReadSettings();
		AudioManager.instance.setVolumes(currentSFX, currentVolume);

		Debug.Log(Application.streamingAssetsPath);

		if(deleteFiles) {
			ClearFiles();
		} else {
			ReadDecks();
		}
	}
	
	public void ChangeState(STATES state) {

		if(state == STATES.OPTIONS) {
			prvState = currentState;
			currentState = (int)state;
			states[prvState].SetActive(true);
			states[currentState].SetActive(true);
		} else {
			prvState = currentState;
			currentState = (int)state;
			fo.fade(state, this);
		}

		// prvState = currentState;
		// currentState = (int)state;
		// states[prvState].SetActive(false);
		// states[currentState].SetActive(true);
	}

	public void RevertState() {
		int temp = currentState;
		currentState = prvState;
		prvState = temp;

		states[currentState].SetActive(true);
		states[prvState].SetActive(false);
	}

	void ReadDecks() {
		if(File.Exists(Application.persistentDataPath + DELIMITER + SAVEPATH)) {
			StreamReader sr = new StreamReader(Application.persistentDataPath + DELIMITER + SAVEPATH);

			string line;

			while((line = sr.ReadLine()) != null) {
				string decrypt = Encryption(line);
				string[] split = decrypt.Split(DELIMITER);
				Deck temp = new Deck();
				List<int> deck = new List<int>();

				for(int i = 2; i < split.Length; i++) {
					deck.Add(int.Parse(split[i]));
				}

				temp.SetDeck(deck, split[0], int.Parse(split[1]));

				decks.Add(temp);
			}

			sr.Close();
		} else {
			FileStream sr = File.Open(Application.persistentDataPath + DELIMITER + SAVEPATH, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			sr.Close();
		}
	}

	void ReadSettings() {
		if(File.Exists(Application.persistentDataPath + DELIMITER + SETTINGSPATH)) {
			StreamReader sr = new StreamReader(Application.persistentDataPath + DELIMITER + SETTINGSPATH);

			string line;

			while((line = sr.ReadLine()) != null) {
				string decrypt = Encryption(line);
				string[] split = decrypt.Split(DELIMITER);

				mutedMusic = bool.Parse(split[0]);
				currentVolume = float.Parse(split[1]);
				mutedVolume = float.Parse(split[2]);
				mutedSFX = bool.Parse(split[3]);
				currentSFX = float.Parse(split[4]);
				mutedSFXVol = float.Parse(split[5]);
			}

			sr.Close();
		} else {
			FileStream sr = File.Open(Application.persistentDataPath + DELIMITER + SETTINGSPATH, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			sr.Close();
		}
	}



	void SaveDecks() {
		StreamWriter test = new StreamWriter(Application.persistentDataPath + DELIMITER + SAVEPATH, false);

		for (int i = 0; i < decks.Count; i++){
			test.WriteLine(Encryption(createDeckString(decks[i])));
		}

		test.Close();
	}

	void SaveSettings() {
		StreamWriter test = new StreamWriter(Application.persistentDataPath + DELIMITER + SETTINGSPATH, false);

		test.WriteLine(Encryption(createSettingsString()));

		test.Close();
	}	

	string createDeckString(Deck temp) {
		string retVal = "";

		string head = temp.name + DELIMITER + temp.imageNumber;
		retVal += head;

		for(int i = 0; i < temp.cards.Count; i++) {
			retVal += ("/" + temp.cards[i]);
		}

		return retVal;
	}

	string createSettingsString() {
		return mutedMusic.ToString() + DELIMITER + currentVolume.ToString() + DELIMITER + mutedVolume.ToString() 
		+ DELIMITER + mutedSFX.ToString() + DELIMITER + currentSFX.ToString() + DELIMITER + mutedSFXVol.ToString();
	}





	void OnApplicationPause(bool pauseStatus)	{
		
		if(pauseStatus) {
			SaveDecks();
			SaveSettings();
		}

	}

	// / <summary>
	// / Callback sent to all game objects before the application is quit.
	// / </summary>
	void OnApplicationQuit()
	{
		SaveDecks();
		SaveSettings();
	}

// ============= BACK END UTILITIES ====================
	string Encryption(string input) {
		string s1 = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		string s2 = "PQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNO";

		string ret = "";

		for (int i = 0; i < input.Length; i++) {
			ret += s2[s1.IndexOf(input.ToCharArray()[i])];
		}

		return ret;
	}
	void ClearFiles() {
		File.Delete(Application.persistentDataPath + GAMEPATH);
		File.Delete(Application.persistentDataPath + SAVEPATH);
	}
// ============= BACK END UTILITIES ====================
}
