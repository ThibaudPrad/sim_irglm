using UnityEngine;
using UnityEngine.UI;

public class InfoPanelShower : MonoBehaviour
{
    public GameObject infoPanel;
    public Button showButton;

    private void Start()
    {
        showButton.onClick.AddListener(() =>
        {
            infoPanel.SetActive(true);
        });
    }
}
