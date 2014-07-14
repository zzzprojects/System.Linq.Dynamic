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

    #region Test Tuples

    public class Tuple<T1, T2, T3>
    {
        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public T1 Item1 { get; private set; }

        public T2 Item2 { get; private set; }

        public T3 Item3 { get; private set; }
    }

    #endregion
}
