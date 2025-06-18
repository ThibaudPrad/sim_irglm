using UnityEngine;
using UnityEngine.UI;

public class InfoPanelHider : MonoBehaviour
{
    public GameObject infoPanel; // Le panel à masquer
    public Button okButton;      // Le bouton OK

    private void Start()
    {
        okButton.onClick.AddListener(() =>
        {
            infoPanel.SetActive(false);
        });
    }
}
