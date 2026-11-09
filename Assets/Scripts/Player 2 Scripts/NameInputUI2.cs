using TMPro;
using UnityEngine;

public class NameInputUI2 : MonoBehaviour
{

[SerializeField] private TMP_InputField inputField1;
[SerializeField] private PlayerNameTag targetPlayer1;



    void Start()
    {
        Debug.Log("NameInputUI started");
        inputField1.onEndEdit.AddListener(OnNameEntered);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void OnNameEntered(string value)
    {
        Debug.Log("Name Entered: " + value);
        if (!string.IsNullOrWhiteSpace(value))
        {
            targetPlayer1.SetName(value.Trim());
        }
    }
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
}
