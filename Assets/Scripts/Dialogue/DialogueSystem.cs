using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;



public enum Dialogos { Posada, Tienda, PrimeraPelea }
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    string[] conversation;
    int currentIndex = 0;
    [HideInInspector] public TMP_Text textoDialogo;
    [HideInInspector] public GameObject dialoguePanel;
    [HideInInspector] public Image spriteHUD;
    [HideInInspector] public Button [] botones;

    [HideInInspector] public bool isActive;

    [HideInInspector] public bool isConversacion;
    [HideInInspector] public bool isDecision;
    [HideInInspector] public bool isHabladoGuardia;
    [HideInInspector] public Dialogos sitioDialogo;
    [HideInInspector] public PlayerController player;

    [HideInInspector] public bool isGacha;
    [HideInInspector] public bool isWaitRespuesta;
    [HideInInspector] public bool isCofre;

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
        InputManager.playerControls.Player.Interactuar.performed += GetInputCerrarGacha;
        InputManager.playerControls.Player.Interactuar.canceled += GetInputCerrarGacha;
    }

    private void OnDisable()
    {
        InputManager.playerControls.Player.Interactuar.performed -= GetInputCerrarGacha;
        InputManager.playerControls.Player.Interactuar.canceled -= GetInputCerrarGacha;
    }

    // Update is called once per frame
    void Update()
    {

    
    }

    public void GetInputCerrarGacha(InputAction.CallbackContext context)
    {
        if (context.performed && isGacha)
        {
            ResetDialogue();
            isGacha = false;
        }
    }


    public void StartConversation(string[] conversation, Sprite spritePerso)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }

        spriteHUD.sprite = spritePerso;
        this.conversation = conversation;
        textoDialogo.text = this.conversation[0];
        dialoguePanel.SetActive(true);
        isConversacion = true;
    }

    public void NextSentence(List<string> botonesConversacion)
    {
      
        currentIndex++;

        if (conversation.Length > currentIndex)
        {
            textoDialogo.text = conversation[currentIndex];
        }
        else if (currentIndex == conversation.Length && botonesConversacion.Count != 0)
        {

            isDecision = true;
            for (int i = 0; i < botonesConversacion.Count; i++)
            {

               
                botones[i].gameObject.SetActive(true);
                botones[i].GetComponentInChildren<TMP_Text>().text = botonesConversacion[i];
                isWaitRespuesta = true;

                botones[i].onClick.RemoveAllListeners();
                switch (sitioDialogo)
                {
                    case Dialogos.Posada:
                        if (i == botonesConversacion.Count - 1)
                        {
                          
                            botones[i].onClick.AddListener(delegate () { GameManager.instance.player.Dormir();

                                isWaitRespuesta = false;
                            });

                        }
   
                        
                        break;
                    case Dialogos.Tienda:
                        if (i == botonesConversacion.Count - 1)
                        {
                            botones[i].onClick.AddListener(delegate () { InventoryManager.instance.AbrirInventarioVenta();

                                ResetDialogue();

                            });
                            
                        }else if (i == botonesConversacion.Count - 2)
                        {
                            botones[i].onClick.AddListener(delegate () {


                                if (player.ComprarGacha(LootGenerator.instance.costeTirada))
                                {
                                    LootGenerator.instance.GetLootGacha();

                                    ResetDialogue();
                                    AbrirPanelPremio(LootGenerator.instance.premio);
                                }
                                else
                                {
                                    MostrarDialogoNoTenerDinero();
                                }

                                isWaitRespuesta = false;

                            });
                        }
        

                        break;
                    case Dialogos.PrimeraPelea:


                        if (i == botonesConversacion.Count - 1)
                        {
                            botones[i].onClick.AddListener(delegate () { BattleController.instance.CargarPrimeraPelea();
                                isWaitRespuesta = false;
                            });
                        }
         

                        break;
                    default:
                        break;
                }
                
            }
            EventSystem.current.SetSelectedGameObject(botones[0].gameObject);
            
        }
        else 
        {
            
            ResetDialogue();
        }
    }

    public void ResetDialogue()
    {
        isDecision = false;
        dialoguePanel.SetActive(false);
        isConversacion = false;
        currentIndex = 0;
        isWaitRespuesta = false;
        isCofre = false;
    }

    void MostrarDialogoNoTenerDinero()
    {
        isGacha = true;
        dialoguePanel.SetActive(true);

        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }

        textoDialogo.text = "Cuesta " + LootGenerator.instance.costeTirada + ". No tienes dinero suficiente.";
    }

    public void MostrarLootCofre(Cofre cofreObtenido)
    {
        isCofre = true;
        dialoguePanel.SetActive(true);

        if (cofreObtenido.oro == 0)
        {
            textoDialogo.text = "Has encontrado x" + cofreObtenido.pociones.Count + " " + cofreObtenido.pociones[0].nombre;
        }
        else
        {
            textoDialogo.text = "Has encontrado " + cofreObtenido.oro + " de ORO";
        }
       
    }

    void AbrirPanelPremio(Equipment premio)
    {

        isGacha = true;
        dialoguePanel.SetActive(true);

        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }

      
        string color = "";

        switch (premio.rareza)
        {
            case Rareza.Nula:
                break;
            case Rareza.Gris:
                color = "grey";
                break;
            case Rareza.Verde:
                color = "green";
                break;
            case Rareza.Azul:
                color = "blue";
                break;
            case Rareza.Morado:
                color = "purple";
                break;
            case Rareza.Dorada:
                color = "yellow";
                break;
            default:
                break;
        }

        textoDialogo.text = "¡Enhorabuena! Has conseguido " + "<color="+color + ">" + premio.nameEquipment + "</color>";
    }

}
