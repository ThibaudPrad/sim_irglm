using UnityEngine;

public class ForceResetButton : MonoBehaviour
{
    public UDPSignalSender udpSender;

    public void TriggerForceReset()
    {
        StartCoroutine(ResetFlagBriefly());
    }

    private System.Collections.IEnumerator ResetFlagBriefly()
    {
        udpSender.forceReset = true;

        // attendre 1 frame pour que SendData() ait le temps de l'envoyer
        yield return null;

        udpSender.forceReset = false;
    }
}
