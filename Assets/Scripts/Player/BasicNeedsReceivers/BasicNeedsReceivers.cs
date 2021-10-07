using Society.Player;

using UnityEngine;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    /// <summary>
    /// Наследники являются обработчиками событий классы <seealso cref="BasicNeeds"/>
    /// </summary>
    public abstract class BasicNeedsEventsReceiver : MonoBehaviour
    {
        protected BasicNeeds basicNeeds;
        protected virtual void Awake() => basicNeeds = FindObjectOfType<BasicNeeds>();
    }
}