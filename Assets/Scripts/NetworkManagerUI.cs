using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            NetworkLog.LogInfo("Server Started");
        });
        
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            NetworkLog.LogInfo("Host Started");
        });
        
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            NetworkLog.LogInfo("Client Started");
        });
    }
}
