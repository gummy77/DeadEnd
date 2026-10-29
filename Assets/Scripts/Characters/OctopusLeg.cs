using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class OctopusLeg : MonoBehaviour
{
    private List<Transform> _legBones;

    float _randomOffset;
    
    private void Start()
    {
        GetLegs();
        
    }

    [ContextMenu("Reset Legs")]
    private void GetLegs()
    {
        _randomOffset = Random.Range(-5f, 5f);
        _legBones = new List<Transform>();
        
        Transform currentBone = transform.GetChild(0);
        while (currentBone.childCount > 0)
        {
            _legBones.Add(currentBone);
            currentBone = currentBone.GetChild(0);
        }
    }
    
    private void Update()
    {
        if (_legBones.Count <= 0 || _randomOffset == 0)
        {
            GetLegs();
        }

        for (var index = 0; index < _legBones.Count; index++)
        {
            float legDelta = index / (float)_legBones.Count;
            
            float angle = Mathf.Lerp(-80f * legDelta, 40f, (Mathf.Cos((legDelta * 9f)- Time.time + _randomOffset) + 1f) / 2f);
            
            angle *= 0.5f + legDelta;
            
            //angle += (Mathf.Sin(Time.time + _randomOffset) * 3);
            
            _legBones[index].localRotation = Quaternion.Euler(angle, 0, 0);
        }
    }
}
