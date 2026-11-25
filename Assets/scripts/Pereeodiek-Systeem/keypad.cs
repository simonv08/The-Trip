using UnityEngine;
using TMPro;

public class KeypadController : MonoBehaviour
{
    // code is mercury goud lood
    public string correctCode = "hgaupb";
    public TMP_Text codeDisplay;
    public GameObject keypadUI;
    public DoorController doorController;

    private string enteredCode = "";
    private bool isActive = false;

    void Update()
    {
        // Let player close keypad with Escape key
        if (isActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }

    public void NumberPressed(string number)
    {
        if (enteredCode.Length < 4)
        {
            enteredCode += number;
            codeDisplay.text = enteredCode;
        }
    }

    public void EnterPressed()
    {
        if (enteredCode == correctCode)
        {
            codeDisplay.text = "ACCESS GRANTED";
            doorController.OpenDoor();
            Invoke(nameof(CloseKeypad), 1.5f);
        }
        else
        {
            codeDisplay.text = "WRONG CODE";
            enteredCode = "";
        }
    }

    public void ClearPressed()
    {
        enteredCode = "";
        codeDisplay.text = "";
    }

    public void OpenKeypad()
    {
        keypadUI.SetActive(true);
        ClearPressed();
        isActive = true;

        // Optional: Lock player movement / camera here if you want
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        keypadUI.SetActive(false);
        isActive = false;

        // Optional: Unlock player movement again here
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}