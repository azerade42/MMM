using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMatChange : MonoBehaviour
{
    public Material[] randomColorMats;
    void Start()
    {
        Material[] materials = GetComponent<Renderer>().materials;
        materials[1] = randomColorMats[Random.Range(0, randomColorMats.Length)];
        GetComponent<Renderer>().materials = materials;
    }

}
