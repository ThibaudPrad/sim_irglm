using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientManager : MonoBehaviour
{
    public GameObject clientRowPrefab; // Drag & drop ClientRow prefab
    public Transform clientListContent; // Drag & drop LeftPanel or content container
    private int clientCounter; // Counter for client rows

    public void AddClientRow()
    {
        GameObject newRow = Instantiate(clientRowPrefab, clientListContent);

        TMP_InputField[] inputs = newRow.GetComponentsInChildren<TMP_InputField>();
        
        // 🔍 On récupère le handler de cette ligne
        ClientRowHandler handler = newRow.GetComponent<ClientRowHandler>();

        // ✅ Étape importante : retrouver le bouton de sélection dans cette ligne
        Button selectButton = newRow.transform.Find("ButtonSelectClient").GetComponent<Button>();
        selectButton.onClick.AddListener(handler.OnSelectClient);


    clientCounter++;
    }

    public void RemoveClientRow()
    {
        if (clientListContent.childCount > 0)
        {
            Transform lastRow = clientListContent.GetChild(clientListContent.childCount - 1);
            Destroy(lastRow.gameObject);
        }
    }
}

