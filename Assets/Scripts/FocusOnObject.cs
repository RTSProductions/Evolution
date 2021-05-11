using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityTemplateProjects;

public class FocusOnObject : MonoBehaviour
{
    public bool focused = false;


    public TextMeshProUGUI Urge;

    public TextMeshProUGUI male;

    public TextMeshProUGUI Speed;

    public TextMeshProUGUI state;

    public Slider Hunger;

    public GameObject stats;

    Animal currentAnimal;

    //public Quaternion goodRot = new Quaternion(0, 0, 0, 0);

    public Vector3 focusOffset;

    Transform animal;

    PlayerMovement movement;

    bool controlling = false;

    public SimpleCameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (animal == null && currentAnimal != null)
        {
            animal = currentAnimal.transform;
        }

        if (Input.anyKeyDown && !Input.GetKey(KeyCode.F) && !Input.GetMouseButton(0) && focused == true && controlling == false)
        {
            UnFocus();
        }

        if (currentAnimal != null)
        {
            Speed.text = "" + currentAnimal.genes.speed;

            Hunger.value = currentAnimal.hunger;

            Urge.text = "" + currentAnimal.genes.reperductiveUrge;

            if (currentAnimal.genes.isMale == true)
            {
                male.text = "Male";
            }
            else
            {
                male.text = "Female";
            }

            if (Hunger.value != currentAnimal.hunger)
            {
                currentAnimal.hunger = Hunger.value;
            }
            if (currentAnimal.state == State.exploring)
            {
                state.text = "Exploring";
            }
            else if (currentAnimal.state == State.goingToFood)
            {
                state.text = "Going To Food";
            }
            else if (currentAnimal.state == State.goingToMate)
            {
                state.text = "Going To Mate";
            }
            else if (currentAnimal.state == State.runningFromPreditor)
            {
                state.text = "Running From Preditor";
            }
            else if (currentAnimal.state == State.searchingForFood)
            {
                state.text = "Searching For Food";
            }
            else if (currentAnimal.state == State.searchingForMate)
            {
                state.text = "Searching For Mate";
            }
            else if (currentAnimal.state == State.waitingForMate)
            {
                state.text = "Waiting For Mate";
            }
            else if (currentAnimal.state == State.eating)
            {
                state.text = "Eating";
            }
        }

        if (focused == true && this.transform.parent != animal)
        {
            this.transform.parent = animal;
        }
        
        var goodRot = new Quaternion(0f, 0f, 0f, 0f);

        if (focused == true && this.transform.localPosition != focusOffset)
        {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, focusOffset, 2 * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, animal.position);

            transform.LookAt(animal.position);

            if (distance <= 20)
            {
                
            }
        }
        if (focused == true && this.transform.localRotation != goodRot && transform.localPosition == focusOffset)
        {
            this.transform.localRotation = Quaternion.Lerp(transform.localRotation, goodRot, 2 * Time.deltaTime);
        }
    }

    public void Focus(float speed, bool isMale, float HUNunger,float urge, Transform animal)
    {
        Hunger.value = HUNunger;

        //followPlayer.enabled = true;

        this.transform.parent = this.animal;

        cameraController.enabled = false;

      

        //followPlayer.player = animal;

        Speed.text = "" + speed;

        Urge.text = "" + urge;

        this.animal = animal;

        currentAnimal = animal.GetComponent<Animal>();

        if (isMale == true)
        {
            male.text = "Male";
        }
        else
        {
            male.text = "Female";
        }

        stats.SetActive(true);

        focused = true;
    }

    public void UnFocus()
    {
        cameraController.enabled = true;

        stats.SetActive(false);
        //followPlayer.enabled = false;

        focused = false;
    }
    public void TakeControll()
    {
        movement = animal.gameObject.AddComponent<PlayerMovement>();

        movement.Take();

        controlling = true;

        

        cameraController.enabled = false;
    }
}
