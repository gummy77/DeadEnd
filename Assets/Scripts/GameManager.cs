using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject loadingScreeGameObject;
    
    [Header("Settings")]
    [SerializeField] private string lobbyid = "GOOBER";
    
    async Awaitable Start()
    {
        loadingScreeGameObject?.SetActive(true);
        DontDestroyOnLoad(this);
        
        try
        {
            SetupUnityServiceEvents();
            await UnityServices.InitializeAsync();

            SetupAuthenticationServiceEvents();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            SessionOptions sessionOptions = new SessionOptions()
            {
                MaxPlayers = 50
            }.WithDistributedAuthorityNetwork();

            SetupMultiplayerServiceEvents();
            await MultiplayerService.Instance.CreateOrJoinSessionAsync(lobbyid, sessionOptions);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void SetupUnityServiceEvents()
    {
        UnityServices.Initialized += () =>
        {
            Debug.Log("Unity Services Initialized");
        };
        UnityServices.InitializeFailed += (exception) =>
        {
            Debug.LogError(exception);
        };
    }

    private void SetupAuthenticationServiceEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log($"Player signed in with PlayerID: {AuthenticationService.Instance.PlayerId}");
        };
        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };
        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };
        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    private void SetupMultiplayerServiceEvents()
    {
        MultiplayerService.Instance.SessionAdded += (ISession session) =>
        {
            Debug.Log($"Session added: {session.Id} -> {session.Code}");
            loadingScreeGameObject?.SetActive(false);
        };
        MultiplayerService.Instance.AddingSessionFailed += (AddingSessionOptions options, SessionException exception) =>
        {
            Debug.LogError($"Session Adding Failed\n{exception}");
        };
        MultiplayerService.Instance.SessionRemoved += (ISession session) =>
        {
            Debug.Log($"Session removed: {session.Id} -> {session.Code}");
        };
    }
}
