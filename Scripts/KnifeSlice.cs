using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSlice : MonoBehaviour
{
    // Prefabs for banana and carrot slices
    public GameObject bananaPrefab;
    public GameObject[] bananaSlicePrefabs;
    public GameObject carrotPrefab;
    public GameObject[] carrotSlicePrefabs;

    // Parameters for slicing and movement
    public float moveDistance = 1f;
    public float sliceGap = 0.5f;
    public float slideSpeed = 1f;

    // Audio and UI elements
    public AudioSource audioSource;
    public AudioClip cuttingSound;
    public GameObject arrowUIForA;
    public GameObject arrowUIForD;

    // Private variables for tracking slicing
    private int currentSlice = 0;
    private int bananaTotalSlices;
    private int carrotTotalSlices;
    private Vector3 initialKnifePosition;
    private Vector3 initialPosition = new Vector3(1.004f, 1.552f, -2.38f);

    // Lists to store instantiated slices
    List<GameObject> bananaInstantiatedSlices = new List<GameObject>();
    List<GameObject> carrotInstantiatedSlices = new List<GameObject>();

    // Parent objects for storing instantiated slices
    private GameObject bananaSlicesParent;
    private GameObject carrotSlicesParent;

    // Flag to track if carrot has been instantiated
    private bool carrotInstantiated = false;

    void Start()
    {
        // Initialize variables and find parent objects
        bananaTotalSlices = bananaSlicePrefabs.Length;
        carrotTotalSlices = carrotSlicePrefabs.Length;
        initialKnifePosition = transform.position;
        bananaSlicesParent = GameObject.Find("BananaSlicesParent");
        carrotSlicesParent = GameObject.Find("CarrotSlicesParent");
    }

    void Update()
    {
        // Handle user inputs
        HandleKeys();
    }

    // Detect collisions with Banana and Carrot
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banana"))
        {
            InstantiateFruitSlices(bananaTotalSlices, bananaInstantiatedSlices, bananaSlicePrefabs, bananaSlicesParent, "Banana");
            if (currentSlice == bananaTotalSlices)
            {
                StartCoroutine(ActivateArrowUIForAWithDelay());
            }
        }
        else if (collision.gameObject.CompareTag("Carrot"))
        {
            InstantiateFruitSlices(carrotTotalSlices, carrotInstantiatedSlices, carrotSlicePrefabs, carrotSlicesParent, "Carrot");
        }
    }

    // A function to handle user key inputs
    private void HandleKeys()
    {
        if (Input.GetKey(KeyCode.A) && currentSlice >= bananaTotalSlices)
        {
            MoveKnife(Vector3.left);

            if (arrowUIForA.activeSelf)
            {
                Animator animator = bananaSlicesParent.GetComponent<Animator>();
                animator.SetTrigger("PlayAnimation");
                // Deactivate the arrow UI for A after pressing A key
                arrowUIForA.SetActive(false);
                StartCoroutine(ActivateArrowUIForDWithDelay());
            }
        }
        if (Input.GetKey(KeyCode.D) && !carrotInstantiated)
        {
            ResetKnife();
            // Deactivate the arrow UI for D after pressing D key
            arrowUIForD.SetActive(false);
        }
    }

    // A function to instantiate fruit slices
    private void InstantiateFruitSlices(int totalSlices, List<GameObject> instantiatedSlices, GameObject[] slicePrefabs, GameObject slicesParent, string fruitType)
    {
        if (currentSlice < totalSlices)
        {
            Vector3 fruitSlicePosition = GetFruitSlicePosition(totalSlices, fruitType);
            GameObject fruitSlice = Instantiate(slicePrefabs[currentSlice], fruitSlicePosition, Quaternion.identity);
            instantiatedSlices.Add(fruitSlice);
            fruitSlice.transform.SetParent(slicesParent.transform);
            audioSource.PlayOneShot(cuttingSound);
            slicePrefabs[currentSlice].SetActive(false);
            if (fruitType == "Banana") MoveFruit(bananaPrefab);
            else MoveFruit(carrotPrefab);
            currentSlice++;
        }
    }

    // A function to move the knife
    private void MoveKnife(Vector3 direction)
    {
        if (transform.position.x < 1)
        {
            transform.position += direction * slideSpeed * Time.deltaTime;
        }
    }

    // A function to reset the knife position and destroy banana
    private void ResetKnife()
    {
        currentSlice = 0;
        transform.position = initialKnifePosition;
        carrotPrefab.transform.position = new Vector3(-0.294f, 1.599f, -2.317f);
        carrotInstantiated = true;
        if (bananaPrefab)
        {
            Destroy(bananaPrefab);
        }
    }

    // A function to get the position of the fruit slice
    private Vector3 GetFruitSlicePosition(int totalSlices, string fruitType)
    {
        Vector3 fruitSlicePosition;
        if (currentSlice < totalSlices / 2)
        {
            float offset = (fruitType == "Carrot") ? -0.1f : 0f;
            fruitSlicePosition = initialPosition + new Vector3((sliceGap * currentSlice) + offset, 0, 0);
        }
        else
        {
            // Add the specific offsets for banana and carrot
            float offset = (fruitType == "Banana") ? 0.25f : 0.185f;
            fruitSlicePosition = initialPosition + new Vector3(sliceGap * (currentSlice - totalSlices / 2) + offset, 0, 0.25f);
        }
        return fruitSlicePosition;
    }

    // A function to move the fruit
    private void MoveFruit(GameObject fruit)
    {
        fruit.transform.position += Vector3.right * moveDistance;
    }

    IEnumerator ActivateArrowUIForDWithDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        arrowUIForD.SetActive(true);
    }

    IEnumerator ActivateArrowUIForAWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        arrowUIForA.SetActive(true);
    }
}