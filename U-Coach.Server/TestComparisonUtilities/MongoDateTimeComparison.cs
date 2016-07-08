using System;
using DeepEqual;

namespace TestComparisonUtilities
{
    /// <summary>
    /// Сравнивает время с точностью до миллисекунд
    /// </summary>
    public class MongoDateTimeComparison : IComparison
    {
        public bool CanCompare(Type type1, Type type2)
        {
            return type1 == typeof(DateTime) && type2 == typeof(DateTime);
        }

        public ComparisonResult Compare(IComparisonContext context, object value1, object value2)
        {
            var dt1 = (DateTime)value1;
            var dt2 = (DateTime)value2;

            var msDt1 = dt1.Ticks / 10000;
            var msDt2 = dt2.Ticks / 10000;

            return
                msDt1 == msDt2 ?
                ComparisonResult.Pass : 
                ComparisonResult.Fail;
        }
    }
}
