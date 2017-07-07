using System.Collections.Generic;

namespace System.Linq.Dynamic
{
    /// <summary>
    /// Interface for providing custom types for Dynamic Linq.
    /// </summary>
    public interface IDynamicLinkCustomTypeProvider
    {
        /// <summary>
        /// Returns a list of custom types that Dynamic Linq will understand.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        HashSet<Type> GetCustomTypes();
    }
}
