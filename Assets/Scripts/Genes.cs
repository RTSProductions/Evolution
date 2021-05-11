using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

[System.Serializable]
public class Genes
{
    [EnumFlags]
    public Species Diet;

    static readonly System.Random prng = new System.Random();

    int momDad = 0;

    int mutation = 0;

    int upDown = 0;

    public float speed = 6;

    public float strength = 1;

    public int offSpringAmount = 1;

    public float criticalPercent = 0.5f;

    public Genes(float[] values)
    {
        isMale = RandomValue() < 0.5f;
    }

    static float RandomValue()
    {
        return (float)prng.NextDouble();
    }

    public bool isMale;

    public float visonDistance = 20;

    public float reperductiveUrge = 0.02f;

    public void randomGenes(float main, float momGenes, float dadGenes)
    {
        momDad = Random.Range(1, 10);

        if (momDad <=  5)
        {
            main = dadGenes;

            mutation = Random.Range(1, 10);

            if (mutation >= 7)
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
            else
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
        }
        else
        {
            main = momGenes;

            mutation = Random.Range(1, 10);

            if (mutation >= 7)
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
            else
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
        }
    }

    public void randomGenesLow(float main, float momGenes, float dadGenes)
    {
        momDad = Random.Range(1, 10);

        if (momDad <= 5)
        {
            main = dadGenes;

            mutation = Random.Range(1, 10);

            if (mutation >= 7)
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(0, 0.7f);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(0, 0.7f);

                    main += upDown2;
                }
            }
            else
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(0, 0.7f);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(0, 0.7f);

                    main += upDown2;
                }
            }
        }
        else
        {
            main = momGenes;

            mutation = Random.Range(1, 10);

            if (mutation >= 7)
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
            else
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    float upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    float upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
        }
    }

    public void randomGenesInt(int main, int momGenes, int dadGenes)
    {
        momDad = Random.Range(1, 10);

        if (momDad <= 5)
        {
            main = dadGenes;

            mutation = Random.Range(1, 10);

            if (mutation >= 7)
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    int upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    int upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
            else
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    int upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    int upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
        }
        else
        {
            main = momGenes;

            mutation = Random.Range(1, 10);

            if (mutation >= 7)
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    int upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    int upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
            else
            {
                upDown = Random.Range(1, 2);

                if (upDown <= 1)
                {
                    int upDown2 = Random.Range(1, 6);

                    main -= upDown2;
                }
                else
                {
                    int upDown2 = Random.Range(1, 6);

                    main += upDown2;
                }
            }
        }
    }


    public void InheritGenes(Genes mother, Genes father)
    {
        randomGenes(speed, mother.speed, father.speed);

        randomGenes(visonDistance, mother.visonDistance, father.visonDistance);

        randomGenes(visonDistance, mother.visonDistance, father.visonDistance);

        randomGenes(strength, mother.strength, father.strength);

        randomGenesLow(criticalPercent, mother.criticalPercent, father.criticalPercent);

        randomGenesInt(offSpringAmount, mother.offSpringAmount, father.offSpringAmount);

        if (strength <= 0)
        {
            strength = 1;
        }
        if (speed <= 0)
        {
            speed = 1;
        }
        if (visonDistance <= 2)
        {
            visonDistance = 2;
        }
        if (reperductiveUrge <= 0)
        {
            reperductiveUrge = 0.01f;
        }
        if (offSpringAmount <= 0)
        {
            offSpringAmount = 1;
        }
    }
}

