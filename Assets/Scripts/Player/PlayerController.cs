using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    

    [Header("Stats")]
    public int hP;
    public int maxHp = 10;
    public int damage;
    public int defensa;
    public int mana;
    public int maxMana = 10;
    public int velocidad;
    public int oro = 200;
  
    [Header("Nivel")]
    public float aMultiplier = 10;
    public float bMultiplier = 1.5f;
    public float cMultiplier = 2;
    public float dMultiplier = 0.5f;
    [HideInInspector] public int expNeedPassLevel;
    [HideInInspector] public int expActual;
    [HideInInspector] public int differenceExp;
    [HideInInspector] public int nivel = 1;

    public Sprite playerSprite;
    public Sprite playerSpriteCombate;
    public GameObject iconoInteractuar;


    bool isCofre;
    bool mostradoMensaje;
    Cofre cofreCogido;


    [Header("Conversacion")]
    public string [] conversacion;

    [Header("TPCiudadBosque")]
    public Transform posInicial;
    public Transform ciudad;
    public Transform bosque;

    /*public PlayerController (PlayerController player)
    {
        speed = player.speed;
        moreSpeed = player.moreSpeed;
        hP = player.hP;
        damage = player.damage;
        defensa = player.defensa;
        vida = player.vida;
        mana = player.mana;
        velocidad = player.velocidad;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        

        
        /*for (int i = 1; i < 11; i++)
        {
            Debug.Log(Mathf.RoundToInt(aMultiplier * Mathf.Log(bMultiplier * i) + cMultiplier * Mathf.Pow(i, 2) + dMultiplier * Mathf.Pow(i, 3)));
        }
        Debug.Log(expNeedPassLevel);*/

        expNeedPassLevel = Mathf.RoundToInt(aMultiplier * Mathf.Log(bMultiplier * nivel) + cMultiplier * Mathf.Pow(nivel, 2) + dMultiplier * Mathf.Pow(nivel, 3));

        GameManager.instance.player = this;
        LevelSystem.instance.player = this;
        GameManager.instance.player = this;
        DialogueSystem.instance.player = this;

    }

    

    private void OnEnable()
    {
        InputManager.playerControls.Player.Correr.performed += GetComponent<PlayerMove>().GetRunInput;  
        InputManager.playerControls.Player.Correr.canceled += GetComponent<PlayerMove>().GetRunInput;

        InputManager.playerControls.Player.Interactuar.performed += GetInputSubmit;
        InputManager.playerControls.Player.Interactuar.canceled += GetInputSubmit;

       
    }

    private void OnDisable()
    {
        InputManager.playerControls.Player.Correr.performed -= GetComponent<PlayerMove>().GetRunInput;
        InputManager.playerControls.Player.Correr.canceled -= GetComponent<PlayerMove>().GetRunInput;

        InputManager.playerControls.Player.Interactuar.performed -= GetInputSubmit; 
        InputManager.playerControls.Player.Interactuar.canceled -= GetInputSubmit; 

    }


    #region Inputs

    public void GetInputSubmit(InputAction.CallbackContext context)
    {
        if (context.performed && mostradoMensaje)
        {
            DialogueSystem.instance.ResetDialogue();
            mostradoMensaje = false;
        }

        if (context.canceled && isCofre)
        {
            isCofre = false;
            mostradoMensaje = true;
            DialogueSystem.instance.MostrarLootCofre(cofreCogido);
            SumarOro(cofreCogido.SumarCofre());
            cofreCogido.gameObject.SetActive(false);
            iconoInteractuar.SetActive(false);
        }
    }
    #endregion

    #region SubirNivel

    public void SumarExp(int cantidad)
    {
        expActual += cantidad;

        if (expActual >= expNeedPassLevel)
        {
            levelUp();
        }
    }

    public void levelUp()
    {
        nivel++;
        CalculateMaxExpLevel();

        //sumar stats
        
        maxHp += 10;
        damage += 1;
        defensa += 1;
        maxMana += 10;
        velocidad += 1;

    }

    void CalculateMaxExpLevel()
    {
        //expNeedPassLevel = (2 * Mathf.Pow((Mathf.Pow(2,0.2f),(nivel + 1 - 2)) + 15) + expActual;
        int expBeforePassLvl = expNeedPassLevel;
        expNeedPassLevel = (Mathf.RoundToInt(aMultiplier * Mathf.Log(bMultiplier * nivel) + cMultiplier * Mathf.Pow(nivel, 2) + dMultiplier * Mathf.Pow(nivel, 3)));
        differenceExp = expBeforePassLvl;
    }

    #endregion

    #region Stats
    public bool TakingDamage(int damage)
    {
        hP = (hP - Mathf.Abs(damage - defensa));
        GetComponentInChildren<DamageNumber>().RotateNumber(0);
        GetComponentInChildren<DamageNumber>().SetNumber(Mathf.Abs(damage - defensa));

        if (hP <= 0)
        {
            hP = 0;
            return true;
        }

        return false;
    }

    public void SumarOro(int oro)
    {
        this.oro += oro;
    }

    public bool ComprarGacha(int coste)
    {
        if (oro >= coste)
        {
            oro -= coste;
            return true;
        }

        return false;
    }
    public void Dormir()
    {
        FadeOutManager.instance.ActivarFadeOut();

        this.hP = maxHp;
    }

    #endregion

    #region Colliders
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cofre") && DialogueSystem.instance.isHabladoGuardia)
        {

            iconoInteractuar.SetActive(true);
            isCofre = true;
            cofreCogido = other.GetComponent<Cofre>();

        }

        if (other.CompareTag("BuscaPelea"))
        {
            BattleController.instance.playerBuscaPelea = true;
            BattleController.instance.player = this;
        }

        if (other.CompareTag("Tienda"))
        {
            DialogueSystem.instance.isActive = true;
            other.gameObject.GetComponentInParent<Vendedor>().enabled = true;
            iconoInteractuar.SetActive(true);
        }

        if (other.CompareTag("PrimeraPelea"))
        {
            DialogueSystem.instance.isActive = true;
            BattleController.instance.player = this;
            other.gameObject.GetComponentInParent<Vendedor>().enabled = true;
            BattleController.instance.guardiaPrimeraBatalla = other.gameObject.GetComponentInParent<Vendedor>().gameObject;
        }

        if (other.CompareTag("Bosque"))
        {
            StartCoroutine(TPMundo(bosque));
        }

        if (other.CompareTag("Ciudad"))
        {
            StartCoroutine(TPMundo(ciudad));
        }
    }

    public IEnumerator TPMundo(Transform pos)
    {
        FadeOutManager.instance.ActivarFadeOut();

        yield return new WaitForSeconds(1f);
       
        GetComponent<PlayerMove>().cc.enabled = false;
        transform.position = pos.position;
        GetComponent<PlayerMove>().cc.enabled = true;

        if (BattleController.instance.isPrimeraPelea)
        {
            Mensaje();
            BattleController.instance.isPrimeraPelea = false;
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BuscaPelea"))
        {
            BattleController.instance.playerBuscaPelea = false;
        }

        if (other.CompareTag("Tienda"))
        {
            DialogueSystem.instance.isActive = false;
            other.gameObject.GetComponentInParent<Vendedor>().enabled = false;
            iconoInteractuar.SetActive(false);
        }

        if (other.CompareTag("Cofre"))
        {
            isCofre = false;
            iconoInteractuar.SetActive(false);
        }
    }

    #endregion

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData player = SaveSystem.LoadPlayer();

        hP = player.hP;
        damage = player.damage;
        defensa = player.defensa;
        mana = player.mana;
        velocidad = player.velocidad;
        gameObject.transform.position = new Vector3(player.positionX, player.positionY, player.positionZ);
    }

    public void Mensaje()
    {
        DialogueSystem.instance.StartConversation(conversacion, playerSprite);
        mostradoMensaje = true;
    }
}
