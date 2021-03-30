using UnityEngine;

namespace PlayerClasses
{
    public sealed class PlayerStatements : MonoBehaviour
    {
        private BasicNeeds mBasicNeeds;// класс базовых нужд
        private bool lockedCursorState;

        private void Awake()
        {
            mBasicNeeds = GetComponent<BasicNeeds>();
            SetCursorState();
        }
        
        public enum Message {meal };
        public  void SendMessage(Message m, EatingObject sender)
        {
            switch (m)
            {
                case Message.meal:                    
                    mBasicNeeds.AddMeal(sender.GetThirst(), sender.GetFood());
                    break;
            }
        }
        private void Update()
        {            
            if (Input.GetKeyDown(KeyCode.Escape))
                SetCursorState();
        }
        private void SetCursorState()
        {
            lockedCursorState = !lockedCursorState;
            Cursor.lockState = lockedCursorState ? CursorLockMode.Locked : CursorLockMode.None;            
            Cursor.visible = lockedCursorState;
        }
    }
}