using System;
using System.Collections;
using System.Collections.Generic;
using hapticDriver;
using UnityEditor;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class Collision_M2 : MonoBehaviour
{
    public string devicePort = "COM4"; // Check which port is used on your system !
    private System.Threading.Timer callbackTimer;
    private Driver driver;

    private int duration = 500;
    private int min = 0;
    private int max = 255;
    private static int nbr_collisions = 0;


    // Start is called before the first frame update
    void Start()
    {
        // create a new driver instance
        driver = new Driver(devicePort);
        // create a new timer that will call the emitterCallback function every 40ms
        callbackTimer = new System.Threading.Timer(emitterCallback, null, 0, 40);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static int getCollisions()
    {
        return nbr_collisions;
    }

    // Cette méthode est appelée lorsqu'il y a une collision avec un autre objet
    void OnTriggerStay(UnityEngine.Collider collision)
    {
        // Vérifier si la collision concerne l'environnement
        if (collision.gameObject.CompareTag("Evironnement"))
        {
            //Debug.Log("Collision proche devant détectée !");
            playLineaire();
            nbr_collisions++;
        }
    }

    // this function is called every 40ms
    private void emitterCallback(object state)
    {
        //driver.SetMessage(getDefaultMessage());
        driver.SendMessage();
    }

    public async void playLineaire()
    {
        int steps = duration / 40;
        int increment = (max - min) / steps;
        byte intensity0 = (byte)min;

        for (int i = 0; i < steps; i++)
        {
            intensity0 += (byte)increment;

            driver.SetMessage(new byte[5] { intensity0, 0, 0, intensity0, Driver.EndMarker });
            await Task.Delay(40);
            driver.SetMessage(getDefaultMessage());
        }
    }

    // default message is a message with all motors at 0 + the end marker
    private byte[] getDefaultMessage()
    {
        return new byte[5] { 0, 0, 0, 0, Driver.EndMarker };
    }
}

// this is the editor script that will be used to display buttons in the inspector
[CustomEditor(typeof(Collision_M2))]
public class VibrationManagerEditor8 : Editor
{
    // instance is the object that is being edited/displayed
    private Collision_M2 instance;
    private void OnEnable()
    {
        instance = (Collision_M2)target;
    }
}
