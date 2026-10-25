using System.Reflection;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Controller player;
    public TMP_Text moveSpeed;
    FieldInfo currentSpeedField;
    void Start()
    {
        currentSpeedField = typeof(Controller).GetField("CurrentSpeed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public);
    }

   
    void Update()
    {
      if(currentSpeedField != null)
        {
            moveSpeed.SetText("Speed: {0}", (float)currentSpeedField.GetValue(player));
        }
    }
}
