using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    public ITarget Target;

    private float _speed = 3f;


    [SerializeField] private NavMeshAgent _navMeshAgent;

    private bool _canMove = true;

    private Vector3 _previousPosition;
    private Vector3 _expectedPosition;

    private float _timeElapsed, _radius;
    private Vector3 _startPosition = Vector3.zero;
    private bool _ranged;


    private void Start()
    {
        Subscribe();
        _expectedPosition = transform.position;
        _startPosition = transform.position;
    }


    public void UpdatePosition(Vector3 position)
    {
        _expectedPosition = position;
        _startPosition = transform.position;
    }

    private void TimeTick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MoveCurrentEnemy();
        }
        else
        {
            MoveEnemyOther();
        }
    }



    private void MoveCurrentEnemy()
    {
        if(Target == null)
        {
            _navMeshAgent.ResetPath();
            return;
        }

        if(Vector3.Distance(Target.GetPosition(), transform.position) < 1f)
        {
            _navMeshAgent.ResetPath();
        }
        else
        {
            _navMeshAgent.SetDestination(Target.GetPosition());
        }

    }


    private void MoveEnemyOther()
    {
        Vector3 curMove = transform.position - _previousPosition;

        if (Target != null)
        {
            if (Vector3.Distance(_expectedPosition, transform.position) >= 3f)
            {
                float dist = Vector3.Distance(_expectedPosition, Target.GetPosition());
                Vector3 averagePostion = _expectedPosition - Target.GetPosition().normalized * dist / 2;

                _navMeshAgent.speed = _speed * 1.5f;
                _navMeshAgent.SetDestination(averagePostion);
                return;
            }
            else
            {
                _navMeshAgent.SetDestination(Target.GetPosition());
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_expectedPosition);
        }

        _previousPosition = transform.position;


    }




       // _navMeshAgent.Warp(_expectedPosition);

        //if ((_expectedPosition - _startPosition).sqrMagnitude > 0.025f)
        //{


        //    if (Target != null)
        //    {
        //        _navMeshAgent.SetDestination(Target.GetPosition());
        //    }
        //    else  // TEST IT MORE!      TODO !!!!
        //    {
        //        float distance = Vector3.Distance(_expectedPosition, transform.position);

        //        float speed = 5f * (distance / 2f); // check it TODO !!!1
        //        if (speed < 3)
        //        {
        //            speed = 3;
        //        }

        //        _navMeshAgent.speed = speed;

        //        if (_expectedPosition != _startPosition)
        //        {
        //            _navMeshAgent.SetDestination(_expectedPosition);
        //        }

        //        _previousPosition = transform.position;
        //    }

        //    return;
        //}

        //_navMeshAgent.SetDestination(_expectedPosition);
        //_previousPosition = transform.position;
    















    // 

    private void Subscribe()
    {
        EventsProvider.ThreeTimesPerSecond += TimeTick;
    }

    private void Dispose()
    {
        EventsProvider.ThreeTimesPerSecond -= TimeTick;
    }

    private void OnDestroy()
    {
        Dispose();
    }
}
