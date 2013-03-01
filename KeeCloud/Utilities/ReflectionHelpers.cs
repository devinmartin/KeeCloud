using System;
using System.Linq.Expressions;

namespace KeeCloud.Utilities
{
    class ReflectionHelpers
    {
        /// <summary>
        /// Takes a lambda expression that accesses a member and returns the name of the member
        /// </summary>
        /// <typeparam name="TObject">The type that contains the member</typeparam>
        /// <typeparam name="TMember">The type of the member</typeparam>
        /// <param name="expression">Lambda expression that does nothing more than access (return) the value of the member</param>
        /// <returns>String name of the member that was acessed by the expression</returns>
        public static string MemberNameFromExpression<TObject, TMember>(Expression<Func<TObject, TMember>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                MemberExpression body = (MemberExpression)expression.Body;
                return body.Member.Name;
            }
            else
                throw new Exception("The lambda expression is invalid. Use format () => object.member and nothing more. Ensure that you are accessing a property or a field only");
        }
    }
}
