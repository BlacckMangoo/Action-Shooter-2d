using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;


public class DroppedItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private InventoryItem itemData;
    [SerializeField] private GameObject infoTextBox;
    [SerializeField] private float rangeForInfoTextShow = 2f;
    [SerializeField] private KeyCode pickupKey = KeyCode.P;
    [SerializeField] private TextMeshPro itemnametext;
    [SerializeField] AudioClip collectAudio;



    private PlayerMovement player;
    private bool canPick;
    private Vector3 startPosition;
    private float hoverTime;

    private void Start()
    {
        // Cache the start position for hover effect
        startPosition = transform.position;

        // Find player reference - consider passing this through a reference instead
        player = FindObjectOfType<PlayerMovement>();
        

        itemnametext.text = itemData.name;
        // Ensure InfoTextBox is hidden at start
        if (infoTextBox != null)
        {
            infoTextBox.SetActive(false);
        }
    }
    private void Update()
    {
       

        float playerDistance = Mathf.Abs(player.transform.position.x -  transform.position.x);
        HandleInteraction(playerDistance);

    
    }

    private void HandleInteraction(float playerDistance)
    {
        bool isInRange = playerDistance < rangeForInfoTextShow;

        // Update pickup availability
        canPick = isInRange;

        // Update UI
        if (infoTextBox != null)
        {
            infoTextBox.SetActive(isInRange);
        }

        // Handle pickup input
        if (canPick && Input.GetKeyDown(pickupKey))
        {
            PickupItem();
        }

       
    }

    private void PickupItem()
    {
        if (InventorySystem.Instance != null)
        {
            // Add item to inventory
            InventorySystem.Instance.AddItem(itemData);
            UIManger.Instance.ShowMessage("Picked up " + itemData.name);
            // Optional: Play pickup sound/effect here

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventorySystem instance not found!");
        }
    }

    

    
}
