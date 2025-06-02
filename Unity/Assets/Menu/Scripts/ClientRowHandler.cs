using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClientRowHandler : MonoBehaviour
{
    public TMP_InputField inputNom;
    public TMP_InputField inputDistanceRoue;
    public TMP_InputField inputDistanceBras;
    public TMP_InputField inputMasse;
    public Toggle toggleClient;

    private bool ignoreToggleEvent = false;

    void Start()
    {
        toggleClient.onValueChanged.AddListener(OnToggleChanged);

        inputNom.onEndEdit.AddListener(delegate { UpdateAndSave(); });
        inputDistanceRoue.onEndEdit.AddListener(delegate { UpdateAndSave(); });
        inputDistanceBras.onEndEdit.AddListener(delegate { UpdateAndSave(); });
        inputMasse.onEndEdit.AddListener(delegate { UpdateAndSave(); });
    }

    void OnToggleChanged(bool isOn)
    {
        if (ignoreToggleEvent) return;

        if (isOn)
        {
            // Si on sélectionne un nouveau client : désélectionner l'ancien
            if (ClientSelectionHandler.Instance.CurrentSelected != null &&
                ClientSelectionHandler.Instance.CurrentSelected != this)
            {
                ClientSelectionHandler.Instance.CurrentSelected.Deselect();
            }

            string nom = inputNom.text;
            string roue = inputDistanceRoue.text;
            string bras = inputDistanceBras.text;
            string masse = inputMasse.text;

            ClientSelectionHandler.Instance.SetSelectedClient(nom, roue, bras, masse, this);
        }
        else
        {
            // Si on désélectionne ce client, on vérifie s’il était sélectionné
            if (ClientSelectionHandler.Instance.CurrentSelected == this)
            {
                ClientSelectionHandler.Instance.ClearSelection();
            }
        }
    }

    public PatientData GetData()
    {
        PatientData data = new PatientData();

        data.nom = inputNom.text;

        float.TryParse(inputDistanceRoue.text, out data.distanceRoue);
        float.TryParse(inputDistanceBras.text, out data.distanceBras);
        float.TryParse(inputMasse.text, out data.masse);

        return data;
    }

    void UpdateAndSave()
{
    ClientManager manager = FindObjectOfType<ClientManager>();
    manager.UpdatePatientListAndSave();
}


    public void Deselect()
    {
        ignoreToggleEvent = true;
        toggleClient.isOn = false;
        ignoreToggleEvent = false;
    }
}
