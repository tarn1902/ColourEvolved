/*----------------------------------------
File Name: GeneticAlgorthm.cs
Purpose: Creates and controls genetics of
DNA scripted objects
Author: Tarn Cooper
Modified: 07 June 2020
------------------------------------------
Copyright 2020 Tarn Cooper.
-----------------------------------*/
using System.Collections.Generic;
using UnityEngine;

//Class for genetic algorthm
public class GeneticAlgorthm : MonoBehaviour
{
    public int populationSize = 10;
    public GameObject prefab;
    List<GameObject> population;
    public float evolveWaitTime = 10;
    float evolveWaitingTime = 0;
    public int pixelAccuracy = 10;
    float lowestFitness = 3;
    float minBreedSize = 1.0f;
    List<GameObject> roulette;
    Texture2D texture;

    //-----------------------------------------------------------
    // Gets all components needed and creates population
    //-----------------------------------------------------------
    void Start()
    {
        population = new List<GameObject>();
        roulette = new List<GameObject>();
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        GameObject popObject;
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-4.0f, 4.0f), 0, Random.Range(-4.0f, 4.0f));
            popObject = Instantiate(prefab, pos, Quaternion.identity);
            popObject.GetComponent<DNA>().colour = new Color(1, 1, 1);
            popObject.GetComponent<DNA>().SetNewMaterial();
            population.Add(popObject);
        }
    }

    //-----------------------------------------------------------
    // Breeds population after certain amount of time
    //-----------------------------------------------------------
    void Update()
    {
        if (evolveWaitingTime >= evolveWaitTime)
        {
            BreedPopulation();
            evolveWaitingTime = 0;
        }
        evolveWaitingTime += Time.deltaTime;
    }

    //-----------------------------------------------------------
    // Breeds population together
    //-----------------------------------------------------------
    void BreedPopulation()
    {
        //Creates roulette
        float totalFitness = MakeRoulette();

        //Breeds two pop with roulette
        Breed();

        //Recalculates culling point
        if (totalFitness / populationSize < lowestFitness)
        {
            lowestFitness = totalFitness / populationSize;
            minBreedSize = (populationSize / totalFitness)/3;
        }

        roulette.Clear();
    }
    //-----------------------------------------------------------
    // Calculates average colour of screen to be used as target 
    // colour for pops
    //-----------------------------------------------------------
    private void OnPostRender()
    {
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        Color[] pixels = texture.GetPixels();
        Color totalPixels = Color.white;
        for (int i = 0; i < pixels.Length; i += pixelAccuracy)
        {
            totalPixels += pixels[i];
        }

        foreach (GameObject pop in population)
        {
            DNA dna = pop.GetComponent<DNA>();
            dna.targetColour = totalPixels / (pixels.Length / pixelAccuracy);
            dna.SetExistingMaterial();
        }
    }

    //-----------------------------------------------------------
    // Creates roultette with advantage towards most fit
    //-----------------------------------------------------------
    private float MakeRoulette()
    {
        //Create roulette
        float totalFitness = 0;
        foreach (GameObject pop in population)
        {
            float fitness = pop.GetComponent<DNA>().CheckFitness();
            totalFitness += fitness;
            while (fitness <= lowestFitness)
            {
                fitness += minBreedSize;
                roulette.Add(pop);
            }
            
        }
        return totalFitness;
    }

    //-----------------------------------------------------------
    // Breed to pop based on roulette
    //-----------------------------------------------------------
    private void Breed()
    {
        //Breed population
        if (roulette.Count != 0)
        {
            for (int i = 0; i < populationSize; i++)
            {
                population[i].GetComponent<DNA>().Breed(roulette[Random.Range(0, roulette.Count)].GetComponent<DNA>(), roulette[Random.Range(0, roulette.Count)].GetComponent<DNA>());
            }
        }
        else
        {
            lowestFitness += 0.1f;
        }
    }
}


