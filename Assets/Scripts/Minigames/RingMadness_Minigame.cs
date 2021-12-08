using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Voidless;
using System;

using Random = UnityEngine.Random;

namespace Flamingo
{
    [Serializable]
    public class RingMadness_Minigame : MiniGame
    {
        private List<PoolGameObject> _ringsList = new List<PoolGameObject>();                            /// <summary>List of rings that are spawn.</summary>
        [SerializeField] private int[] _endSoundEffectsIndex;

        [Header("Rings Spawn Properties")]
        [SerializeField] private int _ringIndex;                                                         /// <summary>Index of the types of rings to spawn.</summary>
        [SerializeField] private int _ringAmount;                                                        /// <summary>Limit of rings to be spawn.</summary>  
        [SerializeField] private LayerMask _ringsMask;                                                   /// <summary>LayerMask of the rings for overlaping.</summary>
        [SerializeField] private float _overlapRadius;                                                   /// <summary>Radius for the overlap Circle to use.</summary>
        [SerializeField] private GameObjectTag[] _ballTag;                                               /// <summary>Detectable tag for the rings to use.</summary>
        private Vector3 spawnPoint;                                                                      /// <summary>Reference position for spawn rings.</summary>
        private float[] _zRotationsRange = new float[] { 0f, 45f, -45f, 90 };
        
        [Header("Rings Passed Variables")]
        private int _score = 0;                                                                          /// <summary>Player Score.</summary>
        [SerializeField] private int _soundEffectIndex;                                                  /// <summary>Sound Effect's Index on the Game's Data.</summary>
        [SerializeField] private int _particleEffectIndex; 	                                             /// <summary>Particle Effect's Index on the Game's Data.</summary>
        [Header("Time Variables")]
        [SerializeField] public Clock _ringMadnessClock;                                                 /// <summary>Clock class for the minigame.</summary>
        [SerializeField] private float _limitMinutesAmount;
        [Header("UI Variables")]
        [SerializeField] private TextMesh _timerText;


#region Set / Get

        public int ringIndex
        {
            set { _ringIndex = value; }
            get { return _ringIndex; }
        }
        /// <summary>Gets and Sets ringList property.</summary>
        public List<PoolGameObject> ringList
        {
            set { _ringsList = value; }
            get { return _ringsList; }
        }

        /// <summary>Gets and Sets ringMask property.</summary>
        public LayerMask ringMask
        {
            set { _ringsMask = value; }
            get { return _ringsMask; }
        }
        /// <summary>Gets and Sets overlapRadius property.</summary>
        public float overlapRadius
        {
            set { _overlapRadius = value; }
            get { return _overlapRadius; }
        }
        /// <summary>Gets and Sets ballTag property.</summary>
        public GameObjectTag[] ballTag
        {
            set { _ballTag = value; }
            get { return _ballTag; }
            
        }

        /// <summary>Gets and Sets ringAmount property.</summary>
        public int ringAmount
        {
            set { _ringAmount = value; }
            get { return _ringAmount; }
        }

        /// <summary>Gets and Sets score property.</summary>
        public int score
        {
            set { _score = value; }
            get { return _score; }
        }
        ///<summary>Gets and Sets ringMadnessClock  .</summary>
        public Clock ringMadnessClock
        {
            set { _ringMadnessClock = value;}
            get{ return _ringMadnessClock;}
        }
        /// <summary>Gets and Sets limitMinutesAmount property.</summary>
        public float limitMinutesAmount
        {
            set { _limitMinutesAmount = value;}
            get{ return _limitMinutesAmount;}
        }

        /// <summary>Gets and Sets soundEffectIndex property.</summary>
        public int soundEffectIndex
        {
            get { return _soundEffectIndex; }
            set { _soundEffectIndex = value; }
        }

        /// <summary>Gets and Sets particleEffectIndex property.</summary>
        public int particleEffectIndex
        {
            get { return _particleEffectIndex; }
            set { _particleEffectIndex = value; }
        }

        /// <summary>Gets and Sets timerText property.</summary>
        public TextMesh timerText 
        {
            get { return _timerText; }
            set { _timerText = value; }
        }

#endregion


#region MinigameCoroutine

        protected override IEnumerator MiniGameCoroutine()
        {
           CreateRings_NoOverlap();
            SubscribeRing();


            //RingMadnessTimer

            while(ringMadnessClock.minutes < limitMinutesAmount)
            {
                yield return new WaitForEndOfFrame();
                ringMadnessClock.Update(Time.deltaTime);
                timerText.text = ringMadnessClock.ToString();
            }

            EndRingMadness();
            
            yield return null;
        }

        /// <summary>Create PoolObjects if there is no other PoolObject in the area.</summary>

        void CreateRings_NoOverlap(){
            while (ringList.Count < ringAmount)
            {
                float new_x = Random.Range(-29f, 33.9f);
                float new_y = Random.Range(0.43f, 5.48f);
                spawnPoint = new Vector3(new_x, new_y, 0f);
                   
                if (Physics2D.OverlapCircle(new Vector2(new_x,new_y),overlapRadius,ringMask)) 
                {
                    continue;
                }
                else
                {
                    PoolGameObject TempRing = PoolManager.RequestPoolGameObject(ringIndex, spawnPoint, Quaternion.Euler(0f, 0f, RandomRotation()));
                    ringList.Add(TempRing);

                }

            }


        }

        
        private float RandomRotation()
        {
            int randomSetter = Random.Range(0, 3);
          
            switch (randomSetter)
            {

                case 0:
                    return  _zRotationsRange[0];
                    break;
                case 1:
                    return _zRotationsRange[1];
                    break;
                case 2:
                    return _zRotationsRange[2];
                    break;
                case 3:
                    return  _zRotationsRange[3];
                    break;

                default:
                    return 0.0f;
                    break;
            }
        }
#endregion

#region Functions


        void OnDeactivation(IPoolObject _poolObject)
        {
            _poolObject.onPoolObjectDeactivation += OnDeactivation;

        }
        

        /// <summary>Overwrite for Rings delegate .</summary>
        public void OnRingPassed(Collider2D _collider)
        {
            score++;
            int particlesIndex = particleEffectIndex;
            Vector3 point = _collider.transform.position;
            Vector3 direction = _collider.transform.position;
            ParticleEffect particleEffect = PoolManager.RequestParticleEffect(particlesIndex, point, VQuaternion.RightLookRotation(direction));
            AudioController.PlayOneShot(SourceType.Scenario, 0, soundEffectIndex);
        }

        /// <summary>Sets delegate functions to PoolObjects.</summary>
        void SubscribeRing()
        {
            foreach(Ring ring in _ringsList)
            {
                ring.onRingPassed += OnRingPassed;
            }
        }

        void UnsubscribeRing()
        {
            foreach(Ring ring in _ringsList)
            {
                ring.onRingPassed -= OnRingPassed;
            }
        }


        void EndRingMadness()
        {
            UnsubscribeRing();
            switch(score)
            {
                case 0:
                    AudioController.PlayOneShot(SourceType.Scenario, 0, _endSoundEffectsIndex[0]);
                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                     AudioController.PlayOneShot(SourceType.Scenario, 0, _endSoundEffectsIndex[1]);
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                     AudioController.PlayOneShot(SourceType.Scenario, 0, _endSoundEffectsIndex[2]);
                    break;
                case 10:
                    AudioController.PlayOneShot(SourceType.Scenario, 0, _endSoundEffectsIndex[3]);
                    break;

                default :

                    break;

            }
            timerText.text = "Final score: \n" + score.ToString() + "/" + ringAmount.ToString();


        }


#endregion
    }

}
