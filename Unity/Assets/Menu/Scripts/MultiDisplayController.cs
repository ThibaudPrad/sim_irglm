using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MultiDisplayController : MonoBehaviour
{
    public Camera menuCamera;         // Caméra pour Display 1
    public Camera simulationCamera;   // Caméra pour Display 2

    void Start()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            Debug.Log("[MultiDisplayController] Display 2 activé");
        }

        if (menuCamera != null)
        {
            menuCamera.targetDisplay = 0;
            menuCamera.enabled = true;
        }

        // Lancer une coroutine pour attendre que la scène soit bien chargée
        StartCoroutine(WaitAndFindSimulationCamera());
    }

    IEnumerator WaitAndFindSimulationCamera()
    {
        // Attendre 0.5s que la scène additive soit bien instanciée
        yield return new WaitForSeconds(0.5f);

        if (simulationCamera == null)
        {
            // Recherche de la caméra de simulation (par exemple nommée "Main Camera")
            Camera[] allCameras = GameObject.FindObjectsOfType<Camera>();

            foreach (Camera cam in allCameras)
            {
                if (cam.name.Contains("Main") || cam.transform.root.name.Contains("UserWheelchair"))
                {
                    simulationCamera = cam;
                    simulationCamera.targetDisplay = 1;
                    simulationCamera.enabled = true;
                    Debug.Log("[MultiDisplayController] Caméra de simulation trouvée et assignée !");
                    break;
                }
            }

            if (simulationCamera == null)
            {
                Debug.LogWarning("[MultiDisplayController] Aucune caméra de simulation trouvée !");
            }
        }
    }
}
