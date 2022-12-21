
using System.Diagnostics;


namespace System.Numerics
{
    /// <summary>
    /// This class is used to enforce that preconditions are met for method calls
    /// using clear and consice semantics.
    /// </summary>
    internal static class Contract
    {


       // [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T ValueAtReturn<T>(out T value) {
            // Marker method, no code required.
            return value = default(T);
        }

        [Conditional("DEBUG")]
        internal static void EndContractBlock() {
        }

        [Conditional("DEBUG")]
        internal static void Ensures(bool condition) {
            // AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.Ensures");
        }


        /// <summary>
        /// Equivalent to Debug.Assert.  
        ///
        /// DevDiv 867813 covers removing this completely at a future date
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void Requires(bool condition, string message = null) {
            Assert(condition, message);
        }

        /// <summary>
        /// Equivalent to Debug.Assert.  
        ///
        /// DevDiv 867813 covers removing this completely at a future date
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void Assert(bool condition, string message = null) {
            if (condition) {
                return;
            }

            if (string.IsNullOrEmpty(message)) {
                Debug.Assert(condition);
            }
            else {
                Debug.Assert(condition, message);
            }
        }

        /// <summary>
        /// Equivalent to Debug.Assert.  
        ///
        /// DevDiv 867813 covers removing this completely at a future date
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assume(bool condition, string message = null) {
            if (string.IsNullOrEmpty(message)) {
                Debug.Assert(condition);
            }
            else {
                Debug.Assert(condition, message);
            }
        }

        /// <summary>
        /// Throws a non-accessible exception if the provided value is null.  This method executes in
        /// all builds
        /// </summary>
        public static void ThrowIfNull<T>(T value, string message = null) where T : class {
            if (value == null) {
                message = message ?? "Unexpected Null";
                Fail(message);
            }
        }

        /// <summary>
        /// Throws a non-accessible exception if the provided value is false.  This method executes
        /// in all builds
        /// </summary>
        public static void ThrowIfFalse(bool condition, string message = null) {
            if (!condition) {
                message = message ?? "Unexpected false";
                Fail(message);
            }
        }

        /// <summary>
        /// Throws a non-accessible exception if the provided value is true. This method executes in
        /// all builds.
        /// </summary>
        public static void ThrowIfTrue(bool condition, string message = null) {
            if (condition) {
                message = message ?? "Unexpected true";
                Fail(message);
            }
        }

        [DebuggerHidden]
        public static void Fail(string message = "Unexpected") {
            throw new InvalidOperationException(message);
        }

        [DebuggerHidden]
        public static T FailWithReturn<T>(string message = "Unexpected") {
            throw new InvalidOperationException(message);
        }

        public static void InvalidEnumValue<T>(T value) {
            Fail(string.Format("Invalid Enumeration value {0}", value));
        }
    }

}
