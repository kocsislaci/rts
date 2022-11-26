using Unit.Character;
using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.BehaviourTree.Checks
{
    public class CheckEnemyInAttackRange: Node
    {
        CharacterController _controller;
        float _attackRange;

        public CheckEnemyInAttackRange(CharacterController controller) : base()
        {
            _controller = controller;
            _attackRange = ((CharacterData)_controller.representingObject.data).attackRange;
        }

        public override NodeState Evaluate()
        {
            object currentTarget = Parent.GetData("currentTarget");
            if (currentTarget == null)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            Transform target = (Transform)currentTarget;

            // (in case the target object is gone - for example it died
            // and we haven't cleared it from the data yet)
            if (!target)
            {
                Parent.ClearData("currentTarget");
                _state = NodeState.FAILURE;
                return _state;
            }

            Vector3 s = target.Find("Mesh").localScale;
            float targetSize = Mathf.Max(s.x, s.z);

            float d = Vector3.Distance(_controller.transform.position, target.position);
            bool isInRange = (d - targetSize) <= _attackRange;
            _state = isInRange ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
    }
}
