using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public ParticleSystem particleDaño;
    [SerializeField] SpriteRenderer spriteBuzo;
    [SerializeField] BoxCollider2D buzoCollider;

    [Header("SFX Sounds")]
    [SerializeField] private MenuManager menuManager;

    [Header("BG Game Over")]
    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private GameObject bubblesPlayer;

    [Header("Puntuacion")]
    [SerializeField] private GameObject[] collisionThingsText;

    [Header("Basura Recogida")]
    [SerializeField] private TMP_Text[] trashCollectedText;
    [SerializeField] internal int trashCollected;



    private void Awake()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
        gameManager = GameManager.manager.GetComponent<GameManager>();
        gameManager.lifePlayer = gameManager.maxLife;
        trashCollected = 0;
        trashCollectedText[0].text = trashCollected.ToString();
        trashCollectedText[1].text = trashCollected.ToString();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Si toco Shark me quita vida
        if (other.CompareTag("Shark"))
        {
            Daño(other, 1);
            collisionThingsText[0].SetActive(true);


        }
        else if (other.CompareTag("Ballena"))
        {
            Daño(other, 0);
            collisionThingsText[1].SetActive(true);


        }
        if (other.CompareTag("Heart"))
        {
            if (gameManager.lifePlayer < gameManager.maxLife)
            {
            menuManager.MenuClick(3); //Sonido de Subir Vida

            gameManager.lifePlayer += 1;
            gameManager.healtUIRect.sizeDelta = new Vector2(gameManager.heartSizeX * gameManager.lifePlayer, gameManager.heartSizeY);
            }
            other.gameObject.SetActive(false);


        }
        if (other.CompareTag("Trash"))
        {
            menuManager.MenuClick(5); //Sonido de Agarrar Basura
            gameManager.SumarPuntos(100);
            other.gameObject.SetActive(false);
            collisionThingsText[2].SetActive(true);
            SumarBasura();

        }
    }

    private void Daño(Collider2D other, int  daño)
    {
        CinemachineMovementCamera.intance.MoverCamara(2,2,0.5f);
        particleDaño.Play();
        menuManager.MenuClick(2); //Sonido de Daño
        StartCoroutine(VisualDaño(1f,spriteBuzo, buzoCollider));
        gameManager.lifePlayer -= daño;
        gameManager.healtUIRect.sizeDelta = new Vector2(gameManager.heartSizeX * gameManager.lifePlayer, gameManager.heartSizeY);

        if (other.CompareTag("Shark")) gameManager.SumarPuntos(-100);
        else if (other.CompareTag("Ballena")) gameManager.SumarPuntos(-200);

        other.gameObject.SetActive(false);
        if (gameManager.lifePlayer == 0)
        {
            gameManager.isPlayerDead = true;
        }

    }

    IEnumerator VisualDaño(float duracion, SpriteRenderer buzoSprite, BoxCollider2D collider)
    {
        float tiempo = 0f;
        SpriteRenderer newColorSprite = buzoSprite;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            newColorSprite.color = Color.red;
            collider.enabled = false;

            yield return null;
        }
        collider.enabled = true;
        newColorSprite.color = Color.white;
    }

    public void SumarBasura()
    {
        trashCollected++;
        trashCollectedText[0].text = trashCollected.ToString();
        trashCollectedText[1].text = trashCollected.ToString();
    }
}
