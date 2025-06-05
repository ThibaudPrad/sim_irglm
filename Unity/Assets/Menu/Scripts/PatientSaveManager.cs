using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PatientListData
{
    public List<PatientData> patients = new List<PatientData>();
}

public class PatientSaveManager : MonoBehaviour
{
    public static PatientSaveManager Instance { get; private set; }

    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        savePath = Path.Combine(Application.persistentDataPath, "patients.json");

        
        Debug.Log(savePath);
    }

    public void SavePatients(List<PatientData> list)
    {
        PatientListData wrapper = new PatientListData { patients = list };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Patients sauvegardés");
    }

    public List<PatientData> LoadPatients()
    {
        if (!File.Exists(savePath)) return new List<PatientData>();

        string json = File.ReadAllText(savePath);
        PatientListData wrapper = JsonUtility.FromJson<PatientListData>(json);
        return wrapper.patients;
    }
}
