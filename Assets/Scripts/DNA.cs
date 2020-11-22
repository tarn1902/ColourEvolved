/*----------------------------------------
File Name: DNA.cs
Purpose: Controls this objects material
with genetic algorthim
Author: Tarn Cooper
Modified: 07 June 2020
------------------------------------------
Copyright 2020 Tarn Cooper.
-----------------------------------*/
using UnityEngine;
//Class for DNA
public class DNA : MonoBehaviour
{
    public Texture texture;
    public Shader shader;
    public Color colour;
    public Color targetColour;
    public float mutationRate = 50;
    public float changingSize = 0.5f;
    RaycastHit hit;
    RenderTexture rt;
    Renderer rend;

    //-----------------------------------------------------------
    // Gets renderer of object this script is attached to
    //-----------------------------------------------------------
    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    //-----------------------------------------------------------
    // Creates new material based on given public objects
    //-----------------------------------------------------------
    public void SetNewMaterial()
    {
        rend.material = new Material(shader);
        rend.material.mainTexture = texture;
        rend.material.color = colour;
    }

    //-----------------------------------------------------------
    // Changes colour of current material
    //-----------------------------------------------------------
    public void SetExistingMaterial()
    {
        rend.material.color = colour;
    }

    //-----------------------------------------------------------
    // Calculates fitness level
    //-----------------------------------------------------------
    public float CheckFitness()
    {
        return Mathf.Abs(targetColour.r - colour.r) + Mathf.Abs(targetColour.g - colour.g) + Mathf.Abs(targetColour.b - colour.b);
    }

    //-----------------------------------------------------------
    // Combines script data into a new one
    // partner1 (DNA): What is the first parent
    // partner2 (DNA): What is the second parent
    //-----------------------------------------------------------
    public void Breed(DNA partner1, DNA partner2)
    {
        //Crossbreed
        colour.r = Random.Range(0, 2) == 0 ? partner1.colour.r : partner2.colour.r;
        colour.g = Random.Range(0, 2) == 0 ? partner1.colour.g : partner2.colour.g;
        colour.b = Random.Range(0, 2) == 0 ? partner1.colour.b : partner2.colour.b;

        //Mutation
        if (mutationRate / 100 > Random.Range(0.0f, 1.0f))
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    {
                        colour.r += Random.Range(0, 2) == 0 ? changingSize : -changingSize;
                        break;
                    }
                case 1:
                    {
                        colour.g += Random.Range(0, 2) == 0 ? changingSize : -changingSize;
                        break;
                    }
                case 3:
                    {
                        colour.b += Random.Range(0, 2) == 0 ? changingSize : -changingSize;
                        break;
                    }
            }

        }
    }
}
