using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArretButton : MonoBehaviour
{
    public void EmergencyStopFromUI()
    {
        Debug.Log("ARRÊT déclenché depuis l'interface !");

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
