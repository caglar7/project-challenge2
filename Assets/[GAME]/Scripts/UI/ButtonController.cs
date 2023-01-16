using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// button listener for button events
/// </summary>

public enum ButtonActionType
{
    ReloadLevel,
}

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    [SerializeField] ButtonActionType actionType;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonAction);
    }

    private void ButtonAction()
    {
        switch (actionType)
        {
            case ButtonActionType.ReloadLevel:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

        }
    }
}
