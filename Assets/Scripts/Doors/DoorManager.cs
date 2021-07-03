using System;
using UnityEngine;

public sealed class DoorManager : MonoBehaviour, IChangeable// класс реализует взаимодействие, а так же движение двери
{
    public enum RateTypes { normal, extrim }
    [SerializeField] private State currentState;
    private State nextState = State.nullable;
    public bool IsOpen { get; private set; }// открытость двери
    private bool canInteract = true;// возможность взаимодействия


    internal void SetDefaultRate(RateTypes rateType, float lerpRate)
    {
        isExtrimSituation = rateType == RateTypes.extrim;
        ExtrimLerpRate = lerpRate;
    }

    [SerializeField] private Vector3 openState;// открытое состояние

    [SerializeField] private Vector3 lockState;// закрытое состояние

    private float ExtrimLerpRate { get; set; } = 100;// скорость экстремального движения двери
    private DoorMesh lastDoorMesh;

    private bool isExtrimSituation;

    public void Interact(DoorMesh doorMesh)
    {
        if (!canInteract)
            return;

        lastDoorMesh = doorMesh;
        if (currentState == State.locked)
        {
            SetDescription();

            return;
        }

        canInteract = !canInteract;
        IsOpen = !IsOpen;
        lastDoorMesh.SetType("None");
    }
    public void SetStateAfterNextInteract(State state)
    {
        nextState = state;
    }
    private void FixedUpdate()
    {
        if (IsOpen && !canInteract)
        {
            if (canInteract = Rotate(openState))
                SetDescription();
        }
        else if (!canInteract)
        {
            if (canInteract = Rotate(lockState))
                SetDescription();
        }
    }
    private bool Rotate(Vector3 state)
    {
        var rotation = Quaternion.Euler(state);
        return isExtrimSituation ? ExtrimRotate(rotation) : UsuallyRotate(rotation);
    }
    private bool UsuallyRotate(Quaternion rotation)
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime);

        return Math.Round(transform.localRotation.y, 5) == Math.Round(rotation.y, 5);
    }

    private bool ExtrimRotate(Quaternion rotation)
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotation, ExtrimLerpRate * Time.deltaTime);

        return Quaternion.Angle(transform.localRotation, rotation) < 0.01f;
    }
    private void SetDescription()
    {
        string output;
        if (currentState == State.locked)
        {
            output = nameof(InteractiveObject.Types.LockedDoor);
        }
        else if (IsOpen)
        {
            output = nameof(InteractiveObject.Types.OpenedDoor);
        }
        else
        {
            output = nameof(InteractiveObject.Types.ClosedDoor);
        }
        lastDoorMesh.SetType(output);
        ChangeState();
    }
    public void SetType(DoorMesh d)
    {
        lastDoorMesh = d;
        SetDescription();
    }
    public void SetExtrimSituation(bool value)
    {
        isExtrimSituation = value;
    }

    public void ChangeState()
    {
        if (nextState != State.nullable)
        {
            currentState = nextState;
            nextState = State.nullable;
        }
    }
    public State GetState()
    {
        return currentState;
    }
}
