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

public class Navigation : MonoBehaviour
{

    public List<GameObject> waypoints;
    public static int curr;
    public float distance;

    public string devicePort = "COM4"; // Check which port is used on your system !
    private System.Threading.Timer callbackTimer;
    private Driver driver;
    private byte intensite = 150;
    private int duration = 1000;
    private int first = 0;


    // Start is called before the first frame update
    void Start()
    {
        curr = 0;
        // create a new driver instance
        driver = new Driver(devicePort);
        // create a new timer that will call the emitterCallback function every 40ms
        callbackTimer = new System.Threading.Timer(emitterCallback, null, 0, 40);
        play();

    }

    // Update is called once per frame
    void Update()
    {
        while(first<1)
        {
            play();
            first++;
        }
        distance = Vector3.Distance(transform.position, waypoints[curr].transform.position);
        //Debug.Log(distance);
        if (distance < 0.6  ) {
            curr++;
            play();
        }
    }

    public static int getCurr()
    {
        return curr;
    }

    // this function is called every 40ms
    private void emitterCallback(object state)
    {
        //driver.SetMessage(getDefaultMessage());
        driver.SendMessage();
    }

    public void play()
    {
        if (curr == 0)
        {
            playForward();
            Debug.Log("playForward");
        }
        else if (curr == 3 || curr == 8 || curr == 9 || curr == 10 || curr == 11 || curr == 13)
        {
            playLeft();
            Debug.Log("playLeft");
        }
        else if(curr == 1 || curr == 2 || curr == 4 || curr == 5 || curr == 6 || curr == 7 || curr == 12 || curr ==14)
        {
            playRight();
            Debug.Log("playRight");
        }
    }

    public async void playRight()
    {

        driver.SetMessage(new byte[5] { 0, 0, intensite, 0, Driver.EndMarker });
        await Task.Delay(duration);
        driver.SetMessage(getDefaultMessage());
        
    }
    public async void playLeft()
    {

        driver.SetMessage(new byte[5] { 0, intensite, 0, 0, Driver.EndMarker });
        await Task.Delay(duration);
        driver.SetMessage(getDefaultMessage());

    }

    public async void playForward()
    {

        driver.SetMessage(new byte[5] { 0, intensite, intensite, 0, Driver.EndMarker });
        await Task.Delay(duration);
        driver.SetMessage(getDefaultMessage());

    }

    // default message is a message with all motors at 0 + the end marker
    private byte[] getDefaultMessage()
    {
        return new byte[5] { 0, 0, 0, 0, Driver.EndMarker };
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(waypoints[curr+1].transform.position, transform.position);
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
        }
    }

}

// this is the editor script that will be used to display buttons in the inspector
[CustomEditor(typeof(Navigation))]
public class VibrationManagerEditor9 : Editor
{
    // instance is the object that is being edited/displayed
    private Navigation instance;
    private void OnEnable()
    {
        instance = (Navigation)target;
    }
}
