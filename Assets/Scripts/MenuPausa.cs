using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public static MenuPausa instance;
    public GameObject panelPausa;
    public GameObject button;
    [HideInInspector] public bool isPausa;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        InputManager.playerControls.UI.Pausa.performed += GetInputPausa;
        InputManager.playerControls.UI.Pausa.canceled += GetInputPausa;
    }

    private void OnDisable()
    {
        InputManager.playerControls.UI.Pausa.performed -= GetInputPausa;
        InputManager.playerControls.UI.Pausa.canceled -= GetInputPausa;
    }



    public void GetInputPausa(InputAction.CallbackContext context)
    {
        if (context.performed && !InventoryManager.instance.isInventario && !BattleController.instance.isPelea && !InventoryManager.instance.isVenta)
        {
            ShowMenu();
        }
    }

    void ShowMenu()
    {
        bool active = panelPausa.activeSelf;
        panelPausa.SetActive(!active);

        if (!active)
        {
            EventSystem.current.SetSelectedGameObject(button);
            Time.timeScale = 0;
            isPausa = true;

        }
        else
        {
            Time.timeScale = 1;
            isPausa = false;
        }
    }

    public void Jugar()
    {
        Time.timeScale = 1;
        isPausa = false;
    }

    public void Salir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
