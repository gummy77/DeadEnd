using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    async Awaitable Start()
    {
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
            };

            SetupMultiplayerServiceEvents();
            await MultiplayerService.Instance.CreateOrJoinSessionAsync("GOOBER", sessionOptions);
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
