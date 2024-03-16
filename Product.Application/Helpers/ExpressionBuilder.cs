using System.Linq.Expressions;
using Product.Application.Dtos.Requests;

namespace Product.Application.Helpers
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<Domain.Models.Product, bool>> BuildWhereExpression(ProductFilterDto filter)
        {
            Expression<Func<Domain.Models.Product, bool>> condition = product => true;

            if (!string.IsNullOrEmpty(filter.Name))
            {
                Expression<Func<Domain.Models.Product, bool>> nameCondition = product => product.Name.ToLower().Contains(filter.Name.ToLower());
                condition = CombineExpressions(condition, nameCondition);
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                Expression<Func<Domain.Models.Product, bool>> descriptionCondition = product => product.Description.ToLower().Contains(filter.Description.ToLower());
                condition = CombineExpressions(condition, descriptionCondition);
            }

            if (filter.Type.HasValue)
            {
                Expression<Func<Domain.Models.Product, bool>> descriptionCondition = product => product.Type == filter.Type.Value;
                condition = CombineExpressions(condition, descriptionCondition);
            }

            if (filter.Price != null)
            {
                Expression<Func<Domain.Models.Product, bool>> descriptionCondition = product => product.Price == filter.Price.Value;
                condition = CombineExpressions(condition, descriptionCondition);
            }

            return condition;
        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(first.Parameters[0], parameter);
            var left = leftVisitor.Visit(first.Body);

            var rightVisitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
            var right = rightVisitor.Visit(second.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }
    }

    class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        public override Expression Visit(Expression node)
        {
            if (node == oldValue)
                return newValue;
            return base.Visit(node);
        }
    }
}
