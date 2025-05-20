using UnityEngine;
using TMPro;

public class ClientRowHandler : MonoBehaviour
{
    public TMP_InputField inputNom;
    public TMP_InputField inputDistanceRoue;
    public TMP_InputField inputDistanceBras;
    public TMP_InputField inputMasse;

    public void OnSelectClient()
    {
        string nom = inputNom.text;
        string roue = inputDistanceRoue.text;
        string bras = inputDistanceBras.text;
        string masse = inputMasse.text;

        // Debug temporaire
        Debug.Log($"Client sélectionné : {nom}, Roue: {roue}, Bras: {bras}, Masse: {masse}");

        // Envoyer les données au gestionnaire central
        ClientSelectionHandler.Instance.SetSelectedClient(nom, roue, bras, masse);
    }
}
