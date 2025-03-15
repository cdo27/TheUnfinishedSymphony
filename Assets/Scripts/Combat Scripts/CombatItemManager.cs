using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatItemManager : MonoBehaviour
{
    public CombatStateManager combatStateManager;

    //1: potion, 2: armor, 4: shield, 5: weapon
    public bool hasItem1;
    public bool hasItem2;
    public bool hasItem4;
    public bool hasItem5;

    //UI Icons for permanent items
    public GameObject item1Icon;
    public GameObject item2Icon;
    public GameObject item5Icon;

    //UI resources for permanent items
    public Sprite item1IconSprite;
    public Sprite item2IconSprite;
    public Sprite item5IconSprite;
    public Sprite emptyIconSprite;

    //UI related to perfect shield
    public GameObject perfectShield;
    public GameObject perfectShieldIcon;
    public GameObject perfectShieldKey;
    public GameObject perfectShieldCountObject;
    public TMP_Text perfectShieldCountText;

    Image perfectShieldIconImage;
    public Sprite perfectShieldActiveIconSprite;
    public Sprite perfectShieldInactiveIconSprite;

    //variables related to perfect shield
    public int perfectShieldCount;
    public bool perfectShieldActive;

    //run at the start in combat state manager, initiate fields related to combat items
    public void itemInitilize()
    {
        hasItem1 = hasItem2 = hasItem4 = hasItem5 = false;

        if (combatStateManager.playerManager == null)
        {
            Debug.LogWarning("PlayerManager not found. Skipping item initialization.");
            return;
        }

        // Initialize item 1
        Image item1IconImage = item1Icon.GetComponent<Image>();
        if (combatStateManager.playerManager.GetOwnedItems().Contains(1))
        {
            hasItem1 = true;
            item1IconImage.sprite = item1IconSprite;
        }
        else
        {
            item1IconImage.sprite = emptyIconSprite;
        }

        // Initialize item 2
        Image item2IconImage = item2Icon.GetComponent<Image>();
        if (combatStateManager.playerManager.GetOwnedItems().Contains(2))
        {
            hasItem2 = true;
            item2IconImage.sprite = item2IconSprite;
        }
        else
        {
            item2IconImage.sprite = emptyIconSprite;
        }

        // Initialize item 4 (perfect shield)
        perfectShieldIconImage = perfectShieldIcon.GetComponent<Image>();
        perfectShieldCount = 2;

        if (combatStateManager.playerManager.GetOwnedItems().Contains(4))
        {
            hasItem4 = true;
            perfectShieldCountObject.SetActive(true);
            perfectShieldIconImage.sprite = perfectShieldActiveIconSprite;
            perfectShieldCountText.text = perfectShieldCount.ToString();
        }
        else
        {
            perfectShieldIconImage.sprite = emptyIconSprite;
        }

        // Initialize item 5
        Image item5IconImage = item5Icon.GetComponent<Image>();
        if (combatStateManager.playerManager.GetOwnedItems().Contains(5))
        {
            hasItem5 = true;
            item5IconImage.sprite = item5IconSprite;
        }
        else
        {
            item5IconImage.sprite = emptyIconSprite;
        }
    }

    
    //perfect shield activation
    public void activatePerfectShield()
    {
        if(perfectShieldCount > 0)
        {
            //activate UI
            perfectShield.SetActive(true);

            //shield now active
            perfectShieldActive = true;

            //play sound
            combatStateManager.audioManager.playPerfectShieldSound();

            perfectShieldCount--;
            perfectShieldCountText.text = perfectShieldCount.ToString();
            perfectShieldIconImage.sprite = perfectShieldInactiveIconSprite;
            combatStateManager.combatItemManager.perfectShieldKey.SetActive(false);
        }
       
    }

    //perfect shield deactivation
    public void deactivatePerfectShield()
    {
        perfectShield.SetActive(false);
        perfectShieldActive = false;

        // Reactivate icon or keep it deactivated
        Image perfectShieldIconImage = perfectShieldIcon.GetComponent<Image>();

        if (perfectShieldCount == 0)
        {
            perfectShieldIconImage.sprite = perfectShieldInactiveIconSprite;
        }
        else if (perfectShieldCount > 0)
        {
            perfectShieldIconImage.sprite = perfectShieldActiveIconSprite;
            combatStateManager.combatItemManager.perfectShieldKey.SetActive(true);
        }
    }
}
