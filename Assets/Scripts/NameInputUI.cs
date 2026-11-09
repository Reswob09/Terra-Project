using TMPro;
using UnityEngine;

public class NameInputUI : MonoBehaviour
{

[SerializeField] private TMP_InputField inputField;
[SerializeField] private PlayerNameTag targetPlayer;



    void Start()
    {
        inputField.onEndEdit.AddListener(OnNameEntered);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void OnNameEntered(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            targetPlayer.SetName(value.Trim());
        }
    }
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
}
