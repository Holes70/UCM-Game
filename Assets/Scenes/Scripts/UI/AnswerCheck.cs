using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AnswerCheck : MonoBehaviour {
		public Text otazka_text;
    public InputField input;
    public GameObject quest;
		public GameObject mapa;

    public IEnumerator UpdateScore(int spravnaOdpoved) {

			WWWForm form = new WWWForm();

			form.AddField("score", Score.score);
			form.AddField("questId", ShowQuest.questId);
			form.AddField("spravnost", spravnaOdpoved);
			form.AddField("playerNickname", "Pirat2");

			using (UnityWebRequest www = UnityWebRequest.Post("https://grid3.kaim.fpv.ucm.sk/~patrikholes/pirate-game/web/index.php?action=update_score", form)) {
				yield return www.SendWebRequest();
			}
    }

    public void OnClick () {
			bool odpovedBolaSpravna = false;

			// Prejdi odpovede ak sa tam nachadza tak true
			foreach (string odpoved in ShowQuest.questAnswers) {
				if (input.text == odpoved) {
					odpovedBolaSpravna = true;
					break;
				}
			}

			if (odpovedBolaSpravna) {
				Score.score += 100;
				StartCoroutine(this.UpdateScore(1));
				Debug.Log("Spravna odpoved");
				mapa.GetComponent<Map>().Open();
				mapa.GetComponent<Map>().ChangeCross(ShowQuest.questId);

				if (HealthMonitor.HealthValue < 3) {
					HealthMonitor.HealthValue += 1;
				} 
			} else {
				HealthMonitor.HealthValue -= 1; 
				StartCoroutine(this.UpdateScore(0));
				Debug.Log("Nespravna odpoved");
			}

			Destroy(ShowQuest.questObject); // Destroy flasa_quest
			quest.SetActive(false);
			otazka_text.text = "";
    }
}
