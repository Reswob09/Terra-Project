using System;
using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviour
{
  
[SerializeField] private TMP_Text nameText;
[SerializeField] private string playerName = "Player 1";

public string PlayerName1 => playerName;


    void Awake()
    {
        if (nameText == null)
        nameText = GetComponentInChildren<TMP_Text>();


    SetName(playerName);
        
    }

    public void SetName(String newName)
    {
        Debug.Log("SetName called on " + gameObject.name + "with: " + newName, this);
        playerName = newName;
        nameText.text = newName;
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
