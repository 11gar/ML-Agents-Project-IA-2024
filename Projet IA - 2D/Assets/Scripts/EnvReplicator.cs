using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvReplicator : MonoBehaviour
{
    public GameObject objectToReplicate; // The object to be replicated
    public int copiesX = 3;      // Number of copies to create
    public int copiesY = 3;      // Number of copies to create
    private Vector3 offsetX = new Vector3(25 / 2, 0, 0); // Spacing between objects
    private Vector3 offsetY = new Vector3(0, 25 / 2, 0); // Spacing between objects

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
                Vector3 position = transform.position + offsetX * i + offsetY * j;
                Instantiate(objectToReplicate, position, Quaternion.identity);
            }
        }
    }
}
