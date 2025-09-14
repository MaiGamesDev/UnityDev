using UnityEngine;

namespace DemonBoss
{

    public interface IDemonState
    {
        void OnEnter();
        void OnState();
        void OnExit();
    }
}
