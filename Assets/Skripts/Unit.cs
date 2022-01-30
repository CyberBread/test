using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (JumpCommand))]
[RequireComponent(typeof (ShotCommand))]

public class Unit : MonoBehaviour
{
    public ICommand jumpCommand;
    public ICommand shotCommand;

    [SerializeField] private float healthPoint = 10f;
    [SerializeField] private float painTime = 2f;

    [SerializeField] GameObject halo;

    private bool selected = false;
    private PhotonView photonView;

    public bool isSelected
    {
        get { return selected; }
        private set { selected = value; }
    }

    public PhotonView GetPhotonView
    {
        get { return photonView; }
    }

    public bool isMine
    {
        get {return photonView.IsMine; }
    }

    private void Start()
    {
        jumpCommand = GetComponent<JumpCommand>();
        shotCommand = GetComponent<ShotCommand>();
        photonView = GetComponent<PhotonView>();
    }

    public void Select()
    {
        isSelected = true;
        Highlight(true);
    }

    public void UnSelect()
    {
        isSelected = false;
        Highlight(false);
    }

    private void Highlight(bool active)
    {
        halo.SetActive(active);
    }


    public void GetDamage(float damage)
    {
        healthPoint -= damage;
        if (healthPoint < 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
        else
        {
            photonView.RPC("ChangeColorToRed", RpcTarget.All);
        }
    }
    [PunRPC]
    private void Die()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    private void ChangeColorToRed()
    {
        StartCoroutine(TurnRed(painTime));
    }

    private IEnumerator TurnRed(float painTime)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(painTime);

        spriteRenderer.color = Color.white;

    }
}
