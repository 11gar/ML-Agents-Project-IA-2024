using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvReplicator : MonoBehaviour
{
    public GameObject objectToReplicate; // The object to be replicated
    public int copiesX = 3;      // Number of copies to create
    public int copiesY = 3;      // Number of copies to create
    // public Vector3 offsetX;
    // public Vector3 offsetY;
    public Vector3 offsetX = new Vector3(30, 0, 0); // Remplacez "30" par une valeur correspondant à votre nouvelle échelle sur l'axe X
    public Vector3 offsetY = new Vector3(0, 0, 30); // Remplacez "30" par une valeur correspondant à votre nouvelle échelle sur l'axe Z


    void Start()
    {
        for (int i = 0; i < copiesX; i++)
        {
            for (int j = 0; j < copiesY; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                Vector3 position = transform.position + offsetX *3* i + offsetY * 3*j;
                Instantiate(objectToReplicate, position, Quaternion.identity);
            }
        }
    }
}
