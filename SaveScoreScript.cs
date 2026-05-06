using UnityEngine;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class SaveScoreScript : MonoBehaviour
{

    [Header("Player Score")]
    public TMP_Text bestScoreText;
    public static int bestScore;
    void Start()
    {
        LoadInfo();
            // ResetGameData();

    }

    // void Update()
    // {
    //     // LoadInfo();


    //         // ResetGameData();
    // }
    private void OnApplicationQuit()
    {
        // ResetGameData();
        SaveInfo(bestScore); // Guardar la mejor puntuación al cerrar el juego
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); //para salir del modo play en unity
#else
            Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {

        public int bestScore;
    }
    public static void SaveInfo(int bestScore)
    {
        // Guardar la mejor puntuación en un archivo JSON

        SaveData data = new SaveData();

        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }


    public static void LoadInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);


            bestScore = data.bestScore;
        }
    }

    public void ResetGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Datos guardados eliminados.");
        }

        // Resetear variables en memoria

        bestScore = 0;
    }
}
