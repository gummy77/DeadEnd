using Helper;
using Player;
using UnityEngine;

public class CardPlayer : MonoBehaviour
{
    private static readonly int FlipCards = Animator.StringToHash("FlipCards");
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private Vector3 lookAtCardsTarget;
    [SerializeField] private DialogueSpeaker speaker;

    [SerializeField] private int[] failedDialogueIndex;
    [SerializeField] private int successDialogueIndex;
    
    [Space(20)]
    
    [SerializeField] private Renderer[] cardRenderers;
    [SerializeField] private Texture2D[] cardTextures;
    
    private PlayerController _playerController;

    public void StartPlaying()
    {
        if (!_playerController)
        {
            _playerController = FindAnyObjectByType<PlayerController>();
        }
        
        _playerController.movementController.LockMovement();
        _playerController.cameraController.StartLookingAt(GetLookPosition(), 15);
        speaker.SetEnabled(false);
        
        // set cards up

        foreach (var cardRenderer in cardRenderers)
        {
            Material newMaterial = Instantiate(cardRenderer.material);
            newMaterial.mainTexture = cardTextures[Random.Range(0, cardTextures.Length)];
            cardRenderer.material = newMaterial;
        }
        
        cardAnimator.SetTrigger(FlipCards);
    }

    public void FinishedFlipping()
    {
        WaitForResult().DiscardAwaitable(nameof(WaitForResult));
    }

    private async Awaitable WaitForResult()
    {
        await Awaitable.WaitForSecondsAsync(1.5f);
        _playerController.speakController.StartTalkingTo(speaker, failedDialogueIndex[Random.Range(0,  failedDialogueIndex.Length)], false);
    }

    public void FinishedUnFlipping()
    {
        _playerController.cameraController.StopLookingAt();
        _playerController.movementController.UnlockMovement();
        speaker.SetEnabled(true);
    }
    
    public Vector3 GetLookPosition()
    {
        return transform.position + (transform.rotation * lookAtCardsTarget);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(GetLookPosition(), 0.25f);
    }
}
