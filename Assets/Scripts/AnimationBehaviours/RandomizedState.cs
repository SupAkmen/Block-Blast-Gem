using UnityEngine;

namespace AnimationBehaviours
{
    /// <summary>
    /// Dung de random thoi gian cho truoc khi kich hoat mot animation trigger
    /// </summary>
    public class RandomizedState : StateMachineBehaviour
    {
        [Header("Randomized State")] 
        [SerializeField] private float minTime = 2f;
        [SerializeField] private float maxTime = 4f;
        [SerializeField] private string randomFinishedStr = "RandomFinished";

        private float currentTimeBeforeBlink;
        private float timeElapsed;

        private bool triggered;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Randomize();
            triggered = false;
        }

        private void Randomize()
        {
            currentTimeBeforeBlink = Random.Range(minTime, maxTime);
            timeElapsed = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(triggered) return;
            
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= currentTimeBeforeBlink)
            {
                triggered = true;
                animator.SetTrigger(Animator.StringToHash(randomFinishedStr));
            }
        }
        
        
    }
}