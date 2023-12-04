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

public class Collisions_d1 : MonoBehaviour
{
    public string devicePort = "COM4"; // Check which port is used on your system !
    private System.Threading.Timer callbackTimer;
    private Driver driver;


    // Examples of values that can be edited in the inspector
    public int duration = 500;

    [Range(0, 255)]
    public float intensity = 200.0f;

    public bool M0, M1, M2, M3;

    public int min = 0;
    public int max = 255;
    public float frequence = 0.002f;

    // the Start function is called when a script is enabled
    private void Start()
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

    // Cette méthode est appelée lorsqu'il y a une collision avec un autre objet
    void OnTriggerStay(UnityEngine.Collider collision)
    {
        // Vérifier si la collision concerne l'environnement
        if (collision.gameObject.CompareTag("Evironnement"))
        {
            Debug.Log("Collision avec l'environnement détectée !");
            playZero();
        }
    }


    //



    

    // this function is called every 40ms
    private void emitterCallback(object state)
    {
        //driver.SetMessage(getDefaultMessage());
        driver.SendMessage();
    }

    // default message is a message with all motors at 0 + the end marker
    private byte[] getDefaultMessage()
    {
        return new byte[5] { 0, 0, 0, 0, Driver.EndMarker };
    }

    // example of a function that will play a vibration on one motor
    // this function is asynchronous, meaning that it will not block the main thread
    public async void playZero()
    {
        driver.SetMessage(new byte[5] { (byte)intensity, 0, 0, 0, Driver.EndMarker });
        await Task.Delay((int)duration);
        driver.SetMessage(getDefaultMessage());
    }

    public async void playOne()
    {
        driver.SetMessage(new byte[5] { 0, (byte)intensity, 0, 0, Driver.EndMarker });
        await Task.Delay((int)duration);
        driver.SetMessage(getDefaultMessage());
    }

    public async void playTwo()
    {
        driver.SetMessage(new byte[5] { 0, 0, (byte)intensity, 0, Driver.EndMarker });
        await Task.Delay((int)duration);
        driver.SetMessage(getDefaultMessage());
    }

    public async void playThree()
    {
        driver.SetMessage(new byte[5] { 0, 0, 0, (byte)intensity, Driver.EndMarker });
        await Task.Delay((int)duration);
        driver.SetMessage(getDefaultMessage());
    }

    /*public async void play()
    {
        byte intensity0 = 0;
        byte intensity1 = 0;
        byte intensity2 = 0;
        byte intensity3 = 0;

        if (M0) intensity0 = (byte)intensity;
        if (M1) intensity1 = (byte)intensity;
        if (M2) intensity2 = (byte)intensity;
        if (M3) intensity3 = (byte)intensity;

        driver.SetMessage(new byte[5] { intensity0, intensity1, intensity2, intensity3, Driver.EndMarker });
        await Task.Delay((int)duration);
        driver.SetMessage(getDefaultMessage());
    }*/

    /*public async void playLineaire()
    {
        int steps = duration / 40;
        int increment = (max - min) / steps;
        byte intensity0 = 0;
        byte intensity1 = 0;
        byte intensity2 = 0;
        byte intensity3 = 0;

        if (M0) intensity0 = (byte)min;
        if (M1) intensity1 = (byte)min;
        if (M2) intensity2 = (byte)min;
        if (M3) intensity3 = (byte)min;

        for (int i = 0; i < steps; i++)
        {
            if (M0) intensity0 += (byte)increment;
            if (M1) intensity1 += (byte)increment;
            if (M2) intensity2 += (byte)increment;
            if (M3) intensity3 += (byte)increment;

            driver.SetMessage(new byte[5] { intensity0, intensity1, intensity2, intensity3, Driver.EndMarker });
            await Task.Delay(40);
            driver.SetMessage(getDefaultMessage());
        }

    }*/

    public async void playSinus()
    {
        float tdeb = Time.time;
        float value = 0;

        while (Time.time < tdeb + duration / 1000)
        {

            value = Mathf.Sin(2 * Mathf.PI * frequence * (Time.time - tdeb)) * intensity;
            if (value < 0) value = -value;
            byte intensity0 = (byte)value;

            // On n'active que le moteur 1
            driver.SetMessage(new byte[5] { intensity0, 0, 0, 0, Driver.EndMarker });
            await Task.Delay(40);
        }
        driver.SetMessage(getDefaultMessage());
    }

}

// this is the editor script that will be used to display buttons in the inspector
[CustomEditor(typeof(Collisions_d1))]
public class VibrationManagerEditor2 : Editor
{
    // instance is the object that is being edited/displayed
    private Collisions_d1 instance;
    private void OnEnable()
    {
        instance = (Collisions_d1)target;
    }
}