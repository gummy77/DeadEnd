using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class EyeThing : MonoBehaviour
    {
        [SerializeField] private List<GameObject> eyeObjects;

        [SerializeField] private Speaker noneFoundSpeaker;
        [SerializeField] private Speaker someFoundSpeaker;
        [SerializeField] private Speaker allFoundSpeaker;
        
        public void AddEye()
        {
            int index = Random.Range(0, eyeObjects.Count);
            eyeObjects[index].SetActive(true);
            eyeObjects.RemoveAt(index);

            if (eyeObjects.Count > 1)
            {
                noneFoundSpeaker.enabled = false;
                someFoundSpeaker.enabled = true;
            }
            else
            {
                someFoundSpeaker.enabled = false;
                allFoundSpeaker.enabled = true;
            }
        }
    }
}
