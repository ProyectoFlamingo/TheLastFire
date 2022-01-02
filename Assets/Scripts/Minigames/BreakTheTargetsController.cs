using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo {
    public class BreakTheTargetsController : MonoBehaviour
    {
        [SerializeField] private BreakTheTargetsMiniGame _breakTheTargetsMiniGame;
        public BreakTheTargetsMiniGame breakTheTargetsMiniGame { get { return _breakTheTargetsMiniGame; } }

        private void Start()
        {

        }
        void Awake()
        {
            breakTheTargetsMiniGame.Initialize(this, MiniGameEvent);
        }


        public void MiniGameEvent(MiniGame miniGame, int ID)
        {

        }

        public void OnDestroy()
        {
            breakTheTargetsMiniGame.Terminate();

        }

        public void EndMiniGameBtt()
        {
            breakTheTargetsMiniGame.Terminate();
        }


      
        /*
        public void ClearList (){
            breakTheTargetsMiniGame.targetList.Clear();      
        }*/


    }



}
