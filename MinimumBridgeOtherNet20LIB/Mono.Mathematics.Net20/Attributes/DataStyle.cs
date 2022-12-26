
namespace Mono.Core
{
    /// <summary>
    /// Specifies the style used for textual serialization when an array/list or a dictionary/map must
    /// be serialized.
    /// </summary>
    public enum DataStyle
    {
        /// <summary>
        /// Let the emitter choose the style.
        /// </summary>
        Any,

        /// <summary>
        /// The normal style (One line per item, structured by space).
        /// </summary>
        Normal,

        /// <summary>
        /// The compact style (style embraced by [] or {})
        /// </summary>
        Compact
    }
}