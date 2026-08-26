using UnityEngine;

namespace AnimationBehaviours
{
    public class CallPopUpMethod : StateMachineBehaviour
    {
        [SerializeField] private string popUpMethodName;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var component  = animator.gameObject.GetComponent<PopUp>();
            if (component != null)
            {
                component.Invoke(popUpMethodName,0f);
            }
        }
    }
}