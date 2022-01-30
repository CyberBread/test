using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private TurnController turnController;

    [SerializeField] private Button startButton;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private InputField unitCountInputField;


    private void Update()
    {
        if (turnController.isTurnDone && !endTurnButton.IsActive())
        {
            endTurnButton.gameObject.SetActive(true);
        }
    }

    public void SpawnOnButtonClick()
    {
        int unitCount = 1;
        int.TryParse(unitCountInputField.text, out unitCount);

        spawner.UnitCount = unitCount;
        spawner.Spawn();
        startButton.gameObject.SetActive(false);
        unitCountInputField.gameObject.SetActive(false);
        endTurnButton.gameObject.SetActive(true);
    }

    public void EndTurnOnButtonClick()
    {
        turnController.EndTurn();
        endTurnButton.gameObject.SetActive(false);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} join the room ");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} left the room ");
    }
}
