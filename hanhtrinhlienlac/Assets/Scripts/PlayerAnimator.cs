using UnityEngine;
using Random = UnityEngine.Random;
using Spine.Unity;
using System;
using System.Data;
using System.Linq.Expressions;
using Spine;

namespace MiniGames
{
    /// <summary>
    /// This is a pretty filthy script. I was just arbitrarily adding to it as I went.
    /// You won't find any programming prowess here.
    /// This is a supplementary script to help with effects and animation. Basically a juice factory.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {
        // [SerializeField] private Animator _anim;
        [SerializeField] private SkeletonAnimation _spine;
        [SerializeField] private AudioSource _source;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private ParticleSystem _jumpParticles, _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles, _landParticles;
        [SerializeField] private AudioClip[] _footsteps;
        [SerializeField] private float _maxTilt = .1f;
        [SerializeField] private float _tiltSpeed = 1;
        [SerializeField, Range(1f, 3f)] private float _maxIdleSpeed = 2;
        [SerializeField] private float _maxParticleFallSpeed = -40;

        [SpineAnimation]
        public string runAnimationName;
        [SpineAnimation]
        public string idleAnimationName;

        [SpineAnimation]
        public string jumpnimationName;

        [SpineAnimation]
        public string ladingAnimation;

        [SpineAnimation]
        public string sitOnTheOxAnimation;

        [SpineAnimation]
        public string deathAnimation;
        private HeroController _player;
        private bool _playerGrounded;
        private ParticleSystem.MinMaxGradient _currentGradient;
        private Vector2 _movement;

        public enum AnimationState
        {
            IDLE,
            JUMP,
            LANDING,
            RUN,
            RIDE_OX,
            DIE
        }

        void Awake()
        {
            _player = GetComponentInParent<HeroController>();
            SetAnimationState(AnimationState.IDLE);
        }

        void Update()
        {
            if (_player == null) return;
            if (_player.rideTheOx)
            {
                SetAnimationState(AnimationState.RIDE_OX);
                return;
            }
            if (_player.isDie)
            {
                SetAnimationState(AnimationState.DIE, false);
                return;
            }
            // Flip the sprite
            if (_player.Input.X != 0) transform.localScale = new Vector3(_player.Input.X > 0 ? 1 : -1, 1, 1);

            // Lean while running
            var targetRotVector = new Vector3(0, 0, Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _player.Input.X)));
            _spine.transform.rotation = Quaternion.RotateTowards(_spine.transform.rotation, Quaternion.Euler(targetRotVector), _tiltSpeed * Time.deltaTime);
            if (_player.Grounded && !_player.JumpingThisFrame && !_player.LandingThisFrame)
            {
                if (_player.Input.X != 0)
                {
                    SetAnimationState(AnimationState.RUN);
                }
                else
                {
                    SetAnimationState(AnimationState.IDLE);
                }
            }

            // Speed up idle while running
            // _anim.SetFloat(IdleSpeedKey, Mathf.Lerp(1, _maxIdleSpeed, Mathf.Abs(_player.Input.X)));

            // Splat
            if (_player.LandingThisFrame)
            {
                SetAnimationState(AnimationState.LANDING, false);
                _source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
            }

            // Jump effects
            if (_player.JumpingThisFrame)
            {
                // _spine.AnimationState.SetAnimation(0,jumpnimationName,true);
                SetAnimationState(AnimationState.JUMP, false);
                //_anim.ResetTrigger(GroundedKey);

                // Only play particles when grounded (avoid coyote)
                if (_player.Grounded)
                {
                    SetColor(_jumpParticles);
                    SetColor(_launchParticles);
                    _jumpParticles.Play();
                }
            }

            // Play landing effects and begin ground movement effects
            if (!_playerGrounded && _player.Grounded)
            {
                _playerGrounded = true;
                _moveParticles.Play();
                _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, _maxParticleFallSpeed, _movement.y);
                SetColor(_landParticles);
                _landParticles.Play();
            }
            else if (_playerGrounded && !_player.Grounded)
            {
                _playerGrounded = false;
                _moveParticles.Stop();
            }

            // Detect ground color
            var groundHit = Physics2D.Raycast(transform.position, Vector3.down, 2, _groundMask);
            if (groundHit && groundHit.transform.TryGetComponent(out SpriteRenderer r))
            {
                _currentGradient = new ParticleSystem.MinMaxGradient(r.color * 0.9f, r.color * 1.2f);
                SetColor(_moveParticles);
            }

            _movement = _player.RawMovement; // Previous frame movement is more valuable
        }

        private void OnDisable()
        {
            _moveParticles.Stop();
        }

        private void OnEnable()
        {
            _moveParticles.Play();
        }

        void SetColor(ParticleSystem ps)
        {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        #region Animation Keys

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
        private static readonly int JumpKey = Animator.StringToHash("Jump");

        #endregion
        private TrackEntry SetAniamtion(String s, bool loop)
        {
            return _spine.AnimationState.SetAnimation(0, s, loop);
        }
        private void AddAnimation(String s, bool loop)
        {
            _spine.AnimationState.AddAnimation(0, s, loop, 0);
        }
        private AnimationState _curAState;

        private void SetAnimationState(AnimationState s, bool loop = true)
        {
            if (_curAState == s) return;
            switch (s)
            {
                case AnimationState.IDLE:
                    {
                        AnimateIdel(loop);
                        break;
                    }
                case AnimationState.RUN:
                    {
                        AnimateRun(loop);
                        break;
                    }
                case AnimationState.RIDE_OX:
                    {
                        AnimateRideOx(loop);
                        break;
                    }
                case AnimationState.DIE:
                    {
                        AnimateDie(loop);
                        break;
                    }
                case AnimationState.JUMP:
                    {
                        AnimateJump(loop);
                        break;
                    }
                case AnimationState.LANDING:
                    {
                        AnimateLanding(loop);
                        break;
                    }
            }
            _curAState = s;
        }
        private void AnimateIdel(bool loop)
        {
            SetAniamtion(idleAnimationName, loop);
        }
        private void AnimateRun(bool loop)
        {
            SetAniamtion(runAnimationName, loop);
        }
        private void AnimateRideOx(bool loop)
        {
            SetAniamtion(sitOnTheOxAnimation, loop);
        }
        private void AnimateDie(bool loop)
        {
            TrackEntry track = SetAniamtion(deathAnimation, loop);
            track.Complete += (track) =>
            {
                Debug.Log("Finished Die Animation " + track.Animation.Name);
                _player.NotifyRevive();
                SetAnimationState(AnimationState.IDLE);

            };
            // _spine.AnimationState.End+=(track)=> {
            //     Debug.Log("Finished Die AnimationState " + track.Animation.Name);
            // };

        }
        private void AnimateJump(bool loop)
        {
            SetAniamtion(jumpnimationName, false);
        }
        private void AnimateLanding(bool loop)
        {
            SetAniamtion(ladingAnimation, false);
        }

    }


}