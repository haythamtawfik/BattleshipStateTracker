using System;

namespace BattleshipTracker.Services.Exceptions
{
    [Serializable]
    public class InCreatableShipException : Exception
    {
        public InCreatableShipException(string message) : base(message)
        {
        }

    }
}