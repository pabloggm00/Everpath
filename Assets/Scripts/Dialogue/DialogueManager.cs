using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TMP_Text textoDialogo;
    public GameObject dialoguePanel;
    public Image sprite;
    public Button[] botones;

    private void Start()
    {
        DialogueSystem.instance.textoDialogo = textoDialogo;
        DialogueSystem.instance.dialoguePanel = dialoguePanel;
        DialogueSystem.instance.spriteHUD = sprite;

        DialogueSystem.instance.botones = new Button[botones.Length];

        for (int i = 0; i < botones.Length; i++)
        {
            DialogueSystem.instance.botones[i] = botones[i];
        }
    }

    public void Salir()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }

        DialogueSystem.instance.ResetDialogue();

        dialoguePanel.SetActive(false);
    }


}
