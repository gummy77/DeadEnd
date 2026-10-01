using Helper;
using Pickup;
using UnityEngine;

public class PlantGuy : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject mound;
    [SerializeField] private DialogueSpeaker dialogueSpeaker;
    [SerializeField] private Collider dialogueCollider;

    private Vector3 _bodyStartPosition;
    private Vector3 _moundStartPosition;
    
    public void Awaken()
    {
        _bodyStartPosition = body.transform.localPosition;
        _moundStartPosition = mound.transform.localPosition;
        DoAwaken().DiscardAwaitable(nameof(Awaken));
    }

    private async Awaitable DoAwaken()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            
            body.transform.localPosition = Vector3.Lerp(_bodyStartPosition, Vector3.zero, timer);
            mound.transform.localPosition = Vector3.Lerp(_moundStartPosition, Vector3.zero, timer);
            await Awaitable.NextFrameAsync();
        }
        dialogueSpeaker.SetEnabled(true);
        dialogueCollider.enabled = true;
    }
}
