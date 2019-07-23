using System;
using System.Runtime.Serialization;

namespace BattleshipTracker.Services.Exceptions
{
    [Serializable]
    public class InCreatableGameException : Exception
    {
        public InCreatableGameException(string message) : base(message)
        {
        }

    }
}