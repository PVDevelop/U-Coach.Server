using System;
using System.Linq;
using System.Linq.Expressions;
using DeepEqual;

namespace TestComparisonUtilities
{
    public class TestComparer
    {
        private ComparisonBuilder _builder = new ComparisonBuilder();

        public TestComparer WithMongoDateTimeComparer()
        {
            _builder = _builder.WithCustomComparison(new MongoDateTimeComparison());
            return this;
        }

        public TestComparer IgnoreProperty<T>(Expression<Func<T, object>> expression)
        {
            _builder = _builder.IgnoreProperty<T>(expression);
            return this;
        }

        public bool Compare(object left, object right, out string comparisonResult)
        {
            var context = new ComparisonContext();
            var comparison = _builder.Create().Compare(context, left, right);

            comparisonResult =
                context.Differences.Any() ?
                string.Format("Различающиеся свойства: {0}",
                    string.Join("; ", context.Differences.Select(diff =>
                        string.Format(
                            "{0}: {1} != {2}",
                            string.IsNullOrEmpty(diff.Breadcrumb) ? diff.ChildProperty : diff.Breadcrumb,
                            diff.Value1,
                            diff.Value2)))) :
                null;

            return comparison == ComparisonResult.Pass;
        }
    }
}
