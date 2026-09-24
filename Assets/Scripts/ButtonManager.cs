using System.Diagnostics;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Diagnostics;

public class ButtonManager : MonoBehaviour
{

    public int CHeckOrder = 5;
    public int score = 0;

    private int countDownInt = 5;
    
    public Text countDownText1;
    public Text countDownText2;
    public Text ScoreTextLost;
    public Text HighscoreText;
    public Text scoreText;

    public float timeMinusing = 0.04f;


    [SerializeField] private GameObject[] PausedItems;
    public bool GamePaused = false;

    [SerializeField] private GameObject LostPanel;
    [SerializeField] private GameObject ReklamaPanel;
    private bool rutinachairto = false;
    private bool rutinachairto1 = false;
    public bool counting = true;


    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    [SerializeField] private Animator animator3;
    [SerializeField] private Animator animator4;
    [SerializeField] private Animator animator5;
    [SerializeField] private Animator animator6;


    [SerializeField] private GameObject soundButtonPlace;
    [SerializeField] private GameObject retryButtonPlace;
    [SerializeField] private GameObject pauseButtonPlace;

    [SerializeField] private GameObject HealthBar;

    private float uiSpeed = 0.09f;

    // Start is called before the first frame update
    void Start()
    {
        CHeckOrder = 5;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (ButgerOrderManager.Instance.timeToSet == true) {
            CHeckOrder = ButgerOrderManager.Instance.burgerSize;
            ButgerOrderManager.Instance.timeToSet = false;
            //UnityEngine.Debug.Log("checkorder: " + CHeckOrder);
        }

        if (GamePaused && PausedItems[0].transform.position != soundButtonPlace.transform.position) {
            PausedItems[0].transform.position = Vector3.MoveTowards(PausedItems[0].transform.position, soundButtonPlace.transform.position , uiSpeed);
        }
        if (PausedItems[0].transform.position == pauseButtonPlace.transform.position) {
            PausedItems[0].SetActive(false);
        }
        if (!GamePaused && PausedItems[0].transform.position != pauseButtonPlace.transform.position) {
            PausedItems[0].transform.position = Vector3.MoveTowards(PausedItems[0].transform.position, pauseButtonPlace.transform.position , uiSpeed);
        }


        if (GamePaused && PausedItems[1].transform.position != retryButtonPlace.transform.position) {
            PausedItems[1].transform.position = Vector3.MoveTowards(PausedItems[1].transform.position, retryButtonPlace.transform.position , uiSpeed);
        }
        if (PausedItems[1].transform.position == pauseButtonPlace.transform.position) {
            PausedItems[1].SetActive(false);
        }
        if (!GamePaused && PausedItems[1].transform.position != pauseButtonPlace.transform.position) {
            PausedItems[1].transform.position = Vector3.MoveTowards(PausedItems[1].transform.position, pauseButtonPlace.transform.position , uiSpeed);
        }

        HealthBar.GetComponent<Slider>().value -= timeMinusing * Time.deltaTime;

        if (CHeckOrder == -1) {
            //CHeckOrder = ButgerOrderManager.Instance.burgerSize;
            score += 1;
            HealthBar.GetComponent<Slider>().value = 1;
            

            if (score <= 10) {
                timeMinusing += 0.015f;
            }else {
                timeMinusing += 0.005f;
            }

            ButgerOrderManager.Instance.nextLevel = true;
            ButgerOrderManager.Instance.daburgereba = true;
        }

        if (score > PlayerPrefs.GetInt("Highscore", 0)) {
            PlayerPrefs.SetInt("Highscore", score);
        } 

        if (HealthBar.GetComponent<Slider>().value == 0) {
            LostPanel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("GameOver");

            if (!rutinachairto) {
                StartCoroutine(CountDown());
                PlayFabManager.Instance.SendLeaderboard(score);
                PlayFabManager.Instance.GetLeaderboard();
                rutinachairto = true;
            }

            countDownText1.text = countDownInt.ToString();

            ScoreTextLost = GameObject.FindGameObjectWithTag("LostScoreText").GetComponent<Text>();
            HighscoreText = GameObject.FindGameObjectWithTag("HighScore").GetComponent<Text>();

            ScoreTextLost.text = "Score: " + score.ToString();
            HighscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        }

        if (GamePaused) {
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.currentSelectedGameObject == null) {
                    PauseClicked();
                }
            }
        }

        if (countDownText1.text == "0") {
            countDownText1.text = "0";
            StopCoroutine(CountDown());
            counting = false;
        }

        if (Input.GetMouseButtonDown(0) && LostPanel.activeSelf && !ReklamaPanel.activeSelf) {
            if (EventSystem.current.currentSelectedGameObject == null) {
                countDownText1.text = countDownInt.ToString();
                countDownInt = 5;
                ReklamaPanel.SetActive(true);
                counting = true;
            }
        }
       

        if (ReklamaPanel.activeSelf) {
            if (!rutinachairto1 && countDownText1.text == "0") {
                StartCoroutine(CountDown());
                rutinachairto1 = true;

            }

            counting = true;
            countDownText2.text = countDownInt.ToString();
            if (countDownText2.text == "0") {
                StopCoroutine(CountDown());
                counting = false;
                SceneManager.LoadScene(2);
            }
            
        }

        scoreText.text = score.ToString();
    }

    public IEnumerator CountDown() {
        while(counting) {
            countDownInt -= 1;
            yield return new WaitForSecondsRealtime(1);
            UnityEngine.Debug.Log("-1");
        }
        yield return new WaitForSeconds(0.2f);
    }


    public void PauseClicked() {
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (GamePaused) {
            Time.timeScale = 1;

            for (int i = 0; i < ButgerOrderManager.Instance.SpawnedIngredients.Length; i++) {
                ButgerOrderManager.Instance.SpawnedIngredients[i].SetActive(true);
            }

            PausedItems[2].SetActive(false);

            GamePaused = false;
        }else if (!GamePaused) {
            Time.timeScale = 0;
            PausedItems[0].SetActive(true);
            PausedItems[1].SetActive(true);
            PausedItems[2].SetActive(true);

            
            for (int i = 0; i < ButgerOrderManager.Instance.SpawnedIngredients.Length; i++) {
                ButgerOrderManager.Instance.SpawnedIngredients[i].SetActive(false);
            }

            GamePaused = true;
        }
    }

    public void RestartGame() {
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        PlayFabManager.Instance.SendLeaderboard(score);
        PlayFabManager.Instance.GetLeaderboard();
        SceneManager.LoadScene(2);
    }

    public void BackToMenu() {
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        PlayFabManager.Instance.SendLeaderboard(score);
        PlayFabManager.Instance.GetLeaderboard();
        SceneManager.LoadScene(1);
    }

    public void ClickedOnCheese() {
        animator1.Play("TestSubject01");
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Cheese") && !GamePaused) {
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Animator>().Play("Pressed");
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            CHeckOrder -= 1;
        }else if (!ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Cheese") && !GamePaused) {
            HealthBar.GetComponent<Slider>().value -= 0.1f;
            FindObjectOfType<AudioManager>().Play("BurgerMistake");
        }
    }

    public void ClickedOnTomato() {
        animator2.Play("TestSubject01");
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Tomato") && !GamePaused) {
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Animator>().Play("Pressed");
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            CHeckOrder -= 1;
        }else if (!ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Tomato") && !GamePaused) {
            HealthBar.GetComponent<Slider>().value -= 0.1f;
            FindObjectOfType<AudioManager>().Play("BurgerMistake");
        }
    }

    public void ClickedOnBread() {
        animator3.Play("TestSubject01");
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Bread") && !GamePaused) {
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Animator>().Play("Pressed");
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            CHeckOrder -= 1;
        }else if (!ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Bread") && !GamePaused) {
            HealthBar.GetComponent<Slider>().value -= 0.1f;
            FindObjectOfType<AudioManager>().Play("BurgerMistake");
        }

        UnityEngine.Debug.Log("checkorder: " + CHeckOrder);
    }

    public void ClickedOnMeat() {
        animator4.Play("TestSubject01");
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Meat") && !GamePaused) {
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Animator>().Play("Pressed");
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            CHeckOrder -= 1;
        }else if (!ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Meat") && !GamePaused) {
            HealthBar.GetComponent<Slider>().value -= 0.1f;
            FindObjectOfType<AudioManager>().Play("BurgerMistake");
        }
    }

    public void ClickedOnSalad() {
        animator5.Play("ForSalad");
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Salad") && !GamePaused) {
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Animator>().Play("Pressed");
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            CHeckOrder -= 1;
        }else if (!ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Salad") && !GamePaused) {
            HealthBar.GetComponent<Slider>().value -= 0.1f;
            FindObjectOfType<AudioManager>().Play("BurgerMistake");
        }
    }

    public void ClickedOnOnion() {
        animator6.Play("TestSubject01");
        FindObjectOfType<AudioManager>().Play("ButtonSound");
        if (ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Onion") && !GamePaused) {
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Animator>().Play("Pressed");
            ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            CHeckOrder -= 1;
        }else if (!ButgerOrderManager.Instance.SpawnedIngredients[CHeckOrder].CompareTag("Onion") && !GamePaused) {
            HealthBar.GetComponent<Slider>().value -= 0.1f;
            FindObjectOfType<AudioManager>().Play("BurgerMistake");
        }
    }
}
