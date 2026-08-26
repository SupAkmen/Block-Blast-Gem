using UnityEngine;

public class CallButtonMethod : StateMachineBehaviour
{
     [SerializeField] private string btnMethod;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
          base.OnStateEnter(animator, stateInfo, layerIndex);
          var component = animator.gameObject.GetComponent<CustomButon>();
          if (component != null)
          {
               component.Invoke(btnMethod,0.3f);
          }
     }
}
