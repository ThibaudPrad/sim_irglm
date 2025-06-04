using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDPSignalSender : MonoBehaviour
{
    private string ipAddress = "192.168.0.200";
    private int port = 25000;
    private UdpClient udpClient;

    public CollisionDetect collisiondetect;
    public double hardwareEnable = 0;
    public double wholeMass = 94.4;
    public double wheelDistance = 0.60;
    public bool forceReset = false;

    double previousFriction;
    bool previousCollisionFound;
    double previousHardwareEnable;
    double previousWholeMass;
    double previousWheelDistance;
    bool previousForceReset;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        try
        {
            udpClient = new UdpClient();
            hardwareEnable = 1;
            forceReset = true;
            Debug.Log("📡 UDPSignalSender initialisé.");
        }
        catch (SocketException e)
        {
            Debug.LogError("❌ Erreur d'ouverture du port UDP : " + e.Message);
        }
    }

    void Update()
    {
        if (collisiondetect == null) return;
        SendData();
    }

    void SendData()
    {
        if (udpClient == null || collisiondetect == null)
            return;

        double friction = collisiondetect.friction;
        bool collisionFound = collisiondetect.collisionfound;

        byte[] data = new byte[40];
        System.BitConverter.GetBytes(hardwareEnable).CopyTo(data, 0);
        System.BitConverter.GetBytes(friction).CopyTo(data, 8);
        System.BitConverter.GetBytes(collisionFound).CopyTo(data, 16);
        System.BitConverter.GetBytes(wholeMass).CopyTo(data, 20);
        System.BitConverter.GetBytes(wheelDistance).CopyTo(data, 28);
        System.BitConverter.GetBytes(forceReset).CopyTo(data, 36);

        if (previousFriction != friction || previousCollisionFound != collisionFound ||
            previousHardwareEnable != hardwareEnable || previousWholeMass != wholeMass ||
            previousWheelDistance != wheelDistance || previousForceReset != forceReset)
        {
            udpClient.Send(data, data.Length, ipAddress, port);

            previousFriction = friction;
            previousCollisionFound = collisionFound;
            previousHardwareEnable = hardwareEnable;
            previousWholeMass = wholeMass;
            previousWheelDistance = wheelDistance;
            previousForceReset = forceReset;

            Debug.Log($"📤 Données envoyées : Mass={wholeMass}, DistanceRoue={wheelDistance}");
        }
    }

    public void ForceSend()
    {
        SendData(); // 💥 Envoi immédiat déclenché depuis le toggle
    }

    void OnApplicationQuit()
    {
        if (udpClient == null) return;
        hardwareEnable = 0;
        forceReset = false;
        SendData();
    }

    void OnDestroy()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
