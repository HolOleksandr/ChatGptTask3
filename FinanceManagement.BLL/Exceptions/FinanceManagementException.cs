using System.Runtime.Serialization;

namespace FinanceManagement.BLL.Exceptions
{
    [Serializable]
    public class FinanceManagementException : Exception
    {
        public FinanceManagementException() { }

        public FinanceManagementException(string message) : base(message) { }

        public FinanceManagementException(string message, Exception innerException) : base(message, innerException) { }

        protected FinanceManagementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
