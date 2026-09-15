using System;
using Player;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject loadingScreenGameObject;
    [SerializeField] private GameObject failureTextGameObject;

    public static Action LobbyStarted;

    private void Awake()
    {
        PlayerController.PlayerSpawned += (player) =>
        {
            loadingScreenGameObject.SetActive(false);
        };
    }

    async Awaitable Start()
    {
        loadingScreenGameObject?.SetActive(true);
        DontDestroyOnLoad(this);

        try
        {
            SetupUnityServiceEvents();
            await UnityServices.InitializeAsync();
        
            SetupAuthenticationServiceEvents();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            SetupMultiplayerServiceEvents();
            // var queryOptions = new QuerySessionsOptions();
            // var sessions = await MultiplayerService.Instance.QuerySessionsAsync(queryOptions);
            //
            // if (sessions.Sessions.Count > 0)
            // {
            //     // Join Session
            //     foreach (var session in sessions.Sessions)
            //     {
            //         if (!session.IsLocked && !session.HasPassword && session.AvailableSlots > 0)
            //         {
            //             ISessionInfo sessionInfo = sessions.Sessions[0];
            //             await MultiplayerService.Instance.JoinSessionByIdAsync(sessionInfo.Id);
            //             break;
            //         }
            //     }
            // }
            // else
            // {
                //Host new Session
                var options = new SessionOptions
                {
                    MaxPlayers = 150
                }.WithDistributedAuthorityNetwork();
                var session = await MultiplayerService.Instance.CreateSessionAsync(options);
            // }
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
            failureTextGameObject?.SetActive(true);
        };
    }

    private void SetupAuthenticationServiceEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log($"Player signed in with PlayerID: {AuthenticationService.Instance.PlayerId}");
        };
        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
            failureTextGameObject?.SetActive(true);
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
            LobbyStarted.Invoke();
        };
        MultiplayerService.Instance.AddingSessionFailed += (AddingSessionOptions options, SessionException exception) =>
        {
            Debug.LogError($"Session Adding Failed\n{exception}");
            failureTextGameObject?.SetActive(true);
        };
        MultiplayerService.Instance.SessionRemoved += (ISession session) =>
        {
            Debug.Log($"Session removed: {session.Id} -> {session.Code}");
        };
    }
}
