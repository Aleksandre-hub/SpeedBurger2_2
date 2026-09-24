using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButgerOrderManager : MonoBehaviour
{

    public GameObject[] Ingredients;
    public GameObject[] SpawnedIngredients = new GameObject[6];
    public GameObject[] animationObjects = new GameObject[7];
    public GameObject[] AnimObjs = new GameObject[7];

    [SerializeField] private GameObject[] ParentObjects;
    public bool nextLevel = true;
    private int randomNumber;
    private int[] randomNumArray = new int[4];


    private int oneKotletRule = 0;

    public bool[] chekced;
    public int checkingOrder = 0;

    public int burgerSize = 0;

    public bool timeToSet = false;

    public bool sheityepa = true;

    public bool daburgereba = false;

    public bool slideable = false;


    int k = 0;

//kotleti = 4

    public static ButgerOrderManager Instance;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < chekced.Length; i++) {
            chekced[i] = false;
        }
 
        randomNumber = Random.Range(1,5);
        randomNumArray[0] = randomNumber;
    }

    // Update is called once per frame
    void Update()
    {

        if(daburgereba) {
            for (int i = 0; i < SpawnedIngredients.Length; i++) {
                if (SpawnedIngredients[i] != null) {
                    if (i < SpawnedIngredients.Length/2) {
                        SpawnedIngredients[i].transform.position = Vector2.MoveTowards(SpawnedIngredients[i].transform.position, new Vector2(SpawnedIngredients[i+1].transform.position.x, SpawnedIngredients[i+1].transform.position.y + 0.3f), 0.09f);
                    }else if (i > SpawnedIngredients.Length/2) {
                        SpawnedIngredients[i].transform.position = Vector2.MoveTowards(SpawnedIngredients[i].transform.position, new Vector2(SpawnedIngredients[i-1].transform.position.x, SpawnedIngredients[i-1].transform.position.y - 0.3f), 0.09f);
                    }
                }
            }
        }
        

        if(nextLevel) {
            timeToSet = true;
            

            FindObjectOfType<AudioManager>().Play("BurgerMade");

            for (int i = 0; i < SpawnedIngredients.Length; i++) {
                
                if (SpawnedIngredients[i] != null) {

                    //StartCoroutine(Burgerdeb());
                    
                    Destroy(SpawnedIngredients[i], 1f);
                    StartCoroutine(Daburgerda());
                }

            }


            if (!daburgereba) {

                for (int i = 0; i < SpawnedIngredients.Length; i++) {
                    if(slideable) {
                        SpawnedIngredients[i].GetComponent<Animator>().Play("winSlide");
                    }
                }
                slideable = false;
            
                burgerSize = Random.Range(3, 7);
                SpawnedIngredients = new GameObject[burgerSize + 1];

                for (int i = 0; i < AnimObjs.Length; i++) {
                
                    if (AnimObjs[i] != null) {
                        Destroy(AnimObjs[i], 0.3f);
                    }

                }

                for (int i = 0; i < AnimObjs.Length; i++) {
                    AnimObjs[i] = Instantiate(animationObjects[i], ParentObjects[i].transform.position, Quaternion.identity, ParentObjects[i].transform.parent);
                }

                SpawnedIngredients[burgerSize] = Instantiate(Ingredients[6], ParentObjects[burgerSize].transform.position, Quaternion.identity, ParentObjects[5].transform.parent);
                SpawnedIngredients[burgerSize].GetComponent<Animator>().Play("spawnSlide");

                Debug.Log("sheityepa");
                oneKotletRule = Random.Range(1,burgerSize);
                for (int i = burgerSize-1; i > 0; i--) {
                    
                    // Debug.Log(i);
                    // Debug.Log("burger size: " + burgerSize);
                    // Debug.Log("spawned ingredient size: " + SpawnedIngredients.Length);

                    
                
                    if (i == oneKotletRule) {
                        SpawnedIngredients[i] = Instantiate(Ingredients[4], ParentObjects[i].transform.position, Quaternion.identity, ParentObjects[i].transform.parent);
                        SpawnedIngredients[i].GetComponent<Animator>().Play("spawnSlide");
                        randomNumber = Random.Range(1,6);
                    }else  {
                        SpawnedIngredients[i] = Instantiate(Ingredients[randomNumber], ParentObjects[i].transform.position, Quaternion.identity, ParentObjects[i].transform.parent);
                        SpawnedIngredients[i].GetComponent<Animator>().Play("spawnSlide");
                        randomNumber = Random.Range(1,6);
                    }

                    
                }

                SpawnedIngredients[0] = Instantiate(Ingredients[0], ParentObjects[0].transform.position, Quaternion.identity, ParentObjects[0].transform.parent);
                SpawnedIngredients[0].GetComponent<Animator>().Play("spawnSlide");
                
                nextLevel = false;
            
            }
        }

    }

    public IEnumerator Daburgerda() {
        yield return new WaitForSeconds(0.4f);
        daburgereba = false;
        slideable = true;
        StopCoroutine(Daburgerda());
    }

}
