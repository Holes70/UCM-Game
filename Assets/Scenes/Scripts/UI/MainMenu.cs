using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

		private Button zmenaOtazokButton;
		private InputField zmenaOtazokInput;
		private Dropdown generatorDropdown;

		private GameObject zmenaOtazokButtonObject;
		private GameObject zmenaOtazokInputObject;
		private GameObject generatorDropdownObject;

		private int zmenaOtazokButtonStav = 1;

		[System.Serializable]
		public class Generator {
			public int id;
			public string name;
		}

		[System.Serializable]
		public class Generators {
			public string status;
			public Generator[] data;
		}

    public void Start() {
      this.zmenaOtazokButton = GameObject.Find("ZmenaOtazokButton").GetComponent<Button>();
			this.zmenaOtazokInput = GameObject.Find("ZmenaOtazokInput").GetComponent<InputField>();
			this.generatorDropdown = GameObject.Find("GeneratorDropdown").GetComponent<Dropdown>();

			this.zmenaOtazokButtonObject = GameObject.Find("ZmenaOtazokButton");
			this.zmenaOtazokInputObject = GameObject.Find("ZmenaOtazokInput");
			this.generatorDropdownObject = GameObject.Find("GeneratorDropdown");

			this.zmenaOtazokInputObject.SetActive(false);
			this.generatorDropdownObject.SetActive(false);

			this.zmenaOtazokButton.onClick.AddListener(ZmenaOtazokButtonOnClick);
    }

		void ZmenaOtazokButtonOnClick() {
			if (this.zmenaOtazokButtonStav == 1) {
				this.zmenaOtazokInputObject.SetActive(true);
				this.zmenaOtazokButtonObject.GetComponentInChildren<Text>().text = "Použiť kód";
				this.zmenaOtazokButtonStav = 2;
			} else if(this.zmenaOtazokButtonStav == 2) {
				StartCoroutine(this.getGenerators());
				this.zmenaOtazokButtonStav = 1;
				PlayerManager.idGenerator = 1;
			}
		}

		public IEnumerator getGenerators() {
			using (UnityWebRequest www = UnityWebRequest.Get(
				"http://localhost/holes/pirate-game/web/index.php?action=get_generators&uid=" + zmenaOtazokInput.text 
			)) {
				yield return www.SendWebRequest();

				if (www.isNetworkError || www.isHttpError) {
					Debug.Log("Databazovy error");
				} else {
					generatorDropdown.ClearOptions();
					Generators response = JsonUtility.FromJson<Generators>(www.downloadHandler.text);

					if (response.status == "sucess") {
						this.generatorDropdownObject.SetActive(true);
						this.zmenaOtazokInputObject.SetActive(false);
						this.zmenaOtazokButtonObject.GetComponentInChildren<Text>().text = "Použiť vybrané otázky";

						List<string> dropdownOptions = new List<string>();
						
						foreach (Generator generator in response.data) {
							dropdownOptions.Add(generator.name);
						}

						generatorDropdown.AddOptions(dropdownOptions);
					} else if (response.status == "empty") {

					} else if (response.status == "error") {

					}
				}

			}
		}

    public void PlayGame()
    {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            slider.value = progress;

            yield return null;
        }   
    }

    public void LoadGame()
    {

    }

    public void Settings()
    {

    }

    public void Score()
    {

    }

    public void AddQuests()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }




}
