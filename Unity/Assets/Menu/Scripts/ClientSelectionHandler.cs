using UnityEngine;

public class ClientSelectionHandler : MonoBehaviour
{
    public static ClientSelectionHandler Instance { get; private set; }

    public string SelectedNom { get; private set; }
    public string SelectedRoue { get; private set; }
    public string SelectedBras { get; private set; }
    public string SelectedMasse { get; private set; }

    public ClientRowHandler CurrentSelected { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SetSelectedClient(string nom, string roue, string bras, string masse, ClientRowHandler handler)
    {
        SelectedNom = nom;
        SelectedRoue = roue;
        SelectedBras = bras;
        SelectedMasse = masse;
        CurrentSelected = handler;

        Debug.Log($"Client sélectionné : {nom}, Roue: {roue}, Bras: {bras}, Masse: {masse}");
    }

    public void ClearSelection()
    {
        SelectedNom = null;
        SelectedRoue = null;
        SelectedBras = null;
        SelectedMasse = null;
        CurrentSelected = null;

        Debug.Log("Aucun client sélectionné");
    }
}
