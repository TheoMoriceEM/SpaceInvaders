using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    [Serializable]
    public struct Scores
    {
        public Score[] datas;
    }

    [Serializable]
    public struct Score
    {
        public int id;
        public string pseudo;
        public int score;
        public string uuid;
    }

    const string URL_REST_API = "https://api.r-c.es/m2dt-unity/asteroids";

    public Text leaderboard;
    public InputField pseudoField;
    public Text pseudoLabel;

    string playerUUID;
    string playerPseudo;

    // Start is called before the first frame update
    void Start()
    {
        print("Network : " + Application.internetReachability);
        print("Mobile : " + Application.isMobilePlatform);
        print("Platform : " + Application.platform);

        playerUUID = PlayerPrefs.GetString("player-UUID");
        if (playerUUID == "")
        {
            // Universally unique identifier
            Guid guid = Guid.NewGuid();
            playerUUID = guid.ToString();
            PlayerPrefs.SetString("player-uuid", playerUUID);
        }

        playerPseudo = PlayerPrefs.GetString("player-Pseudo");
        if (playerPseudo == "")
        {
            playerPseudo = "L'OVNI";
            PlayerPrefs.SetString("player-Pseudo", playerPseudo);
        }

        LoadScores();

        pseudoField.onValueChanged.AddListener(delegate { PseudoFieldOnChange(); });
    }

    public void PseudoFieldOnChange()
    {
        playerPseudo = pseudoField.text;

        PlayerPrefs.SetString("player-pseudo", playerPseudo);
    }

    public void SendScore(int score)
    {
        StartCoroutine(SendScoresToNetwork(score));
    }

    IEnumerator SendScoresToNetwork(int score)
    {
        // préparer les données à envoyer
        WWWForm form = new WWWForm();
        form.AddField("pseudo", playerPseudo);
        form.AddField("uuid", playerUUID);
        form.AddField("score", score);

        // envoyer
        UnityWebRequest request = UnityWebRequest.Post(URL_REST_API, form);

        // attendre
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error : " + request.error);
            leaderboard.text = "Network Error - no Leaderboard";
        }
        else
        {
            LoadScores();
        }
    }

    public void LoadScores()
    {
        StartCoroutine(LoadScoresFromNetwork());
    }

    IEnumerator LoadScoresFromNetwork()
    {
        // demande de flux réseau
        UnityWebRequest request = UnityWebRequest.Get(URL_REST_API);

        // attendre le chargement
        yield return request.SendWebRequest();

        // si erreur
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error : " + request.error);
            leaderboard.text = "Network Error - no Leaderboard";
        }
        else
        {
            string json = request.downloadHandler.text;
            print(json);
            leaderboard.text = "Network OK";

            Scores root = JsonUtility.FromJson<Scores>(json);

            leaderboard.text = "";
            for (int i = 0; i < root.datas.Length; i++)
            {
                Score score = root.datas[i];

                print(score.pseudo + " " + score.score);

                if (score.uuid == playerUUID)
                {
                    score.pseudo = "<b>" + score.pseudo + "</b>";
                }

                leaderboard.text += score.score.ToString("000 000") + " " + score.pseudo + "\n";
            }
        }

        // sinon

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
