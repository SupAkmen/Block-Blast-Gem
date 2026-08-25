using UnityEngine;

public class CallButtonMethod : StateMachineBehaviour
{
     [SerializeField] private string exitMethod;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
          base.OnStateEnter(animator, stateInfo, layerIndex);

          var component = animator.gameObject.GetComponent<CustomButon>();

     }
     
     
}
