using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class Tutorial : MonoBehaviour
{
    [Header("Step 1 UI")]
    [SerializeField] private Image wKeyImage;
    [SerializeField] private Image aKeyImage;
    [SerializeField] private Image sKeyImage;
    [SerializeField] private Image dKeyImage;
    [SerializeField] private Image shiftKeyImage;
    [SerializeField] private Image spacebarImage;

    [Header("Step 2 UI")]
    [SerializeField] private Image fKeyImage;
    [SerializeField] private Image tKeyImage;

    [Header("Step 3 UI")]
    [SerializeField] private Image cKeyImage;

    [Header("Walls")]
    [SerializeField] private GameObject[] wallsToDeactivate;

    [Header("Tutorial Text")]
    [SerializeField] private TextMeshProUGUI tutorialText; // Use TextMeshProUGUI for the tutorial text

    [Header("Car Reference")]
    [SerializeField] private Car car; // Reference to the Car script

    private int currentStep = 1;

    // Flags to track if keys have been pressed at least once
    private bool wPressed, aPressed, sPressed, dPressed, shiftPressed, spacePressed;
    private bool fPressed, tPressed;
    private bool cPressed;

    void Start()
    {
        ShowStep1();
    }

    void Update()
    {
        switch (currentStep)
        {
            case 1:
                HandleStep1();
                break;
            case 2:
                HandleStep2();
                break;
            case 3:
                HandleStep3();
                break;
            case 4:
                HandleStep4();
                break;
        }
    }

    private void ShowStep1()
    {
        // Enable Step 1 UI
        wKeyImage.gameObject.SetActive(true);
        aKeyImage.gameObject.SetActive(true);
        sKeyImage.gameObject.SetActive(true);
        dKeyImage.gameObject.SetActive(true);
        shiftKeyImage.gameObject.SetActive(true);
        spacebarImage.gameObject.SetActive(true);

        // Set tutorial text for Step 1
        tutorialText.text = "Press W, A, S, D to move and Shift to run.";

        // Disable other steps' UI
        DisableStep2UI();
        DisableStep3UI();
    }

    private void HandleStep1()
    {
        if (Input.GetKeyDown(KeyCode.W)) { wPressed = true; wKeyImage.color = Color.gray; }
        if (Input.GetKeyDown(KeyCode.A)) { aPressed = true; aKeyImage.color = Color.gray; }
        if (Input.GetKeyDown(KeyCode.S)) { sPressed = true; sKeyImage.color = Color.gray; }
        if (Input.GetKeyDown(KeyCode.D)) { dPressed = true; dKeyImage.color = Color.gray; }
        if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftPressed = true; shiftKeyImage.color = Color.gray; }
        if (Input.GetKeyDown(KeyCode.Space)) { spacePressed = true; spacebarImage.color = Color.gray; }

        if (wPressed && aPressed && sPressed && dPressed && shiftPressed && spacePressed)
        {
            currentStep = 2;
            ShowStep2();
        }
    }

    private void ShowStep2()
    {
        // Enable Step 2 UI
        fKeyImage.gameObject.SetActive(true);
        tKeyImage.gameObject.SetActive(true);

        // Set tutorial text for Step 2
        tutorialText.text = "Flashlight repels Monster";

        // Disable Step 1 UI
        DisableStep1UI();
    }

    private void HandleStep2()
    {
        if (Input.GetKeyDown(KeyCode.F)) { fPressed = true; fKeyImage.color = Color.gray; }
        if (Input.GetKeyDown(KeyCode.T)) { tPressed = true; tKeyImage.color = Color.gray; }

        if (fPressed && tPressed)
        {
            foreach (GameObject wall in wallsToDeactivate)
            {
                wall.SetActive(false);
            }

            currentStep = 3;
            ShowStep3();
        }
    }

    private void ShowStep3()
    {
        // Enable Step 3 UI
        cKeyImage.gameObject.SetActive(true);

        // Set tutorial text for Step 3
        tutorialText.text = "Go Outside and Press C near the Car";

        // Disable Step 2 UI
        DisableStep2UI();
    }

    private void HandleStep3()
    {
        if (Input.GetKeyDown(KeyCode.C)) { cPressed = true; cKeyImage.color = Color.gray; }

        if (cPressed)
        {
            car.GetInCar(); // Call the GetInCar() method from the Car script
            currentStep = 4;
            ShowStep4();
        }
    }

    private void ShowStep4()
    {
        // Disable Step 3 UI
        DisableStep3UI();

        // Set tutorial text for Step 4
        tutorialText.text = "Go to the office (Follow the road)";
    }

    private void HandleStep4()
    {
        // Step 4 logic can be added here if needed
    }

    private void DisableStep1UI()
    {
        wKeyImage.gameObject.SetActive(false);
        aKeyImage.gameObject.SetActive(false);
        sKeyImage.gameObject.SetActive(false);
        dKeyImage.gameObject.SetActive(false);
        shiftKeyImage.gameObject.SetActive(false);
        spacebarImage.gameObject.SetActive(false);
    }

    private void DisableStep2UI()
    {
        fKeyImage.gameObject.SetActive(false);
        tKeyImage.gameObject.SetActive(false);
    }

    private void DisableStep3UI()
    {
        cKeyImage.gameObject.SetActive(false);
    }
}
