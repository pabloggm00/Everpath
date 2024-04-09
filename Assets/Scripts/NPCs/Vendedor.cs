using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Vendedor : MonoBehaviour
{

    public string[] conversacion;
    public Sprite sprite;
    public List<string> textosBotones;
    public Dialogos sitio;

    GameObject currentButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {

        InputManager.playerControls.Player.Interactuar.performed += GetInteractuarVendedorInput;
        InputManager.playerControls.Player.Interactuar.canceled += GetInteractuarVendedorInput;

    }

    private void OnDisable()
    {
        InputManager.playerControls.Player.Interactuar.performed -= GetInteractuarVendedorInput;
        InputManager.playerControls.Player.Interactuar.canceled -= GetInteractuarVendedorInput;
    }

    private void Update()
    {
        if (DialogueSystem.instance.isActive && !DialogueSystem.instance.isHabladoGuardia)
        {
            DialogueSystem.instance.isHabladoGuardia = true;
  
            DialogueSystem.instance.sitioDialogo = sitio;
            DialogueSystem.instance.StartConversation(conversacion, sprite);
        }

        if (EventSystem.current.currentSelectedGameObject == null && DialogueSystem.instance.isActive)
        {
            EventSystem.current.SetSelectedGameObject(currentButton);
        }
        else
        {
            currentButton = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void GetInteractuarVendedorInput(InputAction.CallbackContext context)
    {
        if (context.performed && DialogueSystem.instance.isActive && !DialogueSystem.instance.isConversacion && !InventoryManager.instance.isInventario && !InventoryManager.instance.isPopUp && DialogueSystem.instance.isHabladoGuardia )
        {
           
            DialogueSystem.instance.sitioDialogo = sitio;
            DialogueSystem.instance.StartConversation(conversacion, sprite);
            
        }else if (context.canceled && DialogueSystem.instance.isConversacion && !DialogueSystem.instance.isGacha && !DialogueSystem.instance.isWaitRespuesta)
        {
            DialogueSystem.instance.NextSentence(textosBotones);
        }
    }

    






}
