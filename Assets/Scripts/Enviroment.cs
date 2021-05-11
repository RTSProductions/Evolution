using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public LayerMask ground;

    public float spawnRange = 10;

    public Spawner[] initialPopulations;

    public Camera camera;

    [Range(1, 100)]
    public float timeScale = 1;

    public Animal[] animals;

    public int bunnyPopulation;

    public int bearPopulation;

    public int wildBoarPopulation;

    public int foxPopulation;

    public int plantPopulation;

    public int wolfPopulation;

    [HideInInspector]
    public List<Animal> wolfs;
    [HideInInspector]
    public List<Animal> bunnys;
    [HideInInspector]
    public List<Animal> foxs;
    [HideInInspector]
    public List<Animal> plants;
    [HideInInspector]
    public List<Animal> wildBoars;
    [HideInInspector]
    public List<Animal> bears;

    private void Start()
    {
        SpawnInitialPopulations();
    }

    void SpawnInitialPopulations()
    {
        foreach (var pop in initialPopulations)
        {
            if (pop.willSpawn == true)
            {
                for (int i = 0; i < pop.population; i++)
                {
                    newSpawnPoint(pop.prefab, false);
                }
            }
        }
        Debug.Log("Life Begins.");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRange, spawnRange, spawnRange));
    }

    private void Update()
    {
        Camera[] cameras = FindObjectsOfType<Camera>();

        foreach (var cam in cameras)
        {
            if (cam != camera)
            {
                Destroy(cam.gameObject.GetComponent<AudioListener>());
            }
        }

        Time.timeScale = timeScale;

        animals = FindObjectsOfType<Animal>();

        if (animals.Length <= 0)
        {
            Debug.Log("All Life Must Come To An End.");
        }

        foreach(var ani in animals)
        {
            if (ani.species == Species.bear && !bears.Contains(ani))
            {
                bears.Add(ani);
            }
            else if (ani.species == Species.bunny && !bunnys.Contains(ani))
            {
                bunnys.Add(ani);
            }
            else if (ani.species == Species.fox && !foxs.Contains(ani))
            {
                foxs.Add(ani);
            }
            else if (ani.species == Species.plant && !plants.Contains(ani))
            {
                plants.Add(ani);
            }
            else if (ani.species == Species.wildBoar && !wildBoars.Contains(ani))
            {
                wildBoars.Add(ani);
            }
            else if (ani.species == Species.wolf && !wolfs.Contains(ani))
            {
                wolfs.Add(ani);
            }
        }

        bearPopulation = bears.Count;

        bunnyPopulation = bunnys.Count;

        foxPopulation = foxs.Count;

        plantPopulation = plants.Count;

        wildBoarPopulation = wildBoars.Count;

        wolfPopulation = wolfs.Count;

        if (bearPopulation <= 0)
        {
            Debug.Log("The bears have gone extinct");
        }

        if (bunnyPopulation <= 0)
        {
            Debug.Log("The bunnys have gone extinct");
        }

        if (foxPopulation <= 0)
        {
            Debug.Log("The foxs have gone extinct");
        }

        if (plantPopulation <= 0)
        {
            Debug.Log("The plants have gone extinct");
        }

        if (wildBoarPopulation <= 0)
        {
            Debug.Log("The wild boars have gone extinct");
        }

        if (wolfPopulation <= 0)
        {
            Debug.Log("The wild wolfs have gone extinct");
        }
    }

    public void SpawnPlant(GameObject prefab)
    {
        if (plantPopulation + 1 <= 1000)
        {
            float randomZ = UnityEngine.Random.Range(-spawnRange, spawnRange);
            float randomX = UnityEngine.Random.Range(-spawnRange, spawnRange);

            RaycastHit hit;

            Vector3 spawnPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if (Physics.Raycast(spawnPoint, -transform.up, out hit, ground))
            {
                var entity = Instantiate(prefab, hit.point, Quaternion.identity);

                entity.GetComponent<Animal>().young = true;
            }
            else
            {
                newSpawnPoint(prefab, true);
            }
        }
    }

    void newSpawnPoint(GameObject prefab, bool isYoung)
    {
        float randomZ = UnityEngine.Random.Range(-spawnRange, spawnRange);
        float randomX = UnityEngine.Random.Range(-spawnRange, spawnRange);

        RaycastHit hit;

        Vector3 spawnPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(spawnPoint, -transform.up, out hit, ground))
        {
            var entity = Instantiate(prefab, hit.point, Quaternion.identity);

            entity.GetComponent<Animal>().young = isYoung;
        }
        else
        {
            newSpawnPoint(prefab, isYoung);
        }
    }
}
[System.Serializable]
public class Spawner
{
    public string name;

    public bool willSpawn = true;

    public int population;

    public GameObject prefab;
}
