using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.BehaviourTree.Checks
{
    public class CheckEnemyInFOVRange: Node
    {
        CharacterController _controller;
        float _fovRadius;
        Team _unitOwner;

        Vector3 _pos;
        
        public CheckEnemyInFOVRange(CharacterController controller) : base()
        {
            _controller = controller;
            _fovRadius = _controller.representingObject.data.fieldOfView;
            _unitOwner = _controller.representingObject.Owner;
        }

        public override NodeState Evaluate()
        {
            _pos = _controller.transform.position;
            IEnumerable<Collider> enemiesInRange =
                Physics.OverlapSphere(_pos, _fovRadius)
                    .Where(delegate (Collider c)
                    {
                        UnitController uc = c.GetComponent<UnitController>();
                        if (uc == null) return false;
                        return uc.representingObject.Owner != _unitOwner;
                    });
            if (enemiesInRange.Any())
            {
                Parent.SetData(
                    "currentTarget",
                    enemiesInRange
                        .OrderBy(x => (x.transform.position - _pos).sqrMagnitude)
                        .First()
                        .transform
                );
                _state = NodeState.SUCCESS;
                return _state;
            }
            _state = NodeState.FAILURE;
            return _state;
        }
        
    }
}