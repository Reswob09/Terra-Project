using System.Reflection;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public PlayerMovement player;
    public float currentVelocity;
    public float currentHeight;
    public TMP_Text moveSpeed;
    public TMP_Text moveHeight;
    public TMP_Text objectIdentifier;
    // ^^^ this TMP_Text will be for debugging
    
    void Start()
    {
        
    }

   
    void Update()
    {
      if(player == null) return;
        {
            moveSpeed.text = $"Speed: {player.currentVelocity:F1}";
        }

        if(player == null) return;
        {
            moveHeight.text = $"Height: {player.currentHeight:F1}";
        }
    }
}
