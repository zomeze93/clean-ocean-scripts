using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager manager;

    public bool estaEnGameOver { get; private set; }
    public bool JuegoParado => estaEnGameOver || MenuManager.Instance.estaPausado;
    //Speed Objetos instanciados
    [SerializeField] internal float speedShark;
    //Cosas del player
    [Header("Player LifeUI")]
    [SerializeField] PlayerHealth playerHealthScript; //si no lo uso borrarlo
    [SerializeField] GameObject healtUI;
    [SerializeField] internal int lifePlayer;
    internal int maxLife = 3;
    internal float heartSizeY = 45f;
    internal float heartSizeX = 53f;
    internal RectTransform healtUIRect;

    internal bool isPlayerDead = false;



    [Header("BG Game Over")]
    [SerializeField] internal bool gameOverMostrado = false;
    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private GameObject bubblesPlayer;

    [Header("Puntuacion")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] internal float score;
    [SerializeField] internal float count;

    [SerializeField] internal TextMeshProUGUI gameOverScoreText;
    public string idiomScoreText;
    public int newScore;

    [Header("Button Movement")]
    [SerializeField] GameObject buttonMovement;

    [Header("Show Messages")]
    [SerializeField] MessagesScripts messagesScripts;

    private void Awake()
    {
        #region Singleton
        if (manager == null)
            manager = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(manager);
        #endregion
        SceneManager.sceneLoaded += OnSceneLoaded;
        scoreText = GetComponent<TextMeshProUGUI>();

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerHealthScript = FindAnyObjectByType<PlayerHealth>();
        if (scoreText == null)
        {
            GameObject textObject = GameObject.FindWithTag("TextPoints");
            if (textObject != null) scoreText = textObject.GetComponent<TextMeshProUGUI>();
        }

        if (healtUI == null)
        {
            healtUI = GameObject.FindWithTag("HealthUI");
            if (healtUI != null)
            {
                healtUIRect = healtUI.GetComponent<RectTransform>();
            }
        }
        if (gameOverBG == null)
        {
            gameOverBG = GameObject.FindWithTag("BGGameOver");
            if (gameOverBG != null)
            {
                gameOverBG.SetActive(false);
                GameObject gameOverTextObject = gameOverBG.gameObject.GetComponentInChildren<TextMeshProUGUI>().gameObject;
                if (gameOverTextObject != null)
                {
                    gameOverScoreText = gameOverTextObject.GetComponent<TextMeshProUGUI>();

                }
            }
        }

        if (bubblesPlayer == null)
        {
            bubblesPlayer = GameObject.FindWithTag("BubblesPlayer");
            if (bubblesPlayer != null)
            {
                bubblesPlayer.SetActive(true);
            }
        }


        //Resetear valores
        ResetLevel();

    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        healtUIRect = healtUI.GetComponent<RectTransform>();

    }

    private void Update()
    {
        if (score < 0)
        {
            score = 0;
        }
        if (!JuegoParado)
        {

            score += 10 * Time.deltaTime;

            scoreText.text = score.ToString("0");
            // gameOverScoreText.text = "Score: " + score.ToString("0");
            // gameOverScoreText.text = idiomScoreText + score.ToString("0");
            // gameOverScoreText.text = score.ToString("0");
            newScore = (int)score;


            //Add dificultad
            count += 10 * Time.deltaTime;
            Adddificult();
        }

        if (isPlayerDead)
        {
            if (!gameOverMostrado)
            {
                PlayerDead();
                gameOverMostrado = true;
                if (SaveScoreScript.bestScore < (int)score)
                {
                    // SaveScoreScript.SaveInfo(Mathf.RoundToInt(score));
                    SaveScoreScript.SaveInfo((int)score);
                    SaveScoreScript.LoadInfo();
                }
            }
        }


    }

    public void SumarPuntos(float puntosEntrada)
    {
        score += puntosEntrada;
    }


    public void GameOver(int index)
    {
        SceneManager.LoadScene(index);

    }
    private void ResetLevel()
    {

        score = 0;
        scoreText.text = score.ToString("0");
        lifePlayer = maxLife;
        speedShark = 4;
        isPlayerDead = false;
        count = 0;
        gameOverMostrado = false;
        estaEnGameOver = false;
        MenuManager.Instance.ReanudarJuego();
    }

    private void Adddificult()
    {
        if (count > 1000)
        {
            speedShark = 8f;
        }
        else if (count > 750)
        {
            speedShark = 7f;
        }
        else if (count > 500)
        {
            speedShark = 6f;
        }
        else if (count > 250)
        {
            speedShark = 5f;
        }

    }

    private void PlayerDead()
    {
        newScore = (int)score;

        gameOverBG.SetActive(true);
        estaEnGameOver = true;

        if (bubblesPlayer != null)
        {
            bubblesPlayer.SetActive(false);
        }

        // ACTIVAR SCORE
        GameObject scoreObj = GameObject.FindWithTag("GameOverScoreText");
        scoreObj.GetComponent<FindGameManager>().enabled = true;
        scoreObj.GetComponent<LocalizeStringEvent>().enabled = true;
        // if (scoreObj != null)
        // scoreObj.SetActive(true);
        // StartCoroutine(ActivateScoreObject());

    }
    // private IEnumerator ActivateScoreObject()
    // {
    //     yield return null;
    //     // if (gameManager.gameOverMostrado)
    //     // {
    //     //     if (scoreObject != null)
    //     //     scoreObject.SetActive(true);
    //     // }
    // }

}
