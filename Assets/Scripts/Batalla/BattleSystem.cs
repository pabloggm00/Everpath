using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public BattleState state;


    [Header("Setup Battle")]
    public GameObject playerPrefab;
    public List<GameObject> enemiesPrefab;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    [Header("UI")]
    public Button firstButtonSelect;
    public GameObject panelObjetos;
    public GameObject accionesHUD;
    public GameObject maskTurn1;
    public GameObject maskTurn2;
    public GameObject maskTurn3;
    public GameObject allMask;
    public GameObject panelLoser;
    public GameObject panelWin;
    public GameObject butonA;
    public GameObject butonSpace;
    public TMP_Text lootMoneyText;
    public TMP_Text lootExpText;
    public TMP_Text lootEnemigoText;
    public TMP_Text lootEnemigoText2;

    [Header("StatsPlayer")]
    public Slider sliderVidaPlayer;
    public TMP_Text textoVidaPlayer;
    public Slider sliderManaPlayer;
    public TMP_Text textoManaPlayer;

    [Header("Objetos")]
    public List<Button> botonesObjetos;
    public Sprite emptySprite;
    

    Enemy enemyBattle;
    PlayerController playerBattle;

    bool isStarted;
    bool isFirstPelea = true;
    bool isLoot;
    GameObject playerTemp;
    GameObject enemyTemp;
    GameObject currentButton;
    GameObject spaceABorrar;
    int defensaNormal;

    private void OnEnable()
    {
        /*InputManager.playerControls.UI.Submit.performed += GetInputConsumir;
        InputManager.playerControls.UI.Submit.canceled += GetInputConsumir;*/

        InputManager.playerControls.UI.Submit.performed += GetInputSubmit;
        InputManager.playerControls.UI.Submit.canceled += GetInputSubmit;
    }

    private void OnDisable()
    {
        /*InputManager.playerControls.UI.Equipar.performed -= GetInputConsumir;
        InputManager.playerControls.UI.Equipar.canceled -= GetInputConsumir;*/

        InputManager.playerControls.UI.Submit.performed -= GetInputSubmit;
        InputManager.playerControls.UI.Submit.canceled -= GetInputSubmit;

    }


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleController.instance.isStarted && !isStarted)
        {
            
            state = BattleState.START;
            StartCoroutine(SetupBattle());
            isStarted = true;
            BattleController.instance.isStarted = false;
            EventSystem.current.SetSelectedGameObject(firstButtonSelect.gameObject);
            //OBJETOS
            BattleController.instance.MostrarObjetos(botonesObjetos);


        }

        sliderVidaPlayer.value = playerBattle.hP;
        sliderManaPlayer.value = playerBattle.mana;
        textoVidaPlayer.text = playerBattle.hP.ToString() + "/" + playerBattle.maxHp.ToString();
        textoManaPlayer.text = playerBattle.mana.ToString() + "/" + playerBattle.maxMana.ToString();

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(currentButton);
        }
        else
        {
            currentButton = EventSystem.current.currentSelectedGameObject;
        }

        CurrentOpcion();

        if (isLoot)
        {
            if (InputManager.GetControlDeviceType() == InputManager.ControlDeviceType.Gamepad)
            {
                butonA.SetActive(true);
                butonSpace.SetActive(false);
            }
            else
            {
                butonSpace.SetActive(true);
                butonA.SetActive(false);
            }
        }
        
    }

    GameObject EnemigoAleatorio()
    {
        int random = Random.Range(1, enemiesPrefab.Count);
        Debug.Log(random);
        for (int i = 1; i < enemiesPrefab.Count; i++)
        {
            if (random == i)
            {
                return enemiesPrefab[i];
            }
        }

        return null;
    }

    #region Inputs

    void GetInputSubmit(InputAction.CallbackContext context)
    {
        if (context.canceled && EventSystem.current.currentSelectedGameObject.tag == "BotonObjetos" && InventoryManager.instance.consumibles.Count > 0 && !isLoot)
        {
            EventSystem.current.SetSelectedGameObject(botonesObjetos[0].gameObject);
        }

        if (context.performed && ComprobarSiEstoyEnListaObjetos() && BattleController.instance.isPelea && EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name != "MarcoVacio" && !isLoot)
        {
            spaceABorrar = EventSystem.current.currentSelectedGameObject;
            Consumibles consumibleConsumido = InventoryManager.instance.BuscarConsumiblesPorSprite(InventoryManager.instance.GetCurrentConsumibleEnInventario().sprite);

            InventoryManager.instance.Consumir();

            playerBattle.hP += 25;

            if (playerBattle.hP + 25 > playerBattle.maxHp)
            {
                playerBattle.hP = playerBattle.maxHp;
            }

            if (consumibleConsumido.valorCuantia > 1)
            {
                ActualizarCuantia(consumibleConsumido);
            }
            else
            {
                //eliminamos del inventario y organizamos el inventario
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = emptySprite;
                EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().enabled = false;
                EventSystem.current.currentSelectedGameObject.SetActive(false);
                int id = InventoryManager.instance.GetIdInventarioConsumibles(consumibleConsumido);
                InventoryManager.instance.consumibles.RemoveAt(id);
                OrganizarInventario(botonesObjetos);

            }

            CurarButton();

        }

        if (context.performed && isLoot)
        {
            butonSpace.SetActive(false);
            butonA.SetActive(false);
            SalirBatalla();
        }
    }

    #endregion

    #region UI
    void ActualizarCuantia(Consumibles consumibleConsumido)
    {
        InventoryManager.instance.BuscarConsumiblesPorSprite(consumibleConsumido.sprite).valorCuantia--;
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text = "x" + InventoryManager.instance.BuscarConsumiblesPorSprite(InventoryManager.instance.GetCurrentConsumibleEnInventario().sprite).valorCuantia.ToString();
    }

    bool ComprobarSiEstoyEnListaObjetos()
    {
        for (int i = 0; i < botonesObjetos.Count; i++)
        {
            if (botonesObjetos[i].tag == EventSystem.current.currentSelectedGameObject.tag)
            {
                return true;
            }
        }

        return false;
    }

    

    void CurrentOpcion()
    {
        if (EventSystem.current.currentSelectedGameObject.CompareTag("BotonAtacar"))
        {
            //panelAtacar.SetActive(true);
            panelObjetos.SetActive(false);
        }
        else if (EventSystem.current.currentSelectedGameObject.CompareTag("BotonObjetos"))
        {
            //panelAtacar.SetActive(false);
            panelObjetos.SetActive(true);

            
        }
    }

    void OrganizarInventario(List<Button> listaBotones)
    {
        GameObject nuevoBotonSelect = listaBotones[spaceABorrar.transform.GetSiblingIndex() + 1].gameObject; //capturo el boton para tener el seleccionado despues 

        int oldIndexCasco = spaceABorrar.transform.GetSiblingIndex(); //capturo el indice donde pondre el nuevo item

        //transformo la lista de botones en la jerarquia de unity
        for (int i = spaceABorrar.transform.GetSiblingIndex() + 1; i < listaBotones.Count; i++)
        {
            listaBotones[i].transform.SetSiblingIndex(i - 1);
        }

        //transformo la lista de botones que existe por dentro
        MoveSpaceInventory(oldIndexCasco, listaBotones.Count - 1);

        //selecciono el nuevo boton para dejarlo seleccionado
        EventSystem.current.SetSelectedGameObject(nuevoBotonSelect);

        if (InventoryManager.instance.consumibles.Count == 0)
        {
            EventSystem.current.SetSelectedGameObject(firstButtonSelect.gameObject);
            
        }
    }

    public void MoveSpaceInventory(int oldIndex, int newIndex)
    {
        Button spaceConsumible = botonesObjetos[oldIndex];

        botonesObjetos.RemoveAt(oldIndex);
        botonesObjetos.Insert(newIndex, spaceConsumible);
    }

    #endregion

    #region Battle

    IEnumerator SetupBattle()
    {
        

        playerTemp = Instantiate(playerPrefab, playerBattleStation);


        if (isFirstPelea)
        {
   
            enemyTemp = Instantiate(enemiesPrefab[0], enemyBattleStation);
            
        }
        else
        {
            enemyTemp = Instantiate(EnemigoAleatorio(), enemyBattleStation);
        }


        enemyBattle = enemyTemp.GetComponent<Enemy>();
        playerBattle = playerTemp.GetComponent<PlayerController>();
        playerBattle.GetComponent<PlayerMove>().enabled = false;

        CargarPlayerData();
        

        yield return new WaitForSeconds(0.5f);

        

        //HAY QUE DETERMINAR LA VELOCIDAD DE CADA UNO PARA QUIEN EMPIEZA EL TURNO
        if (CompararVelocidad(playerBattle.velocidad, enemyBattle.velocidad) == 0)
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();

            maskTurn1.GetComponent<Image>().sprite = playerBattle.playerSpriteCombate;
            maskTurn2.GetComponent<Image>().sprite = enemyBattle.enemySprite;
            maskTurn3.GetComponent<Image>().sprite = playerBattle.playerSpriteCombate;
        }
        else
        {
            state = BattleState.ENEMYTURN;

            StartCoroutine(EnemyTurn());
            maskTurn1.GetComponent<Image>().sprite = enemyBattle.enemySprite;
            maskTurn2.GetComponent<Image>().sprite = playerBattle.playerSpriteCombate;
            maskTurn3.GetComponent<Image>().sprite = enemyBattle.enemySprite;
        }

        allMask.SetActive(true);

    }

    int CompararVelocidad(int velA, int velB)
    {
        if (velA > velB)
        {
            return 0;
        }
        else if (velA == velB)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    void CargarPlayerData()
    {

        playerBattle.hP = BattleController.instance.player.hP;
        playerBattle.maxHp = BattleController.instance.player.maxHp;
        playerBattle.damage = BattleController.instance.player.damage;
        playerBattle.defensa = BattleController.instance.player.defensa;
        playerBattle.mana = BattleController.instance.player.mana;
        playerBattle.maxMana = BattleController.instance.player.maxMana;
        playerBattle.velocidad = BattleController.instance.player.velocidad;
        playerBattle.nivel = BattleController.instance.player.nivel;
        playerBattle.expActual = BattleController.instance.player.expActual;
        playerBattle.expNeedPassLevel = BattleController.instance.player.expNeedPassLevel;
        playerBattle.differenceExp = BattleController.instance.player.differenceExp;
        playerBattle.oro = BattleController.instance.player.oro;

        defensaNormal = playerBattle.defensa;

        //Aplicar en el hud
        sliderVidaPlayer.maxValue = playerBattle.maxHp;
        sliderManaPlayer.maxValue = playerBattle.maxMana;
        
    }

    IEnumerator PlayerAttack()
    {
        accionesHUD.SetActive(false);

        yield return new WaitForSeconds(1f);

        bool isDead = enemyBattle.TakingDamage(playerBattle.damage);
        
        yield return new WaitForSeconds(1.5f); //Según lo que dura la animación

        enemyBattle.GetComponentInChildren<Animator>().SetBool("notAttack", false);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            PassTurnSprite();
            state = BattleState.ENEMYTURN;
            accionesHUD.SetActive(false);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator Curar()
    {
        accionesHUD.SetActive(false);

        yield return new WaitForSeconds(1.5f); //Según lo que dura la animación

        PassTurnSprite();
        state = BattleState.ENEMYTURN;
        accionesHUD.SetActive(false);
        StartCoroutine(EnemyTurn());
        
    }

    IEnumerator Defender()
    {
        accionesHUD.SetActive(false);

        yield return new WaitForSeconds(1f);

        playerBattle.defensa = Mathf.RoundToInt(playerBattle.defensa * 1.5f);

        yield return new WaitForSeconds(1.5f); //Según lo que dura la animación

        PassTurnSprite();
        state = BattleState.ENEMYTURN;
        accionesHUD.SetActive(false);
        StartCoroutine(EnemyTurn());

    }

    void PassTurnSprite()
    {
        maskTurn1.GetComponent<Image>().sprite = maskTurn2.GetComponent<Image>().sprite;
        maskTurn2.GetComponent<Image>().sprite = maskTurn3.GetComponent<Image>().sprite;
        maskTurn3.GetComponent<Image>().sprite = maskTurn1.GetComponent<Image>().sprite;
    }

    IEnumerator EnemyTurn()
    {
        //ataca el enemigo
   
        yield return new WaitForSeconds(1f);

        bool isDead = playerBattle.TakingDamage(enemyBattle.damage);

        yield return new WaitForSeconds(1f); // lo que dure la animación más x tiempo

        if (isDead)
        {
            playerBattle.defensa = defensaNormal;
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            playerBattle.defensa = defensaNormal;
            PassTurnSprite();
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {

        if (state == BattleState.WON)
        {

            Varios dropEnemigo = LootGenerator.instance.LootBatalla1(enemyBattle.loot);
            Consumibles pocion = null;

            if (dropEnemigo == null)
            {
                pocion = LootGenerator.instance.LootBatalla2();
              
            }

            //player ha ganado
            //mostrar lo ganado
            playerBattle.SumarExp(enemyBattle.expLoot);
            playerBattle.SumarOro(enemyBattle.oro);
            panelWin.SetActive(true);

            lootMoneyText.text = enemyBattle.oro.ToString() + " ORO"; 
            lootExpText.text = enemyBattle.expLoot.ToString() + " EXP";

            if (pocion != null && dropEnemigo != null)
            {
                lootEnemigoText.gameObject.SetActive(true);
                lootEnemigoText2.gameObject.SetActive(true);

                lootEnemigoText.text = "x1 " + pocion.nombre;
                lootEnemigoText2.text = "x1 " + dropEnemigo.nombre;
            }
            else if (pocion == null && dropEnemigo != null)
            {
                lootEnemigoText.gameObject.SetActive(false);

                lootEnemigoText2.text = "x1 " + dropEnemigo.nombre;
            }
            else if (pocion != null && dropEnemigo == null)
            {
                lootEnemigoText2.gameObject.SetActive(false);

                lootEnemigoText.text = "x1 " + pocion.nombre;
            }

            
            
            //potis por porcentaje 10% 
            isLoot = true;
         
        }
        else if (state == BattleState.LOST)
        {
            panelLoser.SetActive(true);
           
            isLoot = true;
        }

        if (isFirstPelea)
        {
            isFirstPelea = false;
            BattleController.instance.isPrimeraPelea = true;
        }
    }

    void PlayerTurn()
    {
        //Mostrar acciones
        accionesHUD.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSelect.gameObject);
    }

    void CurarButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(Curar());
    }

    public void DefenderButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(Defender());
    }

    public void AttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void SalirBatalla()
    {
        if (!isFirstPelea)
        {
            accionesHUD.SetActive(false);
            allMask.SetActive(false);
            panelLoser.SetActive(false);
            panelWin.SetActive(false);
            ActualizarDataDespuesBatalla();
            BattleController.instance.ActualizarDatosBatlla(playerBattle);
            BattleController.instance.CargarMundo(state);
            
        }
        
    }

    public int LootBatalla(Enemy lootEnemigo)
    {
        float total_probabilidad = lootEnemigo.loot.probabilidad / 100;
      
        int numLoot = 0;

        for (int i = 0; i < lootEnemigo.maxLoot; i++)
        {
            float num_aleatorio = Random.Range(0f, 1f);
          
            if (num_aleatorio < total_probabilidad)
            {
         
                InventoryManager.instance.AddVarios(lootEnemigo.loot);
                numLoot++;
                
            }
            
        }

        return numLoot;
    }

    void ActualizarDataDespuesBatalla()
    {
        /*BattleController.instance.player.hP = playerBattle.hP;
        
        BattleController.instance.player.maxHp = playerBattle.maxHp;
        BattleController.instance.player.damage = playerBattle.damage;
        BattleController.instance.player.defensa = playerBattle.defensa;
        BattleController.instance.player.mana = playerBattle.mana;
        BattleController.instance.player.maxMana = playerBattle.maxMana;
        BattleController.instance.player.velocidad = playerBattle.velocidad;
        BattleController.instance.player.expActual = playerBattle.expActual; 
        BattleController.instance.player.expNeedPassLevel = playerBattle.expNeedPassLevel;
        BattleController.instance.player.nivel = playerBattle.nivel;
        BattleController.instance.player.differenceExp = playerBattle.differenceExp;*/

     

        isLoot = false;
        isStarted = false;
        Destroy(playerTemp);
        Destroy(enemyTemp);
    }

    #endregion
}
