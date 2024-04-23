using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Silversong.Game
{
    public class HeroAnimatorController : MonoBehaviour
    {

        public float Speed;


        [SerializeField] private Animator _animator;


        private Enums.AnimatorStates _state = Enums.AnimatorStates.normal;


        private Enums.Animations _currentAnimation;


        bool _needToUseMeleeAttack = false;
        bool _needToUseRangeAttack = false;


        private void FixedUpdate()
        {


            // legs

            // main




        }

        private void Start() // should we separate it?  TODO
        {
            EventsProvider.TenTimesPerSecond += CheckAnimatorState;
        }



        private void CheckAnimatorState()
        {
            _needToUseMeleeAttack = TargetProvider.IsEnemiesInMeleeZone(transform.position);



            if (_state == Enums.AnimatorStates.normal)
            {
                if (_needToUseMeleeAttack)
                {
                    SetAnimation(Enums.Animations.Fight);
                }
                else if (_needToUseRangeAttack)
                {
                    SetAnimation(Enums.Animations.FightRange);
                }
                else
                {
                    if (Speed < 0.25f)
                    {
                        SetAnimation(Enums.Animations.Idle);
                    }
                    else
                    {
                        SetAnimation(Enums.Animations.Run);
                    }
                }
            }







        }






        private void SetAnimation(Enums.Animations newAnimation)
        {
            if (newAnimation == _currentAnimation)
            {
                return;
            }

            TurnOffAnimation((int)_currentAnimation);

            int number = (int)newAnimation;

            TurnOnAnimation(number);

            _currentAnimation = newAnimation;
            // weights ???
        }


        private void TurnOnAnimation(int number)
        {
            if (number < 10)  // base states
            {
                _animator.SetBool(((Enums.Animations)number).ToString(), true);

            }
            else if (number < 50)  // triggers states
            {
                _animator.SetTrigger(((Enums.Animations)number).ToString());
            }
            else if (number >= 50)  // control states
            {
                _animator.SetBool(((Enums.Animations)number).ToString(), true);
                _animator.SetLayerWeight(3, 1);
            }
        }


        private void TurnOffAnimation(int animationId)
        {
            if (animationId < 10)  // base states
            {
                _animator.SetBool(((Enums.Animations)animationId).ToString(), false);
            }
            else if (animationId < 50)  // triggers states
            {
                _animator.ResetTrigger(((Enums.Animations)animationId).ToString());
            }
            else if (animationId >= 50)  // control states
            {
                _animator.SetBool(((Enums.Animations)animationId).ToString(), false);
                _animator.SetLayerWeight(3, 0);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            EventsProvider.TenTimesPerSecond -= CheckAnimatorState;
        }
    }
}