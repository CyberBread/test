using System;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

[RequireComponent (typeof (PhotonView))]

public class TurnController : MonoBehaviour, IPunObservable
{
    [SerializeField] private Spawner spawner;

    private Action jumpAction;
    private Action shotAction;

    private PhotonView photonView;

    private bool isThisPlayerReady;
    private bool isOtherPlayerReady;

    public bool isTurnDone { get; private set; }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isThisPlayerReady);
        }
        else
        {
            isOtherPlayerReady =(bool)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if(isTurnDone && !isThisPlayerReady)
        {
            isTurnDone = false;
        }

        if(isThisPlayerReady && isOtherPlayerReady)
        {
            photonView.RPC("PlayActions", RpcTarget.All);
        }
    }

    public void EndTurn()
    {
        Unit[] units = spawner.GetAllUnits();

        foreach(Unit unit in units)
        {
            jumpAction += unit.jumpCommand.Execute;
            shotAction += unit.shotCommand.Execute;
        }

        isThisPlayerReady = true;
    }

    [PunRPC]
    private void PlayActions()
    {
        StartCoroutine(PlayActionsWithDelay(3f));
        StartCoroutine(WaitSomeTimeAndEndTurn());
        isThisPlayerReady = false;
    }

    private IEnumerator WaitSomeTimeAndEndTurn()
    {
        yield return new WaitForSeconds(5);
        isTurnDone = true;
    }

    private IEnumerator PlayActionsWithDelay(float delayBetweenActions)
    {
        jumpAction.Invoke();
        yield return new WaitForSeconds(delayBetweenActions);
        shotAction.Invoke();
    }
}
