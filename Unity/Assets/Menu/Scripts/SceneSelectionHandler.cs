using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneSelectionHandler : MonoBehaviour
{
    public static SceneSelectionHandler Instance { get; private set; }
    public Camera sceneCamera;
    private string loadedScene = null;

    private readonly List<string> namesToKeep = new List<string>
    {
        "SceneSelectionHandler",
        "UDPSignalSender",
        "[Debug Updater]"
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();

        if (sceneCamera != null)
        {
            sceneCamera.targetDisplay = 1;
            sceneCamera.enabled = false;
        }
    }

    public void SetSelectedScene(string sceneName)
    {
        Debug.Log("🟢 Chargement demandé : " + sceneName);
        StartCoroutine(LoadSceneOnDisplay2(sceneName));
    }

    private IEnumerator LoadSceneOnDisplay2(string sceneName)
    {
        // 🔄 Nettoyage de DontDestroyOnLoad AVANT le chargement de la nouvelle scène
        CleanDontDestroyOnLoad();

        // 🔻 Déchargement éventuel de la scène précédente
        if (!string.IsNullOrEmpty(loadedScene))
        {
            yield return SceneManager.UnloadSceneAsync(loadedScene);
        }

        // 📦 Chargement additif
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        loadedScene = sceneName;

        // 🎯 Configuration display 2
        Scene scene = SceneManager.GetSceneByName(sceneName);
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Camera cam in root.GetComponentsInChildren<Camera>())
            {
                cam.targetDisplay = 1;
                cam.enabled = true;
                Debug.Log("🎥 Caméra configurée sur Display 2 : " + cam.name);
            }

            foreach (Canvas canvas in root.GetComponentsInChildren<Canvas>())
            {
                canvas.targetDisplay = 1;
                Debug.Log("🖼️ Canvas redirigé sur Display 2 : " + canvas.name);
            }
        }

        // ⏳ Attendre une frame pour injection
        yield return null;

        // 🧠 Injection CollisionDetect dans le sender
        CollisionDetect cd = FindObjectOfType<CollisionDetect>();
        UDPSignalSender sender = FindObjectOfType<UDPSignalSender>();
        if (sender != null && cd != null)
        {
            sender.collisiondetect = cd;
            Debug.Log("✅ CollisionDetect injecté !");
        }
        else
        {
            Debug.LogWarning("⚠️ Injection de CollisionDetect échouée");
        }
    }

    private void CleanDontDestroyOnLoad()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad" && !namesToKeep.Contains(obj.name))
            {
                Debug.Log("🧹 Suppression de : " + obj.name);
                Destroy(obj);
            }
        }
    }
}
