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

    public void SetSelectedClient(PatientData data, ClientRowHandler handler)
    {
        SelectedNom = data.nom;
        SelectedRoue = data.distanceRoue.ToString();
        SelectedBras = data.distanceBras.ToString();
        SelectedMasse = data.masse.ToString();
        CurrentSelected = handler;

        Debug.Log($" Client sélectionné : {data.nom}, Roue: {data.distanceRoue}, Bras: {data.distanceBras}, Masse: {data.masse}");

        ApplyPatientDataToScene(data);
    }

    private void ApplyPatientDataToScene(PatientData data)
    {
        GameObject wheelchair = GameObject.Find("UserWheelchair");
        if (wheelchair != null)
        {
            UDPSignalSender sender = wheelchair.GetComponent<UDPSignalSender>();
            if (sender != null)
            {
                sender.wheelDistance = data.distanceRoue;
                sender.wholeMass = data.masse;
                sender.ForceSend();
                Debug.Log("📤 Données du patient envoyées au UserWheelchair.");
            }
            else
            {
                Debug.LogWarning(" UDPSignalSender non trouvé sur UserWheelchair !");
            }
        }
        else
        {
            Debug.LogWarning(" UserWheelchair non trouvé dans la scène !");
        }
    }

    public void ApplySelectedClientToScene()
    {
        if (!HasSelectedClient()) return;

        PatientData data = GetSelectedClientData();
        ApplyPatientDataToScene(data);
    }

    public void ClearSelection()
    {
        SelectedNom = null;
        SelectedRoue = null;
        SelectedBras = null;
        SelectedMasse = null;
        CurrentSelected = null;

        Debug.Log(" Aucun client sélectionné");
    }

    public bool HasSelectedClient()
    {
        return !string.IsNullOrEmpty(SelectedNom);
    }

    public PatientData GetSelectedClientData()
    {
        if (!HasSelectedClient()) return null;

        PatientData data = new PatientData();
        data.nom = SelectedNom;
        float.TryParse(SelectedRoue, out data.distanceRoue);
        float.TryParse(SelectedBras, out data.distanceBras);
        float.TryParse(SelectedMasse, out data.masse);
        return data;
    }
}
