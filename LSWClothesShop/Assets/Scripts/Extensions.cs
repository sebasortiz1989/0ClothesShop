using UnityEngine;

namespace Managers
{
    public static class Extensions
    {
        public static void SetTriggers(this Animator[] animatorControllers, int triggerHash)
        {
            foreach (var animatorController in animatorControllers)
            {
                animatorController.SetTrigger(triggerHash);
            }
        }
 
        public static void ResetTriggers(this Animator[] animatorControllers, int triggerHash)
        {
            foreach (var animatorController in animatorControllers)
            {
                animatorController.ResetTrigger(triggerHash);
            }
        }

        public static void SetBools(this Animator[] animatorControllers, int boolHash, bool value)
        {
            foreach (var animatorController in animatorControllers)
            {
                animatorController.SetBool(boolHash, value);
            }
        }
        
        public static void SetFloats(this Animator[] animatorControllers, int boolHash, float value)
        {
            foreach (var animatorController in animatorControllers)
            {
                animatorController.SetFloat(boolHash, value);
            }
        }
    }
}
