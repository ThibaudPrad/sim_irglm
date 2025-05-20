using UnityEngine;

public class ClientSelectionHandler : MonoBehaviour
{
    public static ClientSelectionHandler Instance { get; private set; }

    public string SelectedNom { get; private set; }
    public string SelectedRoue { get; private set; }
    public string SelectedBras { get; private set; }
    public string SelectedMasse { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SetSelectedClient(string nom, string roue, string bras, string masse)
    {
        SelectedNom = nom;
        SelectedRoue = roue;
        SelectedBras = bras;
        SelectedMasse = masse;

        Debug.Log($"[ClientSelectionHandler] Client actif : {nom}");
    }
}
