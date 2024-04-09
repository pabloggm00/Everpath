using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeyendaController : MonoBehaviour
{
    [Header("Gamepad")]
    public GameObject leyendaNormalGamepad;
    public GameObject leyendaInventarioGamepad;
    public GameObject leyendaVentaGamepad;


    [Header("Keyboard")]
    public GameObject leyendaNormalKeyboard;
    public GameObject leyendaInventarioKeyboard;
    public GameObject leyendaVentaKeyboard;
    public GameObject leyendaConfirmarKeyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!BattleController.instance.isPelea)
        {
            ChangeLeyenda();
        }
        else
        {
            OcultarGamepad();
            OcultarTeclado();
        }
       
    }

    void ChangeLeyenda()
    {
        switch (InputManager.GetControlDeviceType())
        {
            case InputManager.ControlDeviceType.KeyboardAndMouse:

                OcultarGamepad();

                if (InventoryManager.instance.isInventario)
                {
                    leyendaNormalKeyboard.SetActive(false);
                    leyendaInventarioKeyboard.SetActive(true);
                    leyendaVentaKeyboard.SetActive(false);
                    leyendaConfirmarKeyboard.SetActive(false);
                }
                else if (InventoryManager.instance.isVenta)
                {
                    leyendaNormalKeyboard.SetActive(false);
                    leyendaInventarioKeyboard.SetActive(false);
                    leyendaVentaKeyboard.SetActive(true);
                    leyendaConfirmarKeyboard.SetActive(false);

                }
                else if (DialogueSystem.instance.isDecision)
                {
                    leyendaNormalKeyboard.SetActive(false);
                    leyendaInventarioKeyboard.SetActive(false);
                    leyendaVentaKeyboard.SetActive(false);
                    leyendaConfirmarKeyboard.SetActive(true);
                }
                else
                {
                    leyendaNormalKeyboard.SetActive(true);
                    leyendaInventarioKeyboard.SetActive(false);
                    leyendaVentaKeyboard.SetActive(false);
                    leyendaConfirmarKeyboard.SetActive(false);
                }


                break;
            case InputManager.ControlDeviceType.Gamepad:

                OcultarTeclado();

                if (InventoryManager.instance.isInventario)
                {
                    leyendaNormalGamepad.SetActive(false);
                    leyendaInventarioGamepad.SetActive(true);
                    leyendaVentaGamepad.SetActive(false);
                }
                else if (InventoryManager.instance.isVenta)
                {
                    leyendaNormalGamepad.SetActive(false);
                    leyendaInventarioGamepad.SetActive(false);
                    leyendaVentaGamepad.SetActive(true);
                }
                else
                {
                    leyendaNormalGamepad.SetActive(true);
                    leyendaInventarioGamepad.SetActive(false);
                    leyendaVentaGamepad.SetActive(false);
                }

                break;
            default:
                break;
        }
    }

    void OcultarGamepad()
    {
        leyendaNormalGamepad.SetActive(false);
        leyendaInventarioGamepad.SetActive(false);
        leyendaVentaGamepad.SetActive(false);
    }

    void OcultarTeclado()
    {
        leyendaNormalKeyboard.SetActive(false);
        leyendaInventarioKeyboard.SetActive(false);
        leyendaVentaKeyboard.SetActive(false);
        leyendaConfirmarKeyboard.SetActive(false);
    }
}
