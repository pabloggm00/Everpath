using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{

    [Header("GACHAPON")]
    public static LootGenerator instance;
    public List<GameObject> listaLoot;
    public GameObject primeraTirada;
    public GameObject segundaTirada;
    public int costeTirada;

    [Header("Probabilidad")]
    public int probGris;
    public int probaVerde;
    public int probAzul;
    public int probMorado;
    public int probDorada;

    int contPrimeraSegundaTirada = 0;
    [HideInInspector] public Equipment premio;

    Dictionary<Rareza, int> probabilidades;

   
    public Consumibles pocion;

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
        probabilidades = new Dictionary<Rareza, int>
        {
            {Rareza.Gris, probGris},
            {Rareza.Verde, probaVerde},
            {Rareza.Azul, probAzul},
            {Rareza.Morado, probMorado},
            {Rareza.Dorada, probDorada}
        };
    }


    #region Gachapon

    //Llamo a este metodo para que realice el gachapon y según lo que toque se agregue al inventario
    public void GetLootGacha()
    {
        switch (LootRareza())
        {
            case Rareza.Nula:
                break;
            case Rareza.Gris:
                InventoryManager.instance.AddEquipment( premio = AleatorioPorRareza(Rareza.Gris));
                break;
            case Rareza.Verde:
                if (contPrimeraSegundaTirada < 2)
                {
                    if (contPrimeraSegundaTirada == 0)
                    {

                        InventoryManager.instance.AddEquipment(primeraTirada.GetComponent<Equipment>());
                        premio = primeraTirada.GetComponent<Equipment>();

                    }
                    else if (contPrimeraSegundaTirada == 1)
                    {

                        InventoryManager.instance.AddEquipment(segundaTirada.GetComponent<Equipment>());
                        premio = segundaTirada.GetComponent<Equipment>();
                    }

                    contPrimeraSegundaTirada++;
                }
                else
                {
                    InventoryManager.instance.AddEquipment( premio = AleatorioPorRareza(Rareza.Verde));
                }
                
                break;
            case Rareza.Azul:
                InventoryManager.instance.AddEquipment(premio = AleatorioPorRareza(Rareza.Azul));
                break;
            case Rareza.Morado:
                InventoryManager.instance.AddEquipment(premio = AleatorioPorRareza(Rareza.Morado));
                break;
            case Rareza.Dorada:
                
                InventoryManager.instance.AddEquipment(premio =  AleatorioPorRareza(Rareza.Dorada));
                break;
            default:
                break;
        }

    }

    //toma las probabilidades por rareza y devuelve la rareza tocada
    Rareza LootRareza()
    {

        if (contPrimeraSegundaTirada < 2)
        {
            return Rareza.Verde;
        }
        else
        {
            int totalProbabilidades = 0;

            foreach (var prob in probabilidades)
            {
                totalProbabilidades += prob.Value;
            }

            float numAleatorio = Random.Range(0, totalProbabilidades - 1);

            foreach (var prob in probabilidades)
            {
                numAleatorio -= prob.Value;

                if (numAleatorio < 0)
                {

                    return prob.Key;
                }
            }

            return Rareza.Nula;
        }


        
    }



    //Despues de conseguir qué rareza toca, coge aleatoriamente un equipment de esa rareza
    public Equipment AleatorioPorRareza(Rareza rarezaTocada)
    {


        List<Equipment> listaRareza = new List<Equipment>();

        for (int i = 0; i < listaLoot.Count; i++)
        {
            if (rarezaTocada == listaLoot[i].GetComponent<Equipment>().rareza)
            {
                listaRareza.Add(listaLoot[i].GetComponent<Equipment>());
                
            }
        }

        int random = Random.Range(0, listaRareza.Count);

      
        return listaRareza[random].GetComponent<Equipment>();

     
    }

    #endregion


    #region Cofre

    //toma las probabilidades por rareza y devuelve la rareza tocada
    public Varios LootBatalla1(Varios loot)
    {

        int totalProbabilidades = 0;

       
        totalProbabilidades += loot.probabilidad;
        

        float numAleatorio = Random.Range(0, totalProbabilidades - 1);

        
        numAleatorio -= loot.probabilidad;

        if (numAleatorio < 0)
        {

            return loot;
        }
        

        return null;

    }

    public Consumibles LootBatalla2()
    {
        int totalProbabilidades = 0;

        
        totalProbabilidades += pocion.probabilidad;
        

        float numAleatorio = Random.Range(0, totalProbabilidades - 1);

       
        numAleatorio -= pocion.probabilidad;

        if (numAleatorio < 0)
        {

            return pocion;
        }
        

        return null;
    }




    #endregion

}
