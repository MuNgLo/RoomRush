using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyState))]
    public class EnemySounds : MonoBehaviour
    {
        public AudioSource _breathing;
        public AudioSource _feet;
        public AudioClip _scatter;

        private Vector3 _lastStepPos;

        private EnemyState _state;

        // Start is called before the first frame update
        void Start()
        {
            _state = GetComponent<EnemyState>();
            _state.OnStateChange.AddListener(OnEnemyStateChange);
            _lastStepPos = transform.position;
        }

        private void OnEnemyStateChange(ENEMYSTATE newState, ENEMYSTATE oldState)
        {
            // Play breathing sound or not
            if(newState == ENEMYSTATE.IDLE)
            {
                _breathing.Play();
            }
            else
            {
                if (_breathing.isPlaying)
                {
                    _breathing.Stop();
                }
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            // Play scattering soudn or not
            if (_state.State == ENEMYSTATE.MOVING)
            {
                if (Vector3.Distance(transform.position, _lastStepPos) > Core.Instance.Settings.Enemies.RavStepDistance)
                {
                    _feet.PlayOneShot(_scatter);
                    _lastStepPos = transform.position;
                }
            }
        }
    }// EOF CLASS
}