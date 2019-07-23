using System;

namespace BattleshipTracker.Services.Exceptions
{
    [Serializable]
    public class AttackDeniedException : Exception
    {
        public AttackDeniedException(string message) : base(message)
        {
        }
    }
}