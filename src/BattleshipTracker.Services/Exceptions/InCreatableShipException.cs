using System;

namespace BattleshipTracker.Services.Exceptions
{
    [Serializable]
    class InCreatableShipException : Exception
    {
        public InCreatableShipException(string message) : base(message)
        {
        }

    }
}