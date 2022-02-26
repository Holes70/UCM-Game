using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Score : MonoBehaviour
{

    public static int score = 0;
    public Text text;

    public IEnumerator getPlayerScore() {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/holes/UcmGameWeb/web/index.php?action=get_score")) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Score.score = 10;
            } else {
                Score.score = int.Parse(www.downloadHandler.text);
            }
            
        }
    }

    void Start() {
        StartCoroutine(this.getPlayerScore());
    }
  
    void Update () {
        text.text = score.ToString() + "x";
    }


}
