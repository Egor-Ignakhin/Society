using UnityEngine;

namespace PlayerClasses.BasicNeedsReceivers
{
    public abstract class BasicNeedsReceiver : MonoBehaviour
    {
        protected BasicNeeds basicNeeds;
        protected virtual void Awake() => basicNeeds = FindObjectOfType<BasicNeeds>();
    }
}