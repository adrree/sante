using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Cette m�thode est appel�e lorsqu'il y a une collision avec un autre objet
    void OnTriggerEnter(UnityEngine.Collider collision)
    {
        // V�rifier si la collision concerne l'environnement
        if (collision.gameObject.CompareTag("Evironnement"))
        {
            Debug.Log("Collision avec l'environnement d�tect�e !");

            // Vous pouvez ajouter ici le code que vous souhaitez ex�cuter en cas de collision avec l'environnement
        }
    }

}
