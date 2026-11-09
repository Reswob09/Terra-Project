using System;
using TMPro;
using UnityEngine;

public class PlayerNameTag2 : MonoBehaviour
{
  
[SerializeField] private TMP_Text nameText1;
[SerializeField] private string playerName1 = "Player 2";

public string PlayerName => playerName1;


    void Awake()
    {
        if (nameText1 == null)
        nameText1 = GetComponentInChildren<TMP_Text>();


    SetName(playerName1);
        
    }

    public void SetName(String newName)
    {
        Debug.Log("SetName called on " + gameObject.name + "with: " + newName, this);
        playerName1 = newName;
        nameText1.text = newName;
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
