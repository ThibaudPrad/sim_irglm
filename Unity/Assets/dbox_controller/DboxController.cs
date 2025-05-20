using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dbox;
using System;


public class DboxController : MonoBehaviour
{


    public DboxStructs.FrameUpdate m_oFrameUpdate;
    private float positionY = 0;
    private double rotationX;
    private double rotationZ;


    const double simulator_width = 0.914;  // meters
    const double simulator_length = 0.914;  // meters
    const double dbox_max_excursion = 0.1524;  // meters
    const double max_dbox_pitch_command = 1;  // 0 to 1, 1 being no limit
    const double max_dbox_roll_command = 1;  // 0 to 1, 1 being no limit
    double max_pitch_angle = System.Math.Atan(dbox_max_excursion / simulator_length) / System.Math.PI * 180;  // deg
    double max_roll_angle = System.Math.Atan(dbox_max_excursion / simulator_width) / System.Math.PI * 180;  // deg

    // Start is called before the first frame update
    void Start()
    {


        DboxSdkWrapper.InitializeDbox();
        DboxSdkWrapper.OpenDbox();
        DboxSdkWrapper.ResetState();
        DboxSdkWrapper.StartDbox();

        DboxStructs.SimConfig m_oSimConfig;
        m_oSimConfig.MasterGain = 0;
        m_oSimConfig.MasterSpectrum = 0;
        DboxSdkWrapper.PostSimConfig(m_oSimConfig);

        m_oFrameUpdate.Heave = 0;
        DboxSdkWrapper.PostFrameUpdate(m_oFrameUpdate);
    }

    void SendRotationToDBox()
    {

        // // Pitch
        // rotationX = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x / max_pitch_angle;
        // if (rotationX > max_dbox_pitch_command) rotationX = max_dbox_pitch_command;
        // if (rotationX < -max_dbox_pitch_command) rotationX = -max_dbox_pitch_command;

        // // Roll
        // rotationZ = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).z / max_roll_angle;
        // if (rotationZ > max_dbox_roll_command) rotationZ = max_dbox_roll_command;
        // if (rotationZ < -max_dbox_roll_command) rotationZ = -max_dbox_roll_command;

        // //Debug.Log(transform.rotation.x + ";" + transform.localRotation.x + ";" + transform.eulerAngles.x + ";" + transform.localEulerAngles.x + ";" + transform.rotation.eulerAngles.x + ";" + transform.localRotation.eulerAngles.x + ";" + UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x);

        // m_oFrameUpdate.Pitch = (float)rotationX;
        // m_oFrameUpdate.Roll = (float)rotationZ;
        // m_oFrameUpdate.Heave = (float)positionY;
        
        // // Send structure information to D-BOX
        // DboxSdkWrapper.PostFrameUpdate(m_oFrameUpdate);

        // 1. Lire la rotation du GameObject en euler angles
        Vector3 euler = transform.rotation.eulerAngles;

        // 2. Extraire Pitch (X) et Roll (Z), et les normaliser
        double rotationX = NormalizeAngle(euler.x) / max_pitch_angle;
        double rotationZ = NormalizeAngle(euler.z) / max_roll_angle;

        // 3. Saturer les valeurs pour ne pas dépasser les limites physiques
        rotationX = Mathf.Clamp((float)rotationX, -1f, 1f);
        rotationZ = Mathf.Clamp((float)rotationZ, -1f, 1f);

        // 4. Mettre à jour la structure FrameUpdate
        m_oFrameUpdate.Pitch = (float)rotationX;
        m_oFrameUpdate.Roll = (float)rotationZ;
        m_oFrameUpdate.Heave = (float)positionY; // souvent 0, mais peut évoluer

        // 5. Envoyer à la D-BOX
        DboxSdkWrapper.PostFrameUpdate(m_oFrameUpdate);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SendRotationToDBox();
    }

    void OnDestroy()
    {
        DboxSdkWrapper.StopDbox();
        DboxSdkWrapper.CloseDbox();
        DboxSdkWrapper.TerminateDbox();
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
