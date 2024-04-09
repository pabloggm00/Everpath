using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;
    [HideInInspector] public bool playerBuscaPelea;
    public float timeParaBuscarPelea;
    float timePassed;

    public float timeUltimaPelea;
    float timePassedUltimaPelea;
    bool canEmpezarTiempoUltimaPelea;
    

    public GameObject batallaWorld;
    public GameObject mundo;
    public GameObject statsPlayerBatalla;
    public GameObject statsPlayerMundo;
    public GameObject statsLvl;

    [HideInInspector] public bool isPelea;
    [HideInInspector] public bool isStarted;
    [HideInInspector] public bool isTerminadoPelea;
    [HideInInspector] public PlayerController player;
    [HideInInspector] public Vector3 posAntesBatallaPlayer;
    [HideInInspector] public int hpDespuesBatalla;
    [HideInInspector] public GameObject guardiaPrimeraBatalla;
    [HideInInspector] public bool isPrimeraPelea;
    bool encontrado;

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

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        BuscarPelea();   
    }

    public void CargarPrimeraPelea()
    {
        StartCoroutine(EmpezarBatalla());
    }

    IEnumerator EmpezarBatalla()
    {
        FadeOutManager.instance.ActivarFadeOut();

        yield return new WaitForSeconds(1f);

        guardiaPrimeraBatalla.GetComponent<Vendedor>().enabled = false;
        guardiaPrimeraBatalla.SetActive(false);

        canEmpezarTiempoUltimaPelea = true;
        //CargoEscenaPelea
        playerBuscaPelea = false;
        isPelea = true;

        isStarted = true;
        CargarBatalla();
    }

    void BuscarPelea()
    {
     
        if (playerBuscaPelea && !InventoryManager.instance.isInventario && !DialogueSystem.instance.isCofre && !MenuPausa.instance.isPausa)
        {
            timePassed += Time.deltaTime;

            if (timePassed >= timeParaBuscarPelea)
            {
                int rndQuieroPelea = Random.Range(0, 5000);

                if (rndQuieroPelea > 4000 && !canEmpezarTiempoUltimaPelea && !encontrado)
                {
                    StartCoroutine(EmpezarBatalla());
                    encontrado = true;

                }
                else if(rndQuieroPelea <= 4000)
                {
              
                    Debug.Log("No entro");
                }
                timePassed = 0;
            }
        }

        if (canEmpezarTiempoUltimaPelea)
        {
            timePassedUltimaPelea += Time.deltaTime;
  
            if (timePassedUltimaPelea >= timeUltimaPelea)
            {
                canEmpezarTiempoUltimaPelea = false;
                timePassedUltimaPelea = 0;
            }
        }
    }

    void CargarBatalla()
    {
        //Mundo
        mundo.SetActive(false);
        statsLvl.SetActive(false);
        statsPlayerMundo.SetActive(false);

        //Batalla
        batallaWorld.SetActive(true);
        statsPlayerBatalla.SetActive(true);
        encontrado = false;
        
    }

    public void CargarMundo(BattleState stateBattle)
    {

        if (stateBattle == BattleState.LOST)
        {
            StartCoroutine(player.TPMundo(player.posInicial));
            player.hP = player.maxHp;
        }

        

        GameManager.instance.player = this.player;
        LevelSystem.instance.player = this.player;
        GameManager.instance.player = this.player;
        DialogueSystem.instance.player = this.player;

        StartCoroutine(CargaMundo());
        
    }

    public void ActualizarDatosBatlla(PlayerController playerBattle)
    {
        player.hP = playerBattle.hP;
        player.maxHp = playerBattle.maxHp;
        player.damage = playerBattle.damage;
        player.defensa = playerBattle.defensa;
        player.mana = playerBattle.mana;
        player.maxMana = playerBattle.maxMana;
        player.velocidad = playerBattle.velocidad;
        player.expActual = playerBattle.expActual;
        player.expNeedPassLevel = playerBattle.expNeedPassLevel;
        player.nivel = playerBattle.nivel;
        player.differenceExp = playerBattle.differenceExp;
        player.oro = playerBattle.oro;
    }

    IEnumerator CargaMundo()
    {
        yield return new WaitForSeconds(1f);

        //Mundo
        mundo.SetActive(true);
        statsLvl.SetActive(true);
        canEmpezarTiempoUltimaPelea = true;

        //Batalla
        statsPlayerBatalla.SetActive(false);
        isPelea = false;
        batallaWorld.SetActive(false);
        statsPlayerMundo.SetActive(true);

    }

    public void MostrarObjetos(List<Button> botonesConsumibles)
    {
      
       
        for (int i = 0; i < botonesConsumibles.Count; i++)
        {
            if (i < InventoryManager.instance.consumibles.Count)
            {
                botonesConsumibles[i].gameObject.SetActive(true);
                botonesConsumibles[i].GetComponent<Image>().sprite = InventoryManager.instance.consumibles[i].sprite;
                botonesConsumibles[i].GetComponentInChildren<TMP_Text>().enabled = true;
                botonesConsumibles[i].GetComponentInChildren<TMP_Text>().text = "x" + InventoryManager.instance.consumibles[i].valorCuantia.ToString();
                   

            }


            if (botonesConsumibles[i].GetComponent<Image>().sprite.name == "MarcoVacio" || InventoryManager.instance.consumibles.Count == 0)
            {
                botonesConsumibles[i].gameObject.SetActive(false);
                botonesConsumibles[i].GetComponentInChildren<TMP_Text>().enabled = false;
            }

        }
        
        
    }
    
}
