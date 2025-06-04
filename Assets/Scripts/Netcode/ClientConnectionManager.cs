using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ClientConnectionManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField addressField;
    [SerializeField] private TMP_InputField portField;
    [SerializeField] private TMP_Dropdown connectionModeDropdown;
    [SerializeField] private TMP_Dropdown teamDropdown;
    [SerializeField] private Button connectButton;
    
    private ushort Port => ushort.Parse(portField.text);
    private string Address => addressField.text;

    private void OnEnable()
    {
        connectionModeDropdown.onValueChanged.AddListener(OnConnectionModeChanged);
        connectButton.onClick.AddListener(OnButtonConnect);
        OnConnectionModeChanged(connectionModeDropdown.value);
    }

    private void OnDisable()
    {
        connectionModeDropdown.onValueChanged.RemoveAllListeners();
        connectButton.onClick.RemoveAllListeners();
    }

    private void OnConnectionModeChanged(int connectionMode)
    {
        string buttonLabel;
        connectButton.enabled = true;

        switch (connectionMode)
        {
            case 0:
                buttonLabel = "Start Host";
                break;
            case 1:
                buttonLabel = "Start Server";
                break;
            case 2:
                buttonLabel = "Start Client";
                break;
            default:
                buttonLabel = "<ERROR>";
                connectButton.enabled = false;
                break;
        }
        
        var buttonText = connectButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = buttonLabel;
    }

    private void OnButtonConnect()
    {
        DestroyLocalSimulationWorld();
        //TODO: Fix this up later to connect to passed in scene as we will have more than one scene
        SceneManager.LoadScene(1);

        switch (connectionModeDropdown.value)
        {
            case 0:
                StartServer();
                StartClient();
                break;
            case 1:
                StartServer();
                break;
            case 2:
                StartClient();
                break;
            default:
                Debug.LogError("Error: Unknown connection mode", gameObject);
                break;
        }
    }

    private static void DestroyLocalSimulationWorld()
    {
        foreach (var world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                world.Dispose();
                break;
            }
        }
    }

    private void StartServer()
    {
        
    }

    private void StartClient()
    {
        
    }
}
