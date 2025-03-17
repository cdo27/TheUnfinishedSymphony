using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Button exitButton;
    
    public GameObject shopUI;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject exitUI;
    public GameObject letterUI;
    public ScrollingTextUI typewriterUI;

    //inventory 
    public GameObject inventoryUI;

    //inventory item prefabs
    public GameObject inventoryItem1;
    public GameObject inventoryItem2;
    public GameObject inventoryItem3;
    public GameObject inventoryItem4;
    public GameObject inventoryItem5;
    public GameObject inventoryItem6;

    //UI Item panel positions
    public GameObject posItem1;
    public GameObject posItem2;
    public GameObject posItem3;
    public GameObject posItem4;
    public GameObject posItem5;
    public GameObject posItem6;

    //Shop
    public GameObject shopItem1;
    public GameObject shopItem2;
    public GameObject shopItem3;
    public GameObject shopItem4;

    // Map GameObjects
    public GameObject entranceMap;
    private GameManager gameManager;
    private PlayerManager playerManager;

    private CutsceneManager cutsceneManager;

    //Puzzle Image
    public GameObject puzzleImage;

    //Symphony Progress Update
    public TextMeshProUGUI symphonyProgress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerManager = FindObjectOfType<PlayerManager>();

        cutsceneManager= FindObjectOfType<CutsceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager != null && shopUI.activeSelf)
        {
            List<int> ownedItems = playerManager.GetOwnedItems();

            //check and disable shop items if already owned
            shopItem1.SetActive(!ownedItems.Contains(1)); // item ID 1
            shopItem2.SetActive(!ownedItems.Contains(2)); // item ID 2
            shopItem3.SetActive(!ownedItems.Contains(4)); // item ID 4
            shopItem4.SetActive(!ownedItems.Contains(5)); // item ID 5
        }

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
        if (inventoryUI == null) return;

        Debug.Log("Showing inventory");
        inventoryUI.SetActive(true);

        //get owned items
        List<int> ownedItems = playerManager.GetOwnedItems();

        GameObject[] itemPoints = { posItem1, posItem2, posItem3, posItem4, posItem5, posItem6 };

        foreach (GameObject point in itemPoints)
        {
            if (point.transform.childCount > 0)
            {
                Destroy(point.transform.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < ownedItems.Count && i < itemPoints.Length; i++)
        {
            int itemID = ownedItems[i];
            GameObject itemPrefab = GetItemPrefabByID(itemID);

            if (itemPrefab != null)
            {
                GameObject newItem = Instantiate(itemPrefab, itemPoints[i].transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localScale = Vector3.one;
            }
            else
            {
                Debug.LogWarning("No prefab for item ID: " + itemID);
            }
        }
    }

    private GameObject GetItemPrefabByID(int itemID)
    {
        switch (itemID)
        {
            case 1: return inventoryItem1;
            case 2: return inventoryItem2;
            case 3: return inventoryItem3;
            case 4: return inventoryItem4;
            case 5: return inventoryItem5;
            case 6: return inventoryItem6;
            default: return null;
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

    //EXIT UI

    public void HideExitUI()
    {
        if (exitUI != null)
        {
            exitUI.SetActive(false);
        }
    }

    public void ShowExitUI()
    {
        if (exitUI != null)
        {
            exitUI.SetActive(true);
        }
    }

    public void YesExitGame()
    {
        if (exitUI != null)
        {
            SceneManager.LoadScene("Intro", LoadSceneMode.Single);
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
        // if (letterUI != null)
        // {
        //     FadeOutAndHide(letterUI);

        //     gameManager.StopIntroMusic(); 
        //     gameManager.SetGameState(GameManager.GameState.Game);

        
        // }

        if (letterUI != null)
        {
            StartCoroutine(LetterButtonSequence());
        }

         
    }

    private IEnumerator LetterButtonSequence()
    {
        CanvasGroup letterCanvasGroup = letterUI.GetComponent<CanvasGroup>();
        if (letterCanvasGroup == null) letterCanvasGroup = letterUI.AddComponent<CanvasGroup>();
        CanvasGroup cutsceneCanvasGroup = cutsceneManager.IntroPanel.GetComponent<CanvasGroup>();
        if (cutsceneCanvasGroup == null) cutsceneCanvasGroup = cutsceneManager.IntroPanel.AddComponent<CanvasGroup>();

        cutsceneManager.IntroPanel.SetActive(true);
        cutsceneCanvasGroup.alpha = 0f;

        float fadeDuration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeDuration;
            letterCanvasGroup.alpha = 1f - alpha;
            cutsceneCanvasGroup.alpha = alpha;

            if (elapsedTime >= fadeDuration * 0.5f && !cutsceneManager.IsCutsceneActive())
            {
                gameManager.StopIntroMusic();
                cutsceneManager.PlayIntroCutscene();
            }

            yield return null;
        }

        letterCanvasGroup.alpha = 0f;
        letterUI.SetActive(false);
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

    void doExitGame() {
        Application.Quit();
    }
    
}
