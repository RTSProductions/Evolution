using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Animal : MonoBehaviour
{
    public Species species;

    float speed;

    Camera cam;

    NavMeshAgent agent;

    bool foundFood = false;

    public State state;

    float eatDuration = 10;

    bool foundMate = false;

    bool canReperduse = true;

    bool tooErgentForFood = false;

    float visonDistance = 10;

    Vector3 walkPoint;

    SphereCollider trigger;

    bool walkPointSet = false;

    float walkPointRange;

    float timeToDeathByHunger = 200;

    public float hunger;

    public LayerMask ground;

    float criticalPercent = 0.5f;

    [HideInInspector]
    public bool young = false;

    [EnumFlags]
    Species diet;

    Transform food;

    Transform mate;

    Transform preditor;

    [HideInInspector]
    public Genes motherGenes;

    [HideInInspector]
    public Genes FatherGenes;

    [HideInInspector]
    public bool didInherit = true;

    public Genes genes;

    // Start is called before the first frame update
    void Start()
    {
        

        if (species != Species.plant)
        {

            if (young == true)
            {
                transform.localScale = Vector3.one * 0.4f;
            }

            eatDuration = 20;

            trigger = this.gameObject.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            diet = genes.Diet;
            speed = genes.speed;
            visonDistance = genes.visonDistance;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
            walkPointRange = visonDistance * 4;
            trigger.radius = visonDistance;

            cam = Camera.main;

            genes.isMale = (Random.value < 0.5);

            if (didInherit == false && motherGenes != null && FatherGenes != null)
            {
                genes.InheritGenes(motherGenes, FatherGenes);

                didInherit = true;
            }
        }
        else
        {
            if (young == true)
            {
                transform.localScale = Vector3.zero;
            }

            criticalPercent = Random.Range(0.1f, 1);

            if (transform.position.y != -0.7f)
            {
                Vector3 newPos = new Vector3(transform.position.x, -0.7f, transform.position.z);

                transform.position = newPos;
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        criticalPercent = genes.criticalPercent;

        if (transform.localScale.y > Vector3.one.y)
        {
            transform.localScale = Vector3.one;
        }

        if (young == true)
        {
            if (transform.localScale.y < Vector3.one.y)
            {
                GrowUp();
            }
            if (transform.localScale == Vector3.one)
            {
                young = false;
            }
        }

        if (species == Species.plant)
        {
            if (transform.position.y != -0.7f)
            {
                Vector3 newPos = new Vector3(transform.position.x, -0.7f, transform.position.z);

                transform.position = newPos;
            }

            hunger += Time.deltaTime * 1 / timeToDeathByHunger;

            if (hunger >= criticalPercent)
            {
                Reperduse();
            }
        }
        if (species != Species.plant)
        {
            if (state == State.eating)
            {
                if (food != null)
                {
                    EatPlant();
                }
                else
                {
                    state = State.exploring;
                }
            }
            else if (state == State.waitingForMate)
            {
                if (food != null)
                {
                    transform.LookAt(mate.position);
                }
                else
                {
                    state = State.exploring;
                }
            }
           

            speed = genes.speed;
            visonDistance = genes.visonDistance;

            walkPointRange = visonDistance * 2;
            trigger.radius = visonDistance;

            agent.speed = speed;

            float hungerTime = speed / 6;

            hunger += Time.deltaTime * hungerTime / timeToDeathByHunger;
            

            //cool = Enviroment.allAnimals;
            if (food != null)
            {
                agent.SetDestination(food.position);

                foundFood = true;
            }
            else
            {
                foundFood = false;
            }
            if (preditor != null)
            {
                tooErgentForFood = true;
            }
            else
            {
                tooErgentForFood = false;
            }

            if (tooErgentForFood == true)
            {
                WalkRandom();

                float distace = Vector3.Distance(preditor.transform.position, transform.position);

                Genes animalGenes = preditor.GetComponent<Animal>().genes;

                if (distace <= 1 && genes.strength > animalGenes.strength)
                {
                    Camera cam = preditor.GetComponentInChildren<Camera>();

                    if (cam != null)
                    {
                        cam.transform.parent = null;

                        cam.GetComponent<FocusOnObject>().UnFocus();
                    }

                    Animal animal = preditor.GetComponent<Animal>();

                    if (diet.HasFlag(animal.species))
                    {
                        LiveEntety live = preditor.gameObject.GetComponent<LiveEntety>();

                        float eatAmount = Mathf.Min(hunger, Time.deltaTime * 1 / eatDuration);
                        eatAmount = live.Consume(eatAmount);
                        hunger -= eatAmount;

                        animal.food = null;

                        preditor = null;
                    }
                    else
                    {
                        LiveEntety live = preditor.gameObject.GetComponent<LiveEntety>();

                        live.Die(causeOfDeath.fighting);

                        preditor = null;
                    }


                    Animal preAnimal = preditor.GetComponent<Animal>();

                    if (preAnimal.species == Species.plant)
                    {
                        Enviroment enviroment = FindObjectOfType<Enviroment>();

                        enviroment.plants.Remove(preAnimal);
                    }
                    else if (preAnimal.species == Species.fox)
                    {
                        Enviroment enviroment = FindObjectOfType<Enviroment>();

                        enviroment.foxs.Remove(preAnimal);
                    }
                    else if (preAnimal.species == Species.wildBoar)
                    {
                        Enviroment enviroment = FindObjectOfType<Enviroment>();

                        enviroment.wildBoars.Remove(preAnimal);
                    }
                    else if (preAnimal.species == Species.bear)
                    {
                        Enviroment enviroment = FindObjectOfType<Enviroment>();

                        enviroment.bears.Remove(preAnimal);
                    }
                    else if (preAnimal.species == Species.bunny)
                    {
                        Enviroment enviroment = FindObjectOfType<Enviroment>();

                        enviroment.bunnys.Remove(preAnimal);
                    }
                    else if (preAnimal.species == Species.wolf)
                    {
                        Enviroment enviroment = FindObjectOfType<Enviroment>();

                        enviroment.wolfs.Remove(preAnimal);
                    }
                }
            }
            

            if (mate != null && mate.GetComponent<Animal>().mate == this.transform)
            {
                if (genes.isMale == true)
                {
                    agent.SetDestination(mate.position);
                }
                else
                {
                    agent.SetDestination(this.transform.position);

                    
                }


               

                foundMate = true;
            }
            else
            {
                foundMate = false;
            }

            if (foundFood == true)
            {
                state = State.goingToFood;

                float distace = Vector3.Distance(food.transform.position, transform.position);

                Genes animalGenes = food.GetComponent<Animal>().genes;

                if (distace <= 1 && genes.strength >= animalGenes.strength)
                {
                    LiveEntety live = food.gameObject.GetComponent<LiveEntety>();

                    bool isNotPlant = false;

                    Animal foodAnimal = food.GetComponent<Animal>();
                    if (foodAnimal.species == Species.plant)
                    {
                        state = State.eating;

                        agent.SetDestination(transform.position);
                    }
                    else
                    {
                        isNotPlant = true;
                    }
                    if (isNotPlant == true)
                    {


                        Camera cam = food.GetComponentInChildren<Camera>();

                        if (cam != null)
                        {
                            cam.transform.parent = null;

                            cam.GetComponent<FocusOnObject>().UnFocus();
                        }


                        if (foodAnimal.species == Species.plant)
                        {
                            Enviroment enviroment = FindObjectOfType<Enviroment>();

                            enviroment.plants.Remove(foodAnimal);
                        }
                        else if (foodAnimal.species == Species.fox)
                        {
                            Enviroment enviroment = FindObjectOfType<Enviroment>();

                            enviroment.foxs.Remove(foodAnimal);
                        }
                        else if (foodAnimal.species == Species.wildBoar)
                        {
                            Enviroment enviroment = FindObjectOfType<Enviroment>();

                            enviroment.wildBoars.Remove(foodAnimal);
                        }
                        else if (foodAnimal.species == Species.bear)
                        {
                            Enviroment enviroment = FindObjectOfType<Enviroment>();

                            enviroment.bears.Remove(foodAnimal);
                        }
                        else if (foodAnimal.species == Species.bunny)
                        {
                            Enviroment enviroment = FindObjectOfType<Enviroment>();

                            enviroment.bunnys.Remove(foodAnimal);
                        }
                        else if (foodAnimal.species == Species.wolf)
                        {
                            Enviroment enviroment = FindObjectOfType<Enviroment>();

                            enviroment.wolfs.Remove(foodAnimal);
                        }


                        live.Die(causeOfDeath.eaten);

                        hunger = 0;

                        food = null;
                    }
                }
            }

            if (foundMate == true)
            {
                if (genes.isMale == true)
                {
                    state = State.goingToMate;
                }
                else
                {
                    state = State.waitingForMate;
                }
               

                float distace = Vector3.Distance(mate.transform.position, transform.position);

                if (distace <= 1 || species == Species.bear && distace <= 3)
                {
                    LiveEntety live = mate.gameObject.GetComponent<LiveEntety>();

                    Reperduse();
                }
            }

            if (hunger >= 1)
            {
                Camera cam = GetComponentInChildren<Camera>();

                if (cam != null)
                {
                    cam.transform.parent = null;

                    cam.GetComponent<FocusOnObject>().UnFocus();
                }

                if (species == Species.plant)
                {
                    Enviroment enviroment = FindObjectOfType<Enviroment>();

                    enviroment.plants.Remove(this);
                }
                else if (this.species == Species.fox)
                {
                    Enviroment enviroment = FindObjectOfType<Enviroment>();

                    enviroment.foxs.Remove(this);
                }
                else if (this.species == Species.wildBoar)
                {
                    Enviroment enviroment = FindObjectOfType<Enviroment>();

                    enviroment.wildBoars.Remove(this);
                }
                else if (this.species == Species.bear)
                {
                    Enviroment enviroment = FindObjectOfType<Enviroment>();

                    enviroment.bears.Remove(this);
                }
                else if (this.species == Species.bunny)
                {
                    Enviroment enviroment = FindObjectOfType<Enviroment>();

                    enviroment.bunnys.Remove(this);
                }
                else if (this.species == Species.wolf)
                {
                    Enviroment enviroment = FindObjectOfType<Enviroment>();

                    enviroment.wolfs.Remove(this);
                }

                LiveEntety live = this.GetComponent<LiveEntety>();

                live.Die(causeOfDeath.hunger);
            }
            if (foundFood == false && tooErgentForFood == false && foundMate == false && hunger < criticalPercent && hunger > genes.reperductiveUrge || foundMate == false && hunger < genes.reperductiveUrge && tooErgentForFood == false && foundFood == false && canReperduse == false)
            {
                WalkRandom();

                state = State.exploring;
            }
            else if (foundFood == false && hunger >= criticalPercent && hunger > genes.reperductiveUrge && tooErgentForFood == false && foundMate == false)
            {
                WalkRandom();

                state = State.searchingForFood;
            }
            else if (foundMate == false && hunger < genes.reperductiveUrge && tooErgentForFood == false && foundFood == false && canReperduse == true)
            {
                WalkRandom();

                state = State.searchingForMate;
            }
            else if (tooErgentForFood == true)
            {
                //WalkRandom();

                agent.SetDestination(new Vector3(-preditor.transform.position.x, preditor.transform.position.y, -preditor.transform.position.z));

                state = State.runningFromPreditor;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (species != Species.plant)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(this.transform.position, visonDistance);
        }
    }

    void WalkRandom()
    {
        if (!walkPointSet)
        {
            SeachWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        float distace = Vector3.Distance(walkPoint, transform.position);
        if (distace <= visonDistance)
        {
            walkPointSet = false;
        }
    }

    void SeachWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
        {
            walkPointSet = true;

            StartCoroutine(WalkNewPlace());

            WalkRandom();
        }


    }

    void EatPlant()
    {
        LiveEntety foodLive = food.GetComponent<LiveEntety>();

        Animal foodAnimal = food.GetComponent<Animal>();

        bool willDie = false;

        bool wontDie = false;

        if (hunger >= foodLive.amountRemaining)
        {
            willDie = true;
        }
        else if (hunger < foodLive.amountRemaining)
        {
            wontDie = true;
        }

        agent.SetDestination(transform.position);

        

        float eatAmount = Mathf.Min(hunger, Time.deltaTime * 1 / eatDuration);
        float eat = eatAmount;
        eatAmount = foodLive.Consume(eat);

        hunger -= eatAmount;



        if (wontDie == true && hunger <= 0)
        {
            food = null;
        }
        if (willDie == true && foodLive.amountRemaining <= 0)
        {
            Enviroment enviroment = FindObjectOfType<Enviroment>();

            enviroment.plants.Remove(foodAnimal);

            food = null;
        }

    }

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.F) && species != Species.plant)
        {
            Focus();
        }
    }

    void Reperduse()
    {
        if (species != Species.plant && canReperduse == true)
        {
            if (genes.isMale == false)
            {
                Genes mateGenes = mate.GetComponent<Animal>().genes;

                for (int i = 0; i < genes.offSpringAmount; i++)
                {
                    GameObject baby = Instantiate(gameObject);

                    Destroy(baby.GetComponent<SphereCollider>());

                    canReperduse = false;

                    baby.GetComponent<Animal>();
                    Animal babyAnimal = baby.GetComponent<Animal>();

                    babyAnimal.motherGenes = genes;
                    babyAnimal.FatherGenes = mateGenes;

                    babyAnimal.StartCoroutine(CanGiveBirthAgain(60f));

                    babyAnimal.didInherit = false;

                    babyAnimal.young = true;

                    Debug.Log("New " + species + " baby!");
                }
            }

            StartCoroutine(CanGiveBirthAgain(120f));

        }
        else if (species == Species.plant && canReperduse == true)
        {
            FindObjectOfType<Enviroment>().SpawnPlant(this.gameObject);

            hunger = 0;
        }
    }

    public IEnumerator CanGiveBirthAgain(float time)
    {
        mate = null;

        canReperduse = false;

        yield return new WaitForSeconds(time);

        canReperduse = true;
    }

    void GrowUp()
    {
        LiveEntety live = GetComponent<LiveEntety>();

        float growAmount = Mathf.Min(0.5f, Time.deltaTime * 1 / eatDuration);

        live.GrowUp(growAmount);
    }

    IEnumerator WalkNewPlace()
    {
        walkPointSet = true;

        yield return new WaitForSeconds(3f);

        walkPointSet = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (species != Species.plant)
        {


            LiveEntety live = other.GetComponent<LiveEntety>();

            LiveEntety liveThis = this.GetComponent<LiveEntety>();

            if (live != null && live != liveThis)
            {
                Animal animal = other.GetComponent<Animal>();

                if (diet.HasFlag(live.species) && tooErgentForFood == false && foundFood == false && genes.reperductiveUrge < hunger && hunger >= criticalPercent)
                {
                    if (live.species == Species.plant)
                    {
                        if (animal.young == false)
                        {
                            food = other.transform;
                        }
                    }
                    food = other.transform;
                }
                else if (live.species == liveThis.species && genes.reperductiveUrge > hunger && animal.genes.reperductiveUrge > animal.hunger && canReperduse == true)
                {
                    if (animal.mate == null || animal.mate == this.transform)
                    {
                        if (genes.isMale == false && animal.genes.isMale == true || genes.isMale == true && animal.genes.isMale == false)
                        {
                            mate = other.transform;
                        }
                    }
                }
                else if (animal.diet.HasFlag(species))
                {
                    preditor = other.transform;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == food)
        {
            food = null;
        }
        else if (other.transform == preditor)
        {
            preditor = null;
        }
    }

    void Focus()
    {
        cam.GetComponent<FocusOnObject>().Focus(genes.speed, genes.isMale, hunger, genes.reperductiveUrge, this.transform);
    }

}

public enum State
{
    exploring = (1 << 0),
    goingToFood = (1 << 1),
    goingToMate = (1 << 2),
    waitingForMate = (1 << 3),
    searchingForFood = (1 << 4),
    searchingForMate = (1 << 5),
    runningFromPreditor = (1 << 6),
    eating = (1 << 7),
}