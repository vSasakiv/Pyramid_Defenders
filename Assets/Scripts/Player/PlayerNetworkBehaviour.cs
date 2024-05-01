using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkBehaviour : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener playerAudioListener;
    
    private void Start()
    {
        if (IsOwner)
        {
            playerCamera.enabled = true;
            playerAudioListener.enabled = true;
        }
    }
}
