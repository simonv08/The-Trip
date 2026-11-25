using System.Collections.Generic;
using UnityEngine;

public class SequentialTagDropTrigger : MonoBehaviour
{
    // ELement 1 2 3 is de volgorde
    [SerializeField]
    private List<string> tagSequence = new List<string>
    {
        "Element 1",
        "Element 2",
        "Element 3"
    };

    [SerializeField] private string VerkeerdElement = "VerkeerdElement";
    [SerializeField] private bool destroyObject = false;

    private int currentIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(VerkeerdElement))
        {
            Debug.Log($"Fout! '{other.name}' heeft de verkeerde tag ('{VerkeerdElement}'). Volgorde is fout en wordt gereset.");
            currentIndex = 0;
            HandleDroppedObject(other.gameObject);
            return;
        }

        if (!tagSequence.Contains(other.tag))
        {
            return;
        }

        string expectedTag = tagSequence[currentIndex];

        if (other.CompareTag(expectedTag))
        {
            currentIndex++;
            Debug.Log($"Goed gedaan! '{other.name}' (tag: {expectedTag}) is op de juiste plek gegooid. ({currentIndex}/{tagSequence.Count})");

            if (currentIndex >= tagSequence.Count)
            {
                Debug.Log("Gelukt! Alle objecten zijn in de juiste volgorde");
                currentIndex = 0; // Reset voor een volgende poging
            }
        }
        else
        {
            Debug.Log($"Fout! Je gooide '{other.name}' (tag: {other.tag}) in de trigger, maar de goede was '{expectedTag}' (stap {currentIndex + 1} van {tagSequence.Count}). Volgorde wordt gereset.");
            currentIndex = 0;
        }

        // Zet het gegooide object uit of verwijder het
        HandleDroppedObject(other.gameObject);
    }

    private void HandleDroppedObject(GameObject droppedObject)
    {
        if (destroyObject)
        {
            Destroy(droppedObject);
        }
        else
        {
            droppedObject.SetActive(false);
        }
    }
}