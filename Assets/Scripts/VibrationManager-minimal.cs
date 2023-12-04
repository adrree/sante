using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using hapticDriver;
using UnityEditor;
using UnityEngine;

public class VibrationManager_minimal : MonoBehaviour
{
    public string devicePort = "COM3"; // Check which port is used on your system !
    private Timer callbackTimer;
    private Driver driver;


    // Examples of values that can be edited in the inspector
    public int duration = 500;

    [Range(0, 255)]
    public float intensity = 200.0f;

    public bool M0, M1, M2, M3;


    public List<MyClass> sequence = new List<MyClass>(); // The elements type must be Serializable

    public int min = 0;
    public int max = 255;
    public float frequence = 0.002f;


    // the Start function is called when a script is enabled
    private void Start()
    {
        // create a new driver instance
        driver = new Driver(devicePort);
        // create a new timer that will call the emitterCallback function every 40ms
        callbackTimer = new Timer(emitterCallback, null, 0, 40);
    }

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

    public async void play()
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
    }

    public async void playSequence()
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            duration = sequence[i].duration;
            intensity = sequence[i].intensity;
            if (sequence[i].motor == 0)
            {
                playZero();
            }
            if (sequence[i].motor == 1)
            {
                playOne();
            }
            if (sequence[i].motor == 2)
            {
                playTwo();
            }
            if (sequence[i].motor == 3)
            {
                playThree();
            }
            await Task.Delay((int)sequence[i].delay);
        }
    }

    public async void playLineaire()
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

    }

    public async void playSinus()
    {
        float tdeb = Time.time;
        float value = 0;

        byte intensity0 = 0;
        byte intensity1 = 0;
        byte intensity2 = 0;
        byte intensity3 = 0;

        while (Time.time < tdeb + duration / 1000)
        {

            value = Mathf.Sin(2 * Mathf.PI * frequence * (Time.time - tdeb)) * intensity;
            if (value < 0) value = -value;

            if (M0) intensity0 = (byte)value;
            if (M1) intensity1 = (byte)value;
            if (M2) intensity2 = (byte)value;
            if (M3) intensity3 = (byte)value;

            driver.SetMessage(new byte[5] { intensity0, intensity1, intensity2, intensity3, Driver.EndMarker });
            await Task.Delay(40);
        }
        driver.SetMessage(getDefaultMessage());
    }
}


// this is the editor script that will be used to display buttons in the inspector
[CustomEditor(typeof(VibrationManager_minimal))]
public class VibrationManagerEditor : Editor
{
    // instance is the object that is being edited/displayed
    private VibrationManager_minimal instance;
    private void OnEnable()
    {
        instance = (VibrationManager_minimal)target;
    }

    // this function is called when the inspector is drawn
    // this is where we can add buttons
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // draw the default inspector

        /*       GUILayout.BeginHorizontal();

               // Button example
               if (GUILayout.Button("Play 0"))
               {
                   Debug.Log("Button pressed");
                   instance.playZero();
               }
               if (GUILayout.Button("Play 1"))
               {
                   Debug.Log("Button pressed");
                   instance.playOne();
               }
               if (GUILayout.Button("Play 2"))
               {
                   Debug.Log("Button pressed");
                   instance.playTwo();
               }
               if (GUILayout.Button("Play 3"))
               {
                   Debug.Log("Button pressed");
                   instance.playThree();
               }

               GUILayout.EndHorizontal();
        */

        if (GUILayout.Button("Play")) instance.play();
        if (GUILayout.Button("Play Sequence")) instance.playSequence();
        if (GUILayout.Button("Play Lineaire")) instance.playLineaire();
        if (GUILayout.Button("Play Sinus")) instance.playSinus();
    }
}

[Serializable]
public class MyClass
{
    public int motor;
    public int intensity;
    public int duration;
    public int delay;
}