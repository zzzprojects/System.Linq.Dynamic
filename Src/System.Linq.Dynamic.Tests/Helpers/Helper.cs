using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.Tests.Helpers
{
    static class Helper
    {
        public static void ExpectException<TException>(Action action) where TException : Exception
        {
            Exception ex = null;

            try
            {
                action();
            }
            catch (TException exception)
            {
                if (exception.GetType() == typeof(TException)) return;

                ex = exception;
            }
            catch (Exception exception)
            {
                ex = exception;
            }

            Assert.Fail("Expected Exception did not occur.");
        }

#if NET35
        public static T GetDynamicProperty<T>(this object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            var type = obj.GetType();
            var propInfo = type.GetProperty(propertyName);

            return (T)propInfo.GetValue(obj, null);
        }
#endif

    }
}
