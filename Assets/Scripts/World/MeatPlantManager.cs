using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class MeatPlantManager : MonoBehaviour
    {
        [SerializeField] private int meatPlantsForState1;
        [SerializeField] private int meatPlantsForState2;

        [SerializeField] private UnityEvent state1Actions;
        [SerializeField] private UnityEvent state2Actions;
        
        public static Action<MeatPlant> MeatPlantInit;
        public static Action<MeatPlant> MeatPlantCut;
        
        private List<MeatPlant> _meatPlants;
        private float _cutPlants = 0;
        private bool _state1Complete;
        private bool _state2Complete;

        private void Awake()
        {
            MeatPlantInit += NewPlant;
            MeatPlantCut += PlantCut;
            _meatPlants = new List<MeatPlant>();
        }

        private void NewPlant(MeatPlant meatPlant)
        {
            _meatPlants.Add(meatPlant);
        }

        private void PlantCut(MeatPlant meatPlant)
        {
            _cutPlants++;
            if (_cutPlants >= meatPlantsForState1)
            {
                state1Actions.Invoke();
                _state1Complete = true;
            }

            if (_cutPlants >= meatPlantsForState2)
            {
                state2Actions.Invoke();
                _state2Complete = true;
            }
        }
    }
}
