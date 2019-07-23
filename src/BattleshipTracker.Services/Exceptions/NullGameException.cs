using System;

namespace BattleshipTracker.Services
{
    [Serializable]
    public class NullGameException : Exception
    {
        public NullGameException(string message) : base(message)
        {
        }
    }
}