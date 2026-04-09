using Unity;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Running,
        Paused,
        GameOver,
    }

    public State currentState = State.Idle;
}
