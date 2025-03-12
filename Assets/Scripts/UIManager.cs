using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button exitButton;
    
    public GameObject shopUI;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject letterUI;
    public ScrollingTextUI typewriterUI;

    //inventory 
    public GameObject inventoryUI;
    public GameObject inventoryItem1;
    public GameObject inventoryItem2;

    // Map GameObjects
    public GameObject entranceMap;
    private GameManager gameManager;
    private PlayerManager playerManager;

    //Puzzle Image
    public GameObject puzzleImage;

    //Symphony Progress Update
    public TextMeshProUGUI symphonyProgress;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.currentState == GameManager.GameState.Game){
            showGameUI();
        }else{
            hideGameUI();
        }

        //if Combat scene is loaded
        Scene combatScene = SceneManager.GetSceneByName("Combat");
        bool isCombatSceneLoaded = combatScene.IsValid() && combatScene.isLoaded;

        //gameUI if in Game state AND Combat scene is not loaded
        if (gameManager.currentState == GameManager.GameState.Game && !isCombatSceneLoaded)
        {
            showGameUI();
        }
        else
        {
            hideGameUI();
        }

    }

    public void HideInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void ShowInventoryUI()
    {
        if (inventoryUI != null)
        {
            Debug.Log("Showing inventory");
            inventoryUI.SetActive(true);

            if (playerManager != null && playerManager.GetPurchasedItems().Contains(1))
            {
                inventoryItem1.SetActive(true);
            }
            else
            {
                inventoryItem1.SetActive(false);
            }

            if (playerManager != null && playerManager.GetPurchasedItems().Contains(2))
            {
                inventoryItem2.SetActive(true);
            }
            else
            {
                inventoryItem2.SetActive(false);
            }
        }
    }

    public void HideShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

    public void ShowShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(true);
        }
    }

    public void playButton()
    {
        if (menuUI != null)
        {
            FadeOutAndHide(menuUI);
            gameManager.SetGameState(GameManager.GameState.CutScene);
            typewriterUI.StartTypewriterText();

        }
    }

    public void letterButton()
    {
        if (letterUI != null)
        {
            FadeOutAndHide(letterUI);

            gameManager.StopIntroMusic(); 
            gameManager.SetGameState(GameManager.GameState.Game);

        
        }

         
    }

    public void showGameUI() //show coin count and inventory button
    {
        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }
    }

    public void hideGameUI() //hide coin count and inventory button
    {
        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }
    }

    //Puzzle Image for wing 1
    public void displayPuzzleImage(){
        puzzleImage.SetActive(true);
    }

    public void hidePuzzleImage(){
        puzzleImage.SetActive(false);
    }


    public void FadeOutAndHide(GameObject uiElement)
    {
        if (uiElement != null)
        {
            CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = uiElement.AddComponent<CanvasGroup>();
            }

            StartCoroutine(FadeOutCoroutine(canvasGroup, uiElement));
        }
        else
        {
            Debug.LogError("UI element is null.");
        }
    }

    private IEnumerator FadeOutCoroutine(CanvasGroup canvasGroup, GameObject uiElement)
    {
        float fadeDuration = 1.0f;
        float elapsedTime = 0f;

        canvasGroup.alpha = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        uiElement.SetActive(false);
    }

    public void UpdateSymphonyProgress(int symphonyMeter){ //1 for tut, 2 for first wing etc.
        if(symphonyMeter == 1){
            symphonyProgress.text = "1/4";
            Debug.Log("Updated progress to 1/4");
        }else if(symphonyMeter == 2){
            symphonyProgress.text = "2/4";
        }else if(symphonyMeter == 3){
            symphonyProgress.text = "3/4";
        }else if(symphonyMeter == 4){
            symphonyProgress.text = "4/4";
        }
        //FindObjectOfType<AudioManager>().PlaySheetCollectSound();
    }
    
}
