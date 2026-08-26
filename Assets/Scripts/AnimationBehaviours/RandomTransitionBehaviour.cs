using System.Collections.Generic;
using UnityEngine;

namespace AnimationBehaviours
{
    public class RandomTransitionBehaviour  : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var parameters = animator.parameters;
            
            var triggerParam = new List<AnimatorControllerParameter>();
            foreach (var param in parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    triggerParam.Add(param);
                }
            }

            if (triggerParam.Count > 0)
            {
                var randomIndex  = Random.Range(0, triggerParam.Count);
                var randomTriggerName = triggerParam[randomIndex].name;
                
                animator.SetTrigger(randomTriggerName);
            }
        }
    }
}