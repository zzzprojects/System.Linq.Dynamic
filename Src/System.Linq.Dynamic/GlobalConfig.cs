using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic
{
    /// <summary>
    /// Static configuration class for Dynamic Linq.
    /// </summary>
    public static class GlobalConfig
    {
        static IDynamicLinkCustomTypeProvider _customTypeProvider;

        /// <summary>
        /// Gets or sets the <see cref="IDynamicLinkCustomTypeProvider"/>.  Defaults to <see cref="DefaultDynamicLinqCustomTypeProvider" />.
        /// </summary>
        public static IDynamicLinkCustomTypeProvider CustomTypeProvider
        {
            get
            {
                if (_customTypeProvider == null) _customTypeProvider = new DefaultDynamicLinqCustomTypeProvider();

                return _customTypeProvider;
            }
            set
            {
                if (_customTypeProvider != value)
                {
                    _customTypeProvider = value;

                    ExpressionParser.ResetDynamicLinqTypes();
                }
            }
        }


    }
}
