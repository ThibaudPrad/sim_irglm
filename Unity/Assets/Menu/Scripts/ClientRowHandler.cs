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
    }

    void OnToggleChanged(bool isOn)
    {
        if (ignoreToggleEvent) return;

        if (isOn)
        {
            // Deselect previous
            if (ClientSelectionHandler.Instance.CurrentSelected != null &&
                ClientSelectionHandler.Instance.CurrentSelected != this)
            {
                ClientSelectionHandler.Instance.CurrentSelected.Deselect();
            }

            PatientData data = GetCurrentData();
            ClientSelectionHandler.Instance.SetSelectedClient(data, this);
        }
        else
        {
            if (ClientSelectionHandler.Instance.CurrentSelected == this)
            {
                ClientSelectionHandler.Instance.ClearSelection();
            }
        }
    }

    public void Deselect()
    {
        ignoreToggleEvent = true;
        toggleClient.isOn = false;
        ignoreToggleEvent = false;
    }

    public PatientData GetCurrentData()
    {
        PatientData data = new PatientData();
        data.nom = inputNom.text;
        float.TryParse(inputDistanceRoue.text, out data.distanceRoue);
        float.TryParse(inputDistanceBras.text, out data.distanceBras);
        float.TryParse(inputMasse.text, out data.masse);
        return data;
    }
}
