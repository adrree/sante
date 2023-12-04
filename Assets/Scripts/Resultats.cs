using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class Resultats : MonoBehaviour
{
    public string path = "/Resources/";
    public string filename = "resulats";

    private float timer = 0f;

    StringBuilder sb = new System.Text.StringBuilder();


    // Start is called before the first frame update
    void Start()
    {
        List<String> colonnes = new List<String>();
        colonnes.Add("heure et date");
        colonnes.Add("nombre de collisions lointaines");
        colonnes.Add("nombre de collisions proches");
        colonnes.Add("nombre de balises atteintes");
        colonnes.Add("parcourt reussi");
        colonnes.Add("temps total (en s)");
        addColnames(colonnes);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void addColnames(List<String> colnames)
    {
        sb.AppendLine(String.Join(";", colnames));
    }

    public void record(List<String> values)
    {
        sb.AppendLine(String.Join(";", values));
        SaveToFile(sb.ToString());
    }

    public void SaveToFile(string content)
    {
        var folder = Application.dataPath + path;
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);


        var filePath = Path.Combine(folder, filename + ".csv");

        using (var writer = new StreamWriter(filePath, true))
        {
            writer.Write(content);
        }
    }

    public void OnDestroy()
    {
        List<String> resultats = new List<String>();
        resultats.Add(string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now));
        resultats.Add(string.Format("{0}", Collision_D1.getCollisions()+Collision_G1.getCollisions()+Collision_M1.getCollisions()));
        resultats.Add(string.Format("{0}", Collision_D2.getCollisions() + Collision_G2.getCollisions() + Collision_M2.getCollisions()));
        resultats.Add(string.Format("{0}", Navigation.getCurr()));
        if(Navigation.getCurr() == 15)
        {
            resultats.Add("oui");
        }
        else
        {
            resultats.Add("non");
        }
        resultats.Add(timer.ToString());
        record(resultats);
    }
}
