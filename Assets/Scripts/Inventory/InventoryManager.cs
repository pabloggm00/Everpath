using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public GameObject panel;

    //PLAYER
    [Header("Player")]
    public PlayerController playerController;

    [Header("Inventario")]
    public List<GameObject> ventanasInventory;
    public GameObject firstButtonInventory;
    public GameObject panelComparacion;
    public GameObject panelDescription;
    public Color colorVacioEquipado;
    public Color colorVacioInventario;

    [Header("PopUp")]
    public GameObject panelPopUp;
    public GameObject firstButtonPopUp;
    GameObject buttonBeforePopUp;
    /*public GameObject panelPopUpCuantia;
    public GameObject firstButtonPopUpCuantia;*/

    [Header("Stats Item Seleccionado")]
    public GameObject panelSeleccionado;
    public Image spriteItemSeleccionado; // se convertirá a sprite
    public TMP_Text atackSeleccionado;
    public TMP_Text defensaSeleccionado;
    public TMP_Text vidaSeleccionado;
    public TMP_Text manaSeleccionado;
    public TMP_Text velocidadSeleccionado;

    [Header("Espacios Equipos")]
    public Image imagenCascoEquipado;
    public Image imagenPantaEquipado;
    public Image imagenBotasEquipado;
    public Image imagenPecheraEquipado;
    public Image imagenArmaEquipado;
    public TMP_Text atackActual;
    public TMP_Text defensaActual;
    public TMP_Text vidaActual;
    public TMP_Text manaActual;
    public TMP_Text velocidadActual;
    public TMP_Text dineroActual;

    [Header("Panel A Comparar")]
    public Image imagenItemAComparar;
    public GameObject panelAComparar;
    Equipment itemEquipado;
    public TMP_Text atackEquipado;
    public TMP_Text defensaEquipado;
    public TMP_Text vidaEquipado;
    public TMP_Text manaEquipado;
    public TMP_Text velocidadEquipado;

    [Header("Descripcion No Equipment")]
    public Image imagenDescription;
    public TMP_Text textoDescription;

    [Header("Arma")]
    public List<Equipment> armas; //estas listas son privadas, solo comprobación
    public List<Button> spacesInventoryArmas;

    [Header("Cascos")]
    public List<Equipment> cascos; //estas listas son privadas, solo comprobación
    public List<Button> spacesInventoryCascos;

    [Header("Pechera")]
    public List<Equipment> pecheras; //estas listas son privadas, solo comprobación
    public List<Button> spacesInventoryPecheras;

    [Header("Pantalones")]
    public List<Equipment> pantalones; //estas listas son privadas, solo comprobación
    public List<Button> spacesInventoryPanta;

    [Header("Botas")]
    public List<Equipment> botas; //estas listas son privadas, solo comprobación
    public List<Button> spacesInventoryBotas;

    [Header("Consumibles")]
    public List<Consumibles> consumibles; //estas listas son privadas, solo comprobación
    public List<Button> spacesInventoryConsumibles;
    Consumibles consumibleObtained;

    [Header("Equipados")]
    public List<Equipment> equipados;
    Equipment equipoEquipado;



    /*[Header("Quest")]
    public List<Button> spacesInventoryQuest;
    public List<Consumibles> quests; //estas listas son privadas, solo comprobación*/


    [Header("Varios")]
    public List<Button> spacesInventoryVarios;
    public List<Varios> varios; //estas listas son privadas, solo comprobación
    Varios lootObtained;


    //GLOBAL
    bool isPaused;
    public Sprite emptySprite;
    [HideInInspector] public bool isInventario;
    [HideInInspector] public bool isPopUp;
    GameObject currentButton;

    //VENTA
    [Header("Venta")]
    public GameObject panelPopUpVenta;
    public GameObject firstButtonPopUpVenta;
    public TMP_Text textoVender;

    bool isPopUpCuantia;
    [HideInInspector] public bool isVenta;

    string tipoObjetoSpaceABorrar;
    GameObject spaceABorrar;
    Equipment equipoAntesDeEliminar;
    Consumibles consumibleAntesDeEliminar;
    Varios varioAntesDeEliminar;
    

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
        InputManager.playerControls.UI.NextVentana.performed += GetInputNextVentana;
        InputManager.playerControls.UI.NextVentana.canceled += GetInputNextVentana;

        InputManager.playerControls.UI.PreviousVentana.performed += GetInputPreviusVentana;
        InputManager.playerControls.UI.PreviousVentana.canceled += GetInputPreviusVentana;

        InputManager.playerControls.Player.Inventario.performed += GetInputInventario;
        InputManager.playerControls.Player.Inventario.canceled += GetInputInventario;

        InputManager.playerControls.UI.Equipar.performed += GetInputEquipar;
        InputManager.playerControls.UI.Equipar.canceled += GetInputEquipar;

        InputManager.playerControls.UI.Tirar.performed += GetInputTirar;
        InputManager.playerControls.UI.Tirar.canceled += GetInputTirar;

        InputManager.playerControls.UI.Cancel.performed += GetInputCancelarVenta;
        InputManager.playerControls.UI.Cancel.canceled += GetInputCancelarVenta;
    }

    private void OnDisable()
    {
        InputManager.playerControls.UI.NextVentana.performed -= GetInputNextVentana;
        InputManager.playerControls.UI.NextVentana.canceled -= GetInputNextVentana;

        InputManager.playerControls.UI.PreviousVentana.performed -= GetInputPreviusVentana;
        InputManager.playerControls.UI.PreviousVentana.canceled -= GetInputPreviusVentana;

        InputManager.playerControls.Player.Inventario.performed -= GetInputInventario;
        InputManager.playerControls.Player.Inventario.canceled -= GetInputInventario;

        InputManager.playerControls.UI.Equipar.performed -= GetInputEquipar;
        InputManager.playerControls.UI.Equipar.canceled -= GetInputEquipar;

        InputManager.playerControls.UI.Tirar.performed -= GetInputTirar;
        InputManager.playerControls.UI.Tirar.canceled -= GetInputTirar;

        InputManager.playerControls.UI.Cancel.performed -= GetInputCancelarVenta;
        InputManager.playerControls.UI.Cancel.canceled -= GetInputCancelarVenta;
    }

    private void Start()
    {
        cascos = new List<Equipment>();
        armas = new List<Equipment>();
        pantalones = new List<Equipment>();
        pecheras = new List<Equipment>();
        equipados = new List<Equipment>();
        botas = new List<Equipment>();

        ShowConsumibles();
        ShowLoots();
    }

    private void Update()
    {
        if (isPaused)
        {

            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(currentButton);
            }
            else
            {
                currentButton = EventSystem.current.currentSelectedGameObject;
            }

            

            if (!isPopUp && !isPopUpCuantia)
            {
                if (GetTipoItem(EventSystem.current.currentSelectedGameObject) == "Equipment")
                {
                    ShowItemSelected();
                    ShowItemEquiped();

                }else if (GetTipoItem(EventSystem.current.currentSelectedGameObject) == "Consumible")
                {
                    ShowConsumiblesSelected();
                    
                }
                else if (GetTipoItem(EventSystem.current.currentSelectedGameObject) == "Empty")
                {
                    panelComparacion.SetActive(false);
                    panelDescription.SetActive(false);
                  
                }
                
            }

         
            
        }
    }

    #region Inputs

    //INVENTARIO
    void GetInputInventario(InputAction.CallbackContext context)
    {
        if (context.canceled && !isPopUp && !BattleController.instance.isPelea && !DialogueSystem.instance.isConversacion && !isVenta && !MenuPausa.instance.isPausa)
        {
            ShowInventory();
            ApplyStats();
        }

    }

    void GetInputPreviusVentana(InputAction.CallbackContext context)
    {
        if (context.performed && !isPopUp && isInventario)
        {

            PreviusVentana();
            FirstSelected();
        }
    }



    void GetInputNextVentana(InputAction.CallbackContext context)
    {
        if (context.performed && isInventario)
        {
            NextVentana();
            FirstSelected();
        }
    }

    //EQUIPAR
    public void GetInputEquipar(InputAction.CallbackContext context)
    {
        if (context.performed && isInventario  && GetTipoItem(EventSystem.current.currentSelectedGameObject) == "Equipment" && !isVenta)
        {
            Equipar();
            ApplyStats();
        }
        else if(context.performed && isInventario && GetTipoItem(EventSystem.current.currentSelectedGameObject) == "Consumible" && !isVenta)
        {
            tipoObjetoSpaceABorrar = GetTipoItem(EventSystem.current.currentSelectedGameObject);
            spaceABorrar = EventSystem.current.currentSelectedGameObject;
            Consumibles consumibleConsumido = BuscarConsumiblesPorSprite(GetCurrentConsumibleEnInventario().sprite);

            Consumir();
            ActualizarDespuesDeConsumir(consumibleConsumido);
        }
        else if (context.performed && isInventario && isVenta 
           && !ComprobarEquipoEnEquipadosPorSprite(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite) && !IsEmptySpace())
        {
            tipoObjetoSpaceABorrar = GetTipoItem(EventSystem.current.currentSelectedGameObject);
            spaceABorrar = EventSystem.current.currentSelectedGameObject;

            if (tipoObjetoSpaceABorrar == "Equipment")
            {
                equipoAntesDeEliminar = BuscarEquipmentEnInventarioPorSprite(spaceABorrar.GetComponent<Image>().sprite);
            }
            else if (tipoObjetoSpaceABorrar == "Consumible")
            {
                consumibleAntesDeEliminar = BuscarConsumiblesPorSprite(spaceABorrar.GetComponent<Image>().sprite);
            }
            else if (tipoObjetoSpaceABorrar == "Varios")
            {
                varioAntesDeEliminar = BuscarLootPorSpriteInventario(spaceABorrar.GetComponent<Image>().sprite);
            }

            MostrarMensajeVender();
        }
    }

    public void GetInputTirar(InputAction.CallbackContext context)
    {
        if (context.performed && isInventario && !isVenta
            && !ComprobarEquipoEnEquipadosPorSprite(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite) && !IsEmptySpace())
        {
            tipoObjetoSpaceABorrar = GetTipoItem(EventSystem.current.currentSelectedGameObject);
            spaceABorrar = EventSystem.current.currentSelectedGameObject;
            if (tipoObjetoSpaceABorrar == "Equipment")
            {
                equipoAntesDeEliminar = BuscarEquipmentEnInventarioPorSprite(spaceABorrar.GetComponent<Image>().sprite);
                MostrarMensajeTirar();
            }
        }
    }

    public void GetInputCancelarVenta(InputAction.CallbackContext context)
    {
        if (context.canceled && isVenta)
        {
            CerrarInventarioVenta();
        }
    }

    void CerrarInventarioVenta()
    {
        Time.timeScale = 1;
        isInventario = false;
        panel.SetActive(false);
        isVenta = false;
        isPaused = false;
    }

    public void AbrirInventarioVenta()
    {
        isInventario = true;
        panel.SetActive(true);
        Time.timeScale = 0;
        isVenta = true;
        ActualizarCuantiaObjetosConsumibles();
        ActualizarCuantiaLoot();
        ResetInventory();

        isPaused = true;
    }

    public void MostrarMensajeVender()
    {
        buttonBeforePopUp = EventSystem.current.currentSelectedGameObject;
        panelPopUpVenta.SetActive(true);
  
        switch (GetTipoItem(buttonBeforePopUp))
        {
            case "Equipment":
                Debug.Log(buttonBeforePopUp);
                textoVender.text = "¿Quieres vender " + BuscarEquipmentEnInventarioPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).nameEquipment + " por " + BuscarEquipmentEnInventarioPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).oroVenta.ToString() + "?";
                break;
            case "Consumible":
                textoVender.text = "¿Quieres vender " + BuscarConsumiblesPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).nombre + " por " + BuscarConsumiblesPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).oroVenta.ToString() + "?";
                break;
            case "Varios":
                textoVender.text = "¿Quieres vender " + BuscarLootPorSpriteInventario(buttonBeforePopUp.GetComponent<Image>().sprite).nombre + " por " + BuscarLootPorSpriteInventario(buttonBeforePopUp.GetComponent<Image>().sprite).dineroVenta.ToString() + "?";
                break;
            default:
                break;
        }

        

        EventSystem.current.SetSelectedGameObject(firstButtonPopUpVenta);
        isInventario = false;
        isPopUp = true;

    }

    public void OcultarMensajeVender()
    {
        panelPopUpVenta.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonBeforePopUp);
        isInventario = true;
        isPopUp = false;
    }

    //PARA TIENDA

    /*void MostrarMensajeTirarCuantia()
    {
        isPopUpCuantia = true;
        isInventario = false;
        buttonBeforePopUp = EventSystem.current.currentSelectedGameObject;
        panelPopUpCuantia.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonPopUpCuantia);
        consumibleAntesDeEliminar = BuscarConsumiblesPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite);
        sliderCuantia.maxValue = consumibleAntesDeEliminar.valorCuantia;
    }

    public void TirarPorCuantia()
    {
        Debug.Log("Has eliminado " + sliderCuantia.value + " consumibles");
        OcultarMensajeTirarCuantia();
    }

    public void OcultarMensajeTirarCuantia()
    {
        panelPopUpCuantia.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonBeforePopUp);
        isInventario = true;
        isPopUp = false;
    }*/

    bool IsEmptySpace()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name == "MarcoVacio")
        {
            return true;
        }

        return false;
    }


    #endregion

    #region Inventario


    string GetTipoItem(GameObject currentSelected)
    {
        if (currentSelected.GetComponent<Image>().sprite.name != "MarcoVacio")
        {
            if (ComprobarSiEsEquipmentPorSprite(currentSelected.GetComponent<Image>().sprite))
            {
                return "Equipment";
            }

            if (ComprobarSiEsConsumiblePorSprite(currentSelected.GetComponent<Image>().sprite))
            {
                return "Consumible";
            }

            if (ComprobarSiEsLootPorSprite(currentSelected.GetComponent<Image>().sprite))
            {
                return "Varios";
            }

        }
        else
        {
            return "Empty";
        }

        return "";
    }

    void NextVentana()
    {
        for (int i = 0; i < ventanasInventory.Count; i++)
        {
            if (ventanasInventory[i].activeSelf)
            {
                ventanasInventory[i].SetActive(false);

                if (i++ == ventanasInventory.Count - 1)
                {

                    ventanasInventory[0].SetActive(true);
                }
                else
                {
                    ventanasInventory[i++].SetActive(true);
                }
            }

        }
    }

    bool ComprobarVentanaInventarioPorTag(string nombreVentana)
    {
        foreach (GameObject ventana in ventanasInventory)
        {
            if (ventana.CompareTag(nombreVentana) && ventana.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    void PreviusVentana()
    {
        for (int i = ventanasInventory.Count - 1; i >= 0; i--)
        {
            if (ventanasInventory[i].activeSelf)
            {
                ventanasInventory[i].SetActive(false);
              
                if (i-- == 0)
                {
                    Debug.Log(i);
                    ventanasInventory[ventanasInventory.Count - 1].SetActive(true);
                }
                else
                {
                    ventanasInventory[i--].SetActive(true);

                }
            }
        }
    }

    void FirstSelected()
    {
        for (int i = 0; i < ventanasInventory.Count; i++)
        {
            if (ventanasInventory[i].activeSelf)
            {
                EventSystem.current.firstSelectedGameObject = ventanasInventory[i].GetComponentInChildren<Button>().gameObject;
                EventSystem.current.SetSelectedGameObject(ventanasInventory[i].GetComponentInChildren<Button>().gameObject);
            }

        }
    }

    void ResetInventory()
    {
        
        for (int i = 0; i < ventanasInventory.Count; i++)
        {
            if (i != 0)
            {
                ventanasInventory[i].SetActive(false);
            }
            else
            {
                ventanasInventory[0].SetActive(true);
            }
            
        }

        FirstSelected();
    }

    public void ShowInventory()
    {
        
        bool active = panel.activeSelf;
        panel.SetActive(!active);

        if (active)
        {
            isPaused = false;
            isInventario = false;
            isPopUp = false;
            Time.timeScale = 1;
        }
        else
        {
            ResetInventory();
            isPaused = true;
            isInventario = true;
            Time.timeScale = 0;
            ActualizarCuantiaObjetosConsumibles();
            ActualizarCuantiaLoot();
        }
    }


    //ITEM EQUIPED
    void ShowItemEquiped()
    {

        if (!IsEmptySpace())
        {
            foreach (Equipment equipo in equipados)
            {
                if (GetCurrentEquipmentEnInventario().tipoDeEquipamiento == equipo.tipoDeEquipamiento)
                {
                    panelAComparar.SetActive(true);
                    //modificar sprite y stats
                    imagenItemAComparar.sprite = equipo.sprite;
                    atackEquipado.text = equipo.damage.ToString();
                    defensaEquipado.text = equipo.defensa.ToString();
                    vidaEquipado.text = equipo.vida.ToString();
                    manaEquipado.text = equipo.mana.ToString();
                    velocidadEquipado.text = equipo.velocidad.ToString();
                }
                else
                {
                    panelAComparar.SetActive(false);
                }           
            }
        }
   
       
    }

    //Item Selected
    public void ShowItemSelected()
    {

        panelComparacion.SetActive(true);
        panelDescription.SetActive(false);

        Sprite spriteCurrentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;

        if (EventSystem.current.currentSelectedGameObject != null && spriteCurrentSelected.name != "MarcoVacio")
        {
            panelSeleccionado.SetActive(true);
            //Debug.Log(BuscarEquipmentPorSprite(currentSelect).sprite);
            spriteItemSeleccionado.sprite = BuscarEquipmentEnInventarioPorSprite(spriteCurrentSelected).sprite;
            atackSeleccionado.text = BuscarEquipmentEnInventarioPorSprite(spriteCurrentSelected).damage.ToString();
            defensaSeleccionado.text = BuscarEquipmentEnInventarioPorSprite(spriteCurrentSelected).defensa.ToString();
            vidaSeleccionado.text = BuscarEquipmentEnInventarioPorSprite(spriteCurrentSelected).vida.ToString();
            manaSeleccionado.text = BuscarEquipmentEnInventarioPorSprite(spriteCurrentSelected).mana.ToString();
            velocidadSeleccionado.text = BuscarEquipmentEnInventarioPorSprite(spriteCurrentSelected).velocidad.ToString();

        }
      
    }

    //Equipment
    bool ComprobarEquipoEnEquipadosPorSprite(Sprite spriteSelected)
    {
        foreach (Equipment equipo in equipados)
        {
            if (spriteSelected == equipo.sprite)
            {
                equipoEquipado = equipo;
                return true;
            }
        }

        return false;
    }

    bool ComprobarSiEsEquipmentPorSprite(Sprite spriteSelected)
    {

        if (armas.Count > 0)
        {
            foreach (Equipment arma in armas)
            {
                if (spriteSelected == arma.sprite)
                {
                    return true;
                }
            }

        }

        if (pecheras.Count > 0)
        {
            foreach (Equipment pechera in pecheras)
            {
                if (spriteSelected == pechera.sprite)
                {
                    return true;
                }
            }
        }

        if (pantalones.Count > 0)
        {
            foreach (Equipment panta in pantalones)
            {
                if (spriteSelected == panta.sprite)
                {
                    return true;
                }
            }
        }

        if (cascos.Count > 0)
        {
            foreach (Equipment casco in cascos)
            {
                if (spriteSelected == casco.sprite)
                {

                    return true;
                }
            }
        }

        if (botas.Count > 0)
        {
            foreach (Equipment bota in botas)
            {
                if (spriteSelected == bota.sprite)
                {

                    return true;
                }
            }
        }


        return false;
    }

    //Se le pasa el boton seleccionado actual para que iguale el sprite
    Equipment BuscarEquipmentEnInventarioPorSprite(Sprite spriteSelected)
    {
    
        foreach (Equipment arma in armas)
        {
            if (spriteSelected == arma.sprite)
            {
                return arma;
            }
        }

        foreach (Equipment pechera in pecheras)
        {
            if (spriteSelected == pechera.sprite)
            {
                return pechera;
            }
        }

        foreach (Equipment panta in pantalones)
        {
            if (spriteSelected == panta.sprite)
            {
                return panta;
            }
        }

        foreach (Equipment casco in cascos)
        {
            if (spriteSelected == casco.sprite)
            {
               
                return casco;
            }
        }

        foreach (Equipment bota in botas)
        {
            if (spriteSelected == bota.sprite)
            {

                return bota;
            }
        }

        return null;
    }

    void ActualizarHuecoInventario()
    {
      
        switch (tipoObjetoSpaceABorrar)
        {
            case "Equipment":
                switch (equipoAntesDeEliminar.tipoDeEquipamiento)
                {
                    case CategoriaEquimpent.Arma:

                        OrganizarInventario(spacesInventoryArmas, "Armas");

                        break;
                    case CategoriaEquimpent.Casco:

                        OrganizarInventario(spacesInventoryCascos, "Cascos");

                        break;
                    case CategoriaEquimpent.Pechera:

                        OrganizarInventario(spacesInventoryPecheras, "Pechera");

                        break;
                    case CategoriaEquimpent.Pantalones:

                        OrganizarInventario(spacesInventoryPanta, "Pantalones");

                        break;
                    case CategoriaEquimpent.Botas:

                        OrganizarInventario(spacesInventoryBotas, "Botas");

                        break;
                    default:
                        break;
                }
                break;
            case "Consumible":

                OrganizarInventario(spacesInventoryConsumibles, "Consumible");

            
                break;
            case "Varios":
                OrganizarInventario(spacesInventoryVarios, "Varios");
                break;
            default:
                break;
        }

        
    }


    public void OrganizarInventario(List<Button> listaBotones, string tipoItem)
    {
        GameObject nuevoBotonSelect = listaBotones[spaceABorrar.transform.GetSiblingIndex() + 1].gameObject; //capturo el boton para tener el seleccionado despues 

        int oldIndexCasco = spaceABorrar.transform.GetSiblingIndex(); //capturo el indice donde pondre el nuevo item

        //transformo la lista de botones en la jerarquia de unity
        for (int i = spaceABorrar.transform.GetSiblingIndex() + 1; i < listaBotones.Count; i++)
        {
            listaBotones[i].transform.SetSiblingIndex(i - 1); 
        }

        //transformo la lista de botones que existe por dentro
        MoveSpaceInventory(oldIndexCasco, listaBotones.Count - 1, tipoItem);

        //selecciono el nuevo boton para dejarlo seleccionado
        EventSystem.current.SetSelectedGameObject(nuevoBotonSelect);
    }

    public void MoveSpaceInventory(int oldIndex, int newIndex, string tipoItem)
    {

        switch (tipoItem)
        {
            case "Armas":

                Button spaceArma = spacesInventoryArmas[oldIndex];

                spacesInventoryArmas.RemoveAt(oldIndex);
                spacesInventoryArmas.Insert(newIndex, spaceArma);

                break;
            case "Cascos":

                Button spaceCasco = spacesInventoryCascos[oldIndex];

                spacesInventoryCascos.RemoveAt(oldIndex);
                spacesInventoryCascos.Insert(newIndex, spaceCasco);

                break;
            case "Pechera":

                Button spacePechera = spacesInventoryPecheras[oldIndex];

                spacesInventoryPecheras.RemoveAt(oldIndex);
                spacesInventoryPecheras.Insert(newIndex, spacePechera);

                break;
            case "Pantalones":

                Button spacePanta = spacesInventoryPanta[oldIndex];

                spacesInventoryPanta.RemoveAt(oldIndex);
                spacesInventoryPanta.Insert(newIndex, spacePanta);

                break;
            case "Botas":

                Button spaceBota = spacesInventoryBotas[oldIndex];

                spacesInventoryBotas.RemoveAt(oldIndex);
                spacesInventoryBotas.Insert(newIndex, spaceBota);

                break;
            case "Consumible":

                Button spaceConsumible = spacesInventoryConsumibles[oldIndex];

                spacesInventoryConsumibles.RemoveAt(oldIndex);
                spacesInventoryConsumibles.Insert(newIndex, spaceConsumible);

                break;
            case "Varios":

                Button spaceVario = spacesInventoryVarios[oldIndex];

                spacesInventoryVarios.RemoveAt(oldIndex);
                spacesInventoryVarios.Insert(newIndex, spaceVario);

                break;
            default:
                break;
        }
        
    }

    #endregion

    #region Varios

    public void AddVarios(Varios loot)
    {
        lootObtained = new Varios(loot.id, loot.nombre, loot.sprite, loot.dineroVenta, loot.valorCuantia);

        if (!LootRepetido(loot.id))
        {
            varios.Add(lootObtained);
        }
        else
        {
            BuscarLootPorId(loot.id).valorCuantia++;
        }

        ShowLoots();
    }

    bool LootRepetido(int id)
    {
        foreach (Varios loot in varios)
        {
            if (id == loot.id)
            {
                return true;
            }
        }

        return false;
    }

    public Varios BuscarLootPorId(int id)
    {
        foreach (Varios loot in varios)
        {
            if (id == loot.id)
            {
                return loot;
            }
        }

        return null;
    }

    public Varios BuscarLootPorSpriteInventario(Sprite spriteSelected)
    {
        foreach (Varios loot in varios)
        {
            if (spriteSelected == loot.sprite)
            {
                return loot;
            }
        }

        return null;
    }

    bool ComprobarSiEsLootPorSprite(Sprite spriteSelected)
    {
        foreach (Consumibles consumible in consumibles)
        {
            if (spriteSelected == consumible.sprite)
            {
                return true;
            }
        }

        return false;
    }

    void ShowLoots()
    {
        for (int i = 0; i < varios.Count; i++)
        {
            if (BuscarLootPorSpriteInventario(lootObtained.sprite).valorCuantia < 2)
            {
                spacesInventoryVarios[i].GetComponent<Image>().sprite = varios[i].sprite;
                Debug.Log(spacesInventoryVarios[i].GetComponent<Image>().sprite);
                spacesInventoryVarios[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    void ActualizarCuantiaLoot()
    {
        for (int i = 0; i < spacesInventoryVarios.Count; i++)
        {
            if (spacesInventoryVarios[i].GetComponent<Image>().sprite.name == "MarcoVacio")
            {
                spacesInventoryVarios[i].GetComponentInChildren<TMP_Text>().enabled = false;
            }
        }

        if (varios.Count > 0)
        {
            for (int i = 0; i < varios.Count; i++)
            {
                if (varios[i].valorCuantia > 0)
                {
                    GetButtonLoot(varios[i]).GetComponentInChildren<TMP_Text>().enabled = true;
                    GetButtonLoot(varios[i]).GetComponentInChildren<TMP_Text>().text = "x" + BuscarLootPorSpriteInventario(varios[i].sprite).valorCuantia.ToString();
                }
            }
           
        }
    }

    Button GetButtonLoot(Varios loot)
    {
        foreach (Button button in spacesInventoryVarios)
        {
            if (loot.sprite == button.GetComponent<Image>().sprite)
            {
                return button;
            }
        }

        return null;
    }

    public int GetIdInventarioLoot(Varios variosSelected)
    {
        Debug.Log(variosSelected.id);
        for (int i = 0; i < varios.Count; i++)
        {
            if (variosSelected.id == varios[i].id)
            {

                return i;
            }
        }

        return 9000;
    }

    Varios GetCurrentLootEnInventario()
    {
        return BuscarLootPorSpriteInventario(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite);
    }

    #endregion

    #region Consumibles

    public void AddConsumible(Consumibles consumible)
    {
        consumibleObtained = new Consumibles(consumible.id, consumible.nombre, consumible.sprite, consumible.categoria, consumible.valorCuantia, consumible.description, consumible.oroVenta);
      

        if (!ConsumibleRepetido(consumibleObtained.id))
        {
            consumibles.Add(consumibleObtained);
        }
        else
        {
            BuscarConsumiblesPorSprite(consumibleObtained.sprite).valorCuantia++;
  
        }
       
        ShowConsumibles();
    }

    Button GetButtonConsumible(Consumibles consumible )
    {
        foreach (Button button in spacesInventoryConsumibles)
        {
            if (consumible.sprite.name == button.GetComponent<Image>().sprite.name)
            {
                return button;
            }
        }

        return null;
    }

    bool ConsumibleRepetido(int id)
    {

        foreach (Consumibles consumible in consumibles)
        {
            if (id == consumible.id)
            {
                return true;
            }
        }

        return false;
    }

    void ShowConsumibles()
    {
        for (int i = 0; i < consumibles.Count; i++)
        {
            if (BuscarConsumiblesPorSprite(consumibleObtained.sprite).valorCuantia < 2)
            {
                spacesInventoryConsumibles[i].GetComponent<Image>().sprite = consumibles[i].sprite;
                spacesInventoryConsumibles[i].GetComponent<Image>().color = Color.white;
            }  
        }
    }

    void ActualizarCuantiaObjetosConsumibles()
    {

        /*if (ComprobarVentanaInventarioPorTag("VentanaConsumibles"))
        {*/
        for (int i = 0; i < spacesInventoryConsumibles.Count; i++)
        {
            if (spacesInventoryConsumibles[i].GetComponent<Image>().sprite.name == "MarcoVacio")
            {
                spacesInventoryConsumibles[i].GetComponentInChildren<TMP_Text>().enabled = false;
            }

        }

        if (consumibles.Count > 0)
        {
            for (int i = 0; i < consumibles.Count; i++)
            {
                if (consumibles[i].valorCuantia > 0)
                {
                    GetButtonConsumible(consumibles[i]).GetComponentInChildren<TMP_Text>().enabled = true;
                    GetButtonConsumible(consumibles[i]).GetComponentInChildren<TMP_Text>().text = "x" + BuscarConsumiblesPorSprite(consumibles[i].sprite).valorCuantia.ToString();
                }
            }
        }

            
        //}
    }


    //Para ver los seleccionados
    void ShowConsumiblesSelected()
    {
        panelComparacion.SetActive(false);
        panelDescription.SetActive(true);

        if (GetCurrentConsumibleEnInventario().sprite != null)
        {
            imagenDescription.sprite = GetCurrentConsumibleEnInventario().sprite;
            textoDescription.text = GetCurrentConsumibleEnInventario().description;
        }
        
    }

    public void Consumir()
    {
        
        switch (BuscarConsumiblesPorSprite(GetCurrentConsumibleEnInventario().sprite).categoria)
        {
            case CategoriaConsumible.Curas:
                //aplicar estados
                playerController.hP += 25;

                if (playerController.hP+25 > playerController.maxHp)
                {
                    playerController.hP = playerController.maxHp;
                }
                break;
            case CategoriaConsumible.PocionesEstados:
                //aplicar estados
                Debug.Log("Otia una poti");
                break;
            default:
                break;
        }

    }

    void ActualizarDespuesDeConsumir(Consumibles consumibleConsumido)
    {
        if (BuscarConsumiblesPorSprite(GetCurrentConsumibleEnInventario().sprite).valorCuantia > 1)
        {
            BuscarConsumiblesPorSprite(GetCurrentConsumibleEnInventario().sprite).valorCuantia--;
            ActualizarCuantiaObjetosConsumibles();
        }
        else
        {
            BuscarConsumiblesPorSprite(GetCurrentConsumibleEnInventario().sprite).valorCuantia--;
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = emptySprite;
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = colorVacioInventario;
            ActualizarCuantiaObjetosConsumibles();
            int id = GetIdInventarioConsumibles(consumibleConsumido);
            consumibles.RemoveAt(id);
            ActualizarHuecoInventario();
            

        }
    }

    //Me devuelve el current consumible del inventario
    public Consumibles GetCurrentConsumibleEnInventario()
    {
        return BuscarConsumiblesPorSprite(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite);
    }

    public int GetIdInventarioConsumibles(Consumibles consumibleSelected)
    {
        Debug.Log(consumibleSelected.id);
        for (int i = 0; i < consumibles.Count; i++)
        {
            if (consumibleSelected.id == consumibles[i].id)
            {
                
                return i;
            }
        }

        return 9000;
    }

    public Consumibles BuscarConsumiblesPorSprite(Sprite spriteSelected)
    {
        foreach (Consumibles consumible in consumibles)
        {
            if (consumible.sprite == spriteSelected)
            {
                return consumible;
            }
        }

        return null;
    }
    
    bool ComprobarSiEsConsumiblePorSprite(Sprite spriteSelected)
    {
        foreach (Consumibles consumible in consumibles)
        {
            if (spriteSelected == consumible.sprite)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Equipment

    public void AddEquipment(Equipment equipment)
    {
        switch (equipment.tipoDeEquipamiento)
        {
            case CategoriaEquimpent.Arma:

                Equipment armaObtained = new Equipment(equipment.nameEquipment, equipment.id, equipment.damage, equipment.defensa, equipment.vida, equipment.mana, equipment.velocidad, equipment.sprite, equipment.isEquipado, equipment.rareza, equipment.tipoDeEquipamiento, equipment.oroVenta);
                if (armas.Count < spacesInventoryArmas.Count)
                {
                    armas.Add(armaObtained);
                    ShowEquipment();
                }
                else
                {
                    Debug.Log("No caben más armas");
                }
                
                break;
            case CategoriaEquimpent.Casco:

                Equipment cascoObtained = new Equipment(equipment.nameEquipment, equipment.id, equipment.damage, equipment.defensa, equipment.vida, equipment.mana, equipment.velocidad, equipment.sprite, equipment.isEquipado, equipment.rareza, equipment.tipoDeEquipamiento, equipment.oroVenta);

                if (cascos.Count < spacesInventoryCascos.Count)
                {
                    cascos.Add(cascoObtained);
                    ShowEquipment();
                }
                else
                {
                    Debug.Log("No caben más cascos");
                }
                
                break;
            case CategoriaEquimpent.Pechera:

                Equipment pecheraObtained = new Equipment(equipment.nameEquipment, equipment.id, equipment.damage, equipment.defensa, equipment.vida, equipment.mana, equipment.velocidad, equipment.sprite, equipment.isEquipado, equipment.rareza, equipment.tipoDeEquipamiento, equipment.oroVenta);

                if (pecheras.Count < spacesInventoryPecheras.Count)
                {
                    pecheras.Add(pecheraObtained);
                    ShowEquipment();
                }
                else
                {
                    Debug.Log("No caben más pecheras");
                }

                break;
            case CategoriaEquimpent.Pantalones:

                Equipment pantalonesObtained = new Equipment(equipment.nameEquipment, equipment.id, equipment.damage, equipment.defensa, equipment.vida, equipment.mana, equipment.velocidad, equipment.sprite, equipment.isEquipado, equipment.rareza, equipment.tipoDeEquipamiento, equipment.oroVenta);

                if (pantalones.Count < spacesInventoryPanta.Count)
                {
                    pantalones.Add(pantalonesObtained);
                    ShowEquipment();
                }
                else
                {
                    Debug.Log("No caben más pantalones");
                }

                break;
            case CategoriaEquimpent.Botas:

                Equipment botasObtained = new Equipment(equipment.nameEquipment, equipment.id, equipment.damage, equipment.defensa, equipment.vida, equipment.mana, equipment.velocidad, equipment.sprite, equipment.isEquipado, equipment.rareza, equipment.tipoDeEquipamiento, equipment.oroVenta);

                if (botas.Count < spacesInventoryBotas.Count)
                {
                    botas.Add(botasObtained);
                    ShowEquipment();
                }
                else
                {
                    Debug.Log("No caben más botas");
                }
               
                break;
            default:
                break;
        }
       
    }

    void ShowEquipment()
    {
        
        for (int i = 0; i < armas.Count; i++)
        {
            spacesInventoryArmas[i].GetComponent<Image>().sprite = armas[i].sprite;
            spacesInventoryArmas[i].GetComponent<Image>().color = Color.white;

            
        }

        for (int i = 0; i < cascos.Count; i++)
        {
            spacesInventoryCascos[i].GetComponent<Image>().sprite = cascos[i].sprite;
            spacesInventoryCascos[i].GetComponent<Image>().color = Color.white;

           
        }

        for (int i = 0; i < pecheras.Count; i++)
        {
            spacesInventoryPecheras[i].GetComponent<Image>().sprite = pecheras[i].sprite;
            spacesInventoryPecheras[i].GetComponent<Image>().color = Color.white;

          
        }

        for (int i = 0; i < pantalones.Count; i++)
        {
            spacesInventoryPanta[i].GetComponent<Image>().sprite = pantalones[i].sprite;
            spacesInventoryPanta[i].GetComponent<Image>().color = Color.white;


        }

        for (int i = 0; i < botas.Count; i++)
        {
            spacesInventoryBotas[i].GetComponent<Image>().sprite = botas[i].sprite;
            spacesInventoryBotas[i].GetComponent<Image>().color = Color.white;

            

        }
    }

    #endregion

    #region Equipar

    
    //Les sumo los stats
    void SumarStats(Equipment equipo)
    {
        playerController.damage += equipo.damage; 
        playerController.defensa += equipo.defensa; 
        playerController.hP += equipo.vida; 
        playerController.mana += equipo.mana; 
        playerController.velocidad += equipo.velocidad;
        playerController.maxHp += equipo.vida;
        
    }

    //Les resto los stats hasta que llegan a cero
    void RestarStats(Equipment equipo)
    {
        playerController.damage -= equipo.damage;
        playerController.defensa -= equipo.defensa;
        playerController.hP -= equipo.vida;
        playerController.mana -= equipo.mana;
        playerController.velocidad -= equipo.velocidad;
        playerController.maxHp -= equipo.vida;

        if (playerController.damage < 0 || playerController.defensa < 0 || playerController.hP < 0 || playerController.mana < 0 || playerController.velocidad < 0)
        {
            playerController.damage = 0;
            playerController.defensa = 0;
            playerController.hP = 0;
            playerController.mana = 0;
            playerController.velocidad = 0;
            
        }
    }

    public void MostrarMensajeTirar()
    {
        buttonBeforePopUp = EventSystem.current.currentSelectedGameObject; 
        panelPopUp.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonPopUp);
        isInventario = false;
        isPopUp = true;

    }

    public void OcultarMensajeTirar()
    {
        panelPopUp.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonBeforePopUp);
        isInventario = true;
        isPopUp = false;
    }


    public void Tirar()
    {
        OcultarMensajeTirar();
        OcultarMensajeVender();

        if (ComprobarSiEsConsumiblePorSprite(buttonBeforePopUp.GetComponent<Image>().sprite))
        {
            if (isVenta)
            {
                playerController.SumarOro(BuscarConsumiblesPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).oroVenta);
            }
            consumibles.RemoveAt(GetIdInventarioConsumibles(GetCurrentConsumibleEnInventario()));
        }
        else if (ComprobarSiEsEquipmentPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite))
        {
            if (isVenta)
            {
                playerController.SumarOro(BuscarEquipmentEnInventarioPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).oroVenta);
            }

            switch (BuscarEquipmentEnInventarioPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite).tipoDeEquipamiento)
            {
                case CategoriaEquimpent.Arma:
                    armas.RemoveAt(GetIdInventarioEquipment(GetCurrentEquipmentEnInventario(), armas));
                    break;
                case CategoriaEquimpent.Casco:
                    cascos.RemoveAt(GetIdInventarioEquipment(GetCurrentEquipmentEnInventario(), cascos));
                    break;
                case CategoriaEquimpent.Pechera:
                    pecheras.RemoveAt(GetIdInventarioEquipment(GetCurrentEquipmentEnInventario(), pecheras));
                    break;
                case CategoriaEquimpent.Pantalones:
                    pantalones.RemoveAt(GetIdInventarioEquipment(GetCurrentEquipmentEnInventario(), pantalones));
                    break;
                case CategoriaEquimpent.Botas:
                    botas.RemoveAt(GetIdInventarioEquipment(GetCurrentEquipmentEnInventario(), botas));
                    break;
                default:
                    break;
            }

            
        }
        else if (ComprobarSiEsLootPorSprite(buttonBeforePopUp.GetComponent<Image>().sprite))
        {
            if (isVenta)
            {
                playerController.SumarOro(BuscarLootPorSpriteInventario(buttonBeforePopUp.GetComponent<Image>().sprite).dineroVenta);
                varios.RemoveAt(GetIdInventarioLoot(GetCurrentLootEnInventario()));
            }
        }


        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = emptySprite;
        buttonBeforePopUp.GetComponent<Image>().color = colorVacioInventario;

        ActualizarHuecoInventario();

    }

    int GetIdInventarioEquipment(Equipment equipoSelected, List<Equipment> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].id == equipoSelected.id)
            {
                return i;
            }
        }

        return 9000;
    }

    //Necesito el id del item actual para poder eliminarlo de la lista de equipados
    int GetIdEquipados(Equipment equipoSelected)
    {
        for (int i = 0; i < equipados.Count; i++)
        {
            if (equipados[i].tipoDeEquipamiento == equipoSelected.tipoDeEquipamiento)
            {
                return i;
            }
        }

        return 99;
    }

    //Si el player si tiene equipado un item del mismo tipo de categoria con el current item y no es el mismo item, elimino el anterior y meto el nuevo.
    //Si el player no tiene equipado un item del mismo tipo de categoria con el current item, le agrego el item seleccionado.
    void Equipar()
    {

        if (!IsEmptySpace())
        {
            if (ComprobarTipoEquipamientoEquipado(GetCurrentEquipmentEnInventario().tipoDeEquipamiento))
            {

                if (!ComprobarEquipoEnEquipadosPorSprite(GetCurrentEquipmentEnInventario().sprite))
                {

                    //Quito el item de los equipados y le resto los stats
                    equipoEquipado.isEquipado = false;
                    RestarStats(equipoEquipado);
                    equipados.RemoveAt(GetIdEquipados(GetCurrentEquipmentEnInventario()));

                    //Agrego el otro item seleccionado y le sumo los stats
                    equipados.Add(GetCurrentEquipmentEnInventario());
                    SumarStats(GetCurrentEquipmentEnInventario());
                    GetCurrentEquipmentEnInventario().isEquipado = true;

                    switch (GetCurrentEquipmentEnInventario().tipoDeEquipamiento)
                    {
                        case CategoriaEquimpent.Arma:
                            imagenArmaEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                            imagenArmaEquipado.color = Color.white;
                            break;
                        case CategoriaEquimpent.Casco:
                            imagenCascoEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                            imagenCascoEquipado.color = Color.white;
                            break;
                        case CategoriaEquimpent.Pechera:
                            imagenPecheraEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                            imagenPecheraEquipado.color = Color.white;
                            break;
                        case CategoriaEquimpent.Pantalones:
                            imagenPantaEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                            imagenPantaEquipado.color = Color.white;
                            break;
                        case CategoriaEquimpent.Botas:
                            imagenBotasEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                            imagenBotasEquipado.color = Color.white;
                            break;
                        default:
                            break;
                    }


                    
                }else if (ComprobarEquipoEnEquipadosPorSprite(GetCurrentEquipmentEnInventario().sprite))
                {
                    //Quito el item de los equipados y le resto los stats
                    GetCurrentEquipmentEnInventario().isEquipado = false;
                    RestarStats(GetCurrentEquipmentEnInventario());
                    equipados.RemoveAt(GetIdEquipados(GetCurrentEquipmentEnInventario()));

                    switch (GetCurrentEquipmentEnInventario().tipoDeEquipamiento)
                    {
                        case CategoriaEquimpent.Arma:
                            imagenArmaEquipado.sprite = emptySprite;
                            imagenArmaEquipado.color = colorVacioEquipado;
                            break;
                        case CategoriaEquimpent.Casco:
                            imagenCascoEquipado.sprite = emptySprite;
                            imagenCascoEquipado.color = colorVacioEquipado;
                            break;
                        case CategoriaEquimpent.Pechera:
                            imagenPecheraEquipado.sprite = emptySprite;
                            imagenPecheraEquipado.color = colorVacioEquipado;
                            break;
                        case CategoriaEquimpent.Pantalones:
                            imagenPantaEquipado.sprite = emptySprite;
                            imagenPantaEquipado.color = colorVacioEquipado;
                            break;
                        case CategoriaEquimpent.Botas:
                            imagenBotasEquipado.sprite = emptySprite;
                            imagenBotasEquipado.color = colorVacioEquipado;
                            break;
                        default:
                            break;
                    }
                }

                panelAComparar.SetActive(false);

            }
            else if (!ComprobarTipoEquipamientoEquipado(GetCurrentEquipmentEnInventario().tipoDeEquipamiento))
            {
                //Agrego el item seleccionado y le sumo los stats
                equipados.Add(GetCurrentEquipmentEnInventario());

                switch (GetCurrentEquipmentEnInventario().tipoDeEquipamiento)
                {
                    case CategoriaEquimpent.Arma:
                        imagenArmaEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                        imagenArmaEquipado.color = Color.white;
                        break;
                    case CategoriaEquimpent.Casco:
                        imagenCascoEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                        imagenCascoEquipado.color = Color.white;
                        break;
                    case CategoriaEquimpent.Pechera:
                        imagenPecheraEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                        imagenPecheraEquipado.color = Color.white;
                        break;
                    case CategoriaEquimpent.Pantalones:
                        imagenPantaEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                        imagenPantaEquipado.color = Color.white;
                        break;
                    case CategoriaEquimpent.Botas:
                        imagenBotasEquipado.sprite = GetCurrentEquipmentEnInventario().sprite;
                        imagenBotasEquipado.color = Color.white;
                        break;
                    default:
                        break;
                }

                GetCurrentEquipmentEnInventario().isEquipado = true;
                SumarStats(GetCurrentEquipmentEnInventario());
            }


        }
        
    }


    //Me devuelve el current item de todos los del inventario
    Equipment GetCurrentEquipmentEnInventario()
    {
        return BuscarEquipmentEnInventarioPorSprite(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite);
    }

    bool ComprobarTipoEquipamientoEquipado(CategoriaEquimpent categoria)
    {
        if (equipados.Count > 0)
        {
            foreach (Equipment equipo in equipados)
            {
                if (categoria == equipo.tipoDeEquipamiento)
                {
                    equipoEquipado = equipo;
                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    void ApplyStats()
    {
        atackActual.text = playerController.damage.ToString();
        defensaActual.text = playerController.defensa.ToString();
        vidaActual.text = playerController.hP.ToString();
        manaActual.text = playerController.mana.ToString();
        velocidadActual.text = playerController.velocidad.ToString();
        dineroActual.text = playerController.oro.ToString();
        
    }

   
}

