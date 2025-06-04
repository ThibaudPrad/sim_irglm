using UnityEngine;

public class ClientSelectionHandler : MonoBehaviour
{
    public static ClientSelectionHandler Instance { get; private set; }

    public string SelectedNom { get; private set; }
    public string SelectedRoue { get; private set; }
    public string SelectedBras { get; private set; }
    public string SelectedMasse { get; private set; }

    public ClientRowHandler CurrentSelected { get; private set; }

    private UDPSignalSender udpSender; // 👉 glisse ici UserWheelchair dans l’inspecteur

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

        Debug.Log($"Client sélectionné : {data.nom}, Roue: {data.distanceRoue}, Bras: {data.distanceBras}, Masse: {data.masse}");

        if (udpSender == null)
        {
            GameObject wheelchair = GameObject.Find("UserWheelchair");
            if (wheelchair != null)
            {
                udpSender = wheelchair.GetComponent<UDPSignalSender>();
                Debug.Log("🔄 UDPSignalSender récupéré dynamiquement.");
            }
            else
            {
                Debug.LogWarning("⚠️ UserWheelchair non trouvé dans la scène !");
            }
        }

        if (udpSender != null)
        {
            udpSender.wheelDistance = data.distanceRoue;
            udpSender.wholeMass = data.masse;
            udpSender.ForceSend();
        }
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
