using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{

    public GameObject rowPrefab;
    public Transform rowsParent;
    [SerializeField] private GameObject scoreboardPanel;
    [SerializeField] private GameObject table;
    private bool streched = false;
    private int updated = 0;

    [SerializeField] private Text highscoreText;
    public int currentHighScore = 0;

    [Header("UI")]
    public Text messageText;
    public InputField userNameField;
    public InputField passwordField;
    public Sprite lightbg;
    public Sprite darkbg;

    public static PlayFabManager Instance;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        if (SceneManager.GetActiveScene().buildIndex == 1) {
            highscoreText.text = "Highscore: 0";
            GetLeaderboard();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            GetLeaderboard();
        }
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void ClickedOnLeaderboard() {
        FindObjectOfType<AudioManager>().Play("ButtonSound");

        if (scoreboardPanel.activeSelf) {

            scoreboardPanel.SetActive(false);

        }
        else if (!scoreboardPanel.activeSelf) {

            scoreboardPanel.SetActive(true);

        }

    }

    public void RegisterButton() {
        if (passwordField.text.Length < 6) {
            messageText.text = "Password too short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest {
            Username = userNameField.text,
            Password = passwordField.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        messageText.text = "Successful Registration!";
        PlayerPrefs.SetString("PlayerID", result.PlayFabId);
        SceneManager.LoadScene(1);
    }

    public void LoginButton() {
        var request = new LoginWithPlayFabRequest {
            Username = userNameField.text,
            Password = passwordField.text
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);
    }

    public void OnLoginSuccess(LoginResult result) {
        messageText.text = "Successful Login";
        PlayerPrefs.SetString("PlayerID", result.PlayFabId);
        SceneManager.LoadScene(1);
    }

    public void OnError(PlayFabError error) {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            messageText.text = error.ErrorMessage;
        }
        Debug.Log(error);
    }

    public void SendLeaderboard(int score) {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "SpeedBurger",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result) {

        

    }

    public void GetLeaderboard() {

        var request = new GetLeaderboardRequest {
            StatisticName = "SpeedBurger",
            StartPosition = 0,
            MaxResultsCount = 30
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    public void OnLeaderboardGet(GetLeaderboardResult result) {
        
        if (rowsParent != null) {
            foreach (Transform item in rowsParent) {
                if (item.gameObject != null) {
                    Destroy(item.gameObject);
                }
            }
        }
        
        foreach (var item in result.Leaderboard) {

            if (PlayerPrefs.GetString("PlayerID", "nothing") == item.PlayFabId) {
                currentHighScore = item.StatValue;
                highscoreText.text = "Highscore: " + item.StatValue.ToString();
                PlayerPrefs.SetInt("Highscore", currentHighScore);
            }

            // Debug.Log("id: "+ PlayerPrefs.GetString("PlayerID", "nothing"));
            // Debug.Log("item.PlayFabID: "+ item.PlayFabId);

            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = item.Position.ToString();
            texts[1].text = item.PlayFabId;
            texts[2].text = item.StatValue.ToString();

            
            if (item.Position % 2 == 0 && SceneManager.GetActiveScene().buildIndex == 1) {
                newGo.GetComponent<Image>().sprite = darkbg;
            }else if (SceneManager.GetActiveScene().buildIndex == 1 && item.Position % 2 != 0){
                newGo.GetComponent<Image>().sprite = lightbg;
            }

        }

        if (result.Leaderboard.Count > updated) {
            streched = false;
        }

        if (!streched && SceneManager.GetActiveScene().buildIndex == 1) {
            updated = result.Leaderboard.Count;

            table.GetComponent<RectTransform>().sizeDelta = new Vector3 (table.GetComponent<RectTransform>().sizeDelta.x, 1616.8f);
            streched = true;

            table.GetComponent<RectTransform>().sizeDelta = 
            new Vector3 (table.GetComponent<RectTransform>().sizeDelta.x, table.GetComponent<RectTransform>().sizeDelta.y + ((result.Leaderboard.Count - 11) * 133.5f));
            streched = true;
        }

    }

}
