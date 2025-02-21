using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button exitButton;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject letterUI;
    public ScrollingTextUI typewriterUI;

    // Map GameObjects

    public GameObject entranceMap;
    private GameManager gameManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.currentState == GameManager.GameState.Game){
            showGameUI();
        }else{
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
            Debug.Log("showing inventory");
            inventoryUI.SetActive(true);
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


    public void doExitGame() {
        Application.Quit();
    }
    
}
