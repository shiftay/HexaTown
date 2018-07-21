using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum STATES { MAINMENU, PREGAME, COLLECTION, GAME, ENDGAME, OPTIONS, CREDITS }

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
	public List<int> currentHand = new List<int>();
	public List<int> currentDiscard = new List<int>();
	public List<int> currentDeck = new List<int>();
	public List<int> tileSpace = new List<int>();
	public int objectiveVal, populationVal, happinessVal, commuter, party;
	public List<int> prevObjective = new List<int>();
	public List<int> prevPopulation = new List<int>();
	public List<int> prevHappiness = new List<int>();
	public List<int> tileState = new List<int>();
}

public class BackEndManager : MonoBehaviour {

	public static BackEndManager instance;
	public bool gameWon = false;
	string SAVEPATH = "decks.txt";
	string GAMEPATH = "lastGame";
	string SETTINGSPATH = "settings.txt";
	char DELIMITER = '/';
	public List<Deck> decks = new List<Deck>();
	public int currentState;
	public int prvState;
	public int creditsState;
	public List<GameObject> states = new List<GameObject>();
	public bool deleteFiles = false;
	public SavedGame sGame;
	public bool resume = false;
	public SavedGame clear {
		set {
			sGame = value;
			ClearSave();
		}
	}
	public SavedGame save {
		set {
			sGame = value;
			writeSave();
		}
	}
	public float currentVolume = 0.5f;
	public float mutedVolume = 0f;
	public bool mutedMusic = false;
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
		ReadSave();
		AudioManager.instance.setVolumes(currentSFX, currentVolume);
		AudioManager.instance.mute(mutedMusic);
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
		} else if(state == STATES.CREDITS) {
			creditsState = prvState;
			prvState = currentState;
			currentState = (int)state;
			fo.fade(state, this);
			// states[creditsState].SetActive(false);
			// states[prvState].SetActive(false);
			// states[currentState].SetActive(true);

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

	public void LeaveGame() {
		states[currentState].SetActive(false);
		currentState = (int)STATES.PREGAME;
		
		fo.fade((STATES)currentState, this);
	}

	public void RevertState() {
		int temp = currentState;
		currentState = prvState;
		prvState = temp;

		states[currentState].SetActive(true);
		states[prvState].SetActive(false);
	}

	public void LeaveCredits() {


		// states[creditsState].SetActive(true);
		// states[prvState].SetActive(true);
		// states[currentState].SetActive(false);
		currentState = prvState;
		prvState = creditsState;	

		fo.fade((STATES)currentState, this);
		
	}

	void ClearSave() {
		File.Delete(Application.persistentDataPath + DELIMITER + "saveGame.txt");
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
				currentSFX = float.Parse(split[3]);
				mutedSFXVol = float.Parse(split[4]);
			}

			sr.Close();
		} else {
			FileStream sr = File.Open(Application.persistentDataPath + DELIMITER + SETTINGSPATH, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			sr.Close();
		}
	}



	void ReadSave() {
		if(File.Exists(Application.persistentDataPath + DELIMITER + "saveGame.txt")) {
			StreamReader sr = new StreamReader(Application.persistentDataPath + DELIMITER + "saveGame.txt");

			string line;
			int counter = 0;
			SavedGame temp = new SavedGame();

			while((line = sr.ReadLine()) != null) {
				string[] split = line.Split('/');

				switch(counter) {
					case 0:	//deck
						foreach(string x in split) {
							if(x != "") {
								temp.currentDeck.Add(int.Parse(x));
							}
							
						}

						break;
					case 1:	//discard
						foreach(string x in split) {
							if(x != "") {
							temp.currentDiscard.Add(int.Parse(x));
							}
						}

						break;
					case 2:	// hand
						foreach(string x in split) {
							if(x != "") {
								temp.currentHand.Add(int.Parse(x));
							}
						}
					 	break;
					case 3:	// happ
						foreach(string x in split) {
							if(x != "") {
								temp.prevHappiness.Add(int.Parse(x));
							}
						}
						break;
					case 4:// obj
						foreach(string x in split) {
							if(x != "") {
								temp.prevObjective.Add(int.Parse(x));
							}
						}
						break;
					case 5: // pop
						foreach(string x in split) {
							if(x != "") {
								temp.prevPopulation.Add(int.Parse(x));
							}
						}
						break;
					case 6: // tile
						foreach(string x in split) {
							if(x != "") {
								temp.tileSpace.Add(int.Parse(x));
							}
						}
						break;
					case 7: // tile
						foreach(string x in split) {
							if(x != "") {
								temp.tileState.Add(int.Parse(x));
							}
						}
						break;
					case 8: // happ, pop , obj
						temp.happinessVal = int.Parse(split[0]);
						temp.populationVal = int.Parse(split[1]);
						temp.objectiveVal = int.Parse(split[2]);
						temp.party = int.Parse(split[3]);
						temp.commuter = int.Parse(split[4]);
						break;
				}

				counter++;


			}
			sGame = temp;
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
		+ DELIMITER + currentSFX.ToString() + DELIMITER + mutedSFXVol.ToString();
	}


	public void writeSave() {
			FileStream sr = File.Open(Application.persistentDataPath + DELIMITER + "saveGame.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
			sr.Close();

			StreamWriter test = new StreamWriter(Application.persistentDataPath + DELIMITER + "saveGame.txt", false);

			string temp = "";
			foreach(int x in sGame.currentDeck) {
				temp += x.ToString() + "/";
			}

			if(temp != "") {
				temp.Remove(temp.LastIndexOf('/')-1, 1);
			} 


			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.currentDiscard) {
				temp += x.ToString() + "/";
			}

			temp.Remove(temp.LastIndexOf('/')-1, 1);
			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.currentHand) {
				temp += x.ToString() + "/";
			}

			if(temp != "") {
				temp.Remove(temp.LastIndexOf('/')-1, 1);
			} 
			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.prevHappiness) {
				temp += x.ToString() + "/";
			}

			temp.Remove(temp.LastIndexOf('/')-1, 1);
			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.prevObjective) {
				temp += x.ToString() + "/";
			}

			temp.Remove(temp.LastIndexOf('/')-1, 1);
			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.prevPopulation) {
				temp += x.ToString() + "/";
			}

			temp.Remove(temp.LastIndexOf('/')-1, 1);
			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.tileSpace) {
				temp += x.ToString() + "/";
			}

			temp.Remove(temp.LastIndexOf('/')-1, 1);
			test.WriteLine(temp);

			temp = "";
			foreach(int x in sGame.tileState) {
				temp += x.ToString() + "/";
			}

			temp.Remove(temp.LastIndexOf('/')-1, 1);
			test.WriteLine(temp);

			temp = sGame.happinessVal.ToString() + "/" + sGame.populationVal.ToString() + "/" + sGame.objectiveVal.ToString() + "/" +
				sGame.party + "/" + sGame.commuter;
			test.WriteLine(temp);

			test.Close();
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
		File.Delete(Application.persistentDataPath + SETTINGSPATH);
	}
// ============= BACK END UTILITIES ====================
}
