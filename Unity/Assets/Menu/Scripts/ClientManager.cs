using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour
{
    public GameObject clientRowPrefab;
    public Transform clientListContent;

    private List<PatientData> patientList = new List<PatientData>();
    private int clientCounter;

    void Start()
    {
        // Chargement des patients sauvegardés
        patientList = PatientSaveManager.Instance.LoadPatients();
        foreach (var patient in patientList)
        {
            AddClientRow(patient);
        }
    }

    
    public void AddClientRow()
    {
        GameObject newRow = Instantiate(clientRowPrefab, clientListContent);
        ClientRowHandler handler = newRow.GetComponent<ClientRowHandler>();

        clientCounter++;

        PatientData data = new PatientData
        {
            nom = "",
            distanceRoue = 0f,
            distanceBras = 0f,
            masse = 0f
        };

        patientList.Add(data);
        PatientSaveManager.Instance.SavePatients(patientList);
    }

    public void AddClientRow(PatientData data)
    {
        GameObject newRow = Instantiate(clientRowPrefab, clientListContent);
        ClientRowHandler handler = newRow.GetComponent<ClientRowHandler>();

        handler.inputNom.text = data.nom;
        handler.inputDistanceRoue.text = data.distanceRoue.ToString();
        handler.inputDistanceBras.text = data.distanceBras.ToString();
        handler.inputMasse.text = data.masse.ToString();

        Toggle toggle = newRow.transform.Find("ToggleSelectClient").GetComponent<Toggle>();
        handler.toggleClient = toggle;

        clientCounter++;
    }

    public void RemoveClientRow()
    {
        if (clientListContent.childCount > 0)
        {
            Transform lastRow = clientListContent.GetChild(clientListContent.childCount - 1);
            Destroy(lastRow.gameObject);

            if (patientList.Count > 0)
            {
                patientList.RemoveAt(patientList.Count - 1);
                PatientSaveManager.Instance.SavePatients(patientList);
            }
        }
    }

    public void UpdatePatientListAndSave()
    {
        patientList.Clear();

        foreach (Transform child in clientListContent)
        {
            ClientRowHandler handler = child.GetComponent<ClientRowHandler>();
            if (handler != null)
            {
                patientList.Add(handler.GetCurrentData());
            }
        }

        PatientSaveManager.Instance.SavePatients(patientList);
    }
}
