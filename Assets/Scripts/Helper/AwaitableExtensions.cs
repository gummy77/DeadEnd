using System;
using UnityEngine;

namespace Helper
{
    public static class AwaitableExtensions
    {
        /// <summary>
        /// Replaces general await discards ('_ = ()') but ensures that errors are still reported correctly. Usage is as follows:
        /// <para>
        /// `MyAwaitableFunction().DiscardAwaitable(nameof(MyAwaitableFunction))`
        /// </para>
        /// <para>
        /// Generally async void is not recommended because exceptions are unhandled, but because we're capturing it in this case, its okay.
        /// </para>
        /// </summary>
        /// <param name="awaitable">The function that uses awaitable</param>
        /// <param name="callerName">The name of the function being called using nameof()</param>
        /// <param name="logError">Whether to log the error. True by default.</param>
        public static async void DiscardAwaitable(
            this Awaitable awaitable,
            string callerName = "",
            bool logError = true)
        {
            try
            {
                await awaitable;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                if (logError)
                {
                    Debug.LogError($"[DiscardedAwaitable] Unhandled exception in {callerName}:\n{e}");
                }
            }
        }
    }
}