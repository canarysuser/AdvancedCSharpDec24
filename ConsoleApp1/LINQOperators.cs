using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleApp1
{
    internal class LINQOperators
    {
        static List<string> cities = new List<string>()
        {
            "Bengaluru", "Chennai", "Mumbai", "Panaji", "Hyderabad", "Bhubaneswar", "Kolkata", "New Delhi",
            "Gandhinagar", "Srinagar", "Shimla", "Dehradoon", "Lucknow", "Raipur","Ranchi", "Patna", "Jaipur",
            "Aizwal", "Kohima", "Itanagar", "Imphal", "Gangtok", "Port Blair", "Chandigard", "Thiruvananthapuram",
            "Leh","Shillong", "Dispur"
        };

        static string line = "".PadLeft(45, '=');
        static int counter = 1;
        static void PrintList(IEnumerable<string> list, string header)
        {
            WriteLine(line);
            WriteLine($"                 {header} ");
            WriteLine(line);
            foreach (var item in list)
                Write($"{item}, ");
            WriteLine($"\n{line}");

        }
        class ProjectionType
        {
            public string Name { get; set; }
            public char InitialLetter { get; set; }
            public int Length { get; set; }
        }
        internal static void Test()
        {
            //BasicQuery();
            //ProjectionOperators();
            //RestrictionOperator();
            //SortingOperators(); 
            //AggregationOperators();
            // GroupingOperator();
            //PartitionOperators();
            //ElementOperators();
            //DbEntityLinq();
            CustomExpressionTree();

        }
        static void CustomExpressionTree()
        {
            IQueryable<string> queryableData = cities.AsQueryable<string>();
            ParameterExpression pe = Expression.Parameter(typeof(string), "c");
            Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            Expression right = Expression.Constant("shimla");
            Expression e1 = Expression.Equal(left, right);
            
            left = Expression.Property(pe, typeof(string).GetProperty("Length"));
            right = Expression.Constant(10, typeof(int));
            Expression e2 = Expression.GreaterThan(left, right);

            Expression predicateBody = Expression.OrElse(e1, e2);
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                 queryableData.Expression,
                Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe })
                );
            IQueryable<string> results = queryableData.Provider.CreateQuery<string>(whereCallExpression);
            foreach (var item in results)
                WriteLine(item);

        }
        static void DbEntityLinq()
        {
            ProductDbContext db = new ProductDbContext();
            db.Database.Log = WriteLine;
            //var q1 = from c in db.Products
            //         where c.CategoryId==1 || c.CategoryId==2
            //         select c;
            var q1 = db.Products
                .OrderBy(c => c.UnitPrice)
                .Where(c => c.UnitsInStock > 5)
                .Select(c => c)
                .Skip(5)
                .Take(5);
            var value = db.Products.Sum(c => c.UnitsInStock * c.UnitPrice);
            WriteLine("Stock Value: " + value);

            WriteLine(line);
            foreach(var item in q1)
            {
                WriteLine($"{item.CategoryId}={item.ProductName},{item.UnitPrice},{item.UnitsInStock},{item.ProductId}");
            }
            WriteLine("\n"+line);
        }
        static void ElementOperators()
        {
            //First, Last, ElementAt, FirstOrDefault, LastOrDefault, ElementAtOrDefault
            var first = cities.First(c=>c.Length==10);
            var last = cities.Last(c=>c.Length==10);
            Console.WriteLine("First " + first + ", Last: " + last);
            first = cities.FirstOrDefault(c => c.Length == 12);
            last = cities.LastOrDefault(c => c.Length == 12);
            Console.WriteLine("First " + (string.IsNullOrEmpty(first)?"null":first) + ", Last: " + last);

        }
        static void PartitionOperators()
        {
            //Take, TakeWhile, Skip, SkipWhile 
            //derives a subsection of the list 
            //Take(5) -> yields 5 items and skips the remaining items 
            //Skip(15) -> skip the first 15 items and then takes the remaining items.
            var take5 = cities.Take(5);
            PrintList(take5, $"{counter++}. Take 5");
            var skip15 = cities.Skip(15);
            PrintList(skip15, $"{counter++}. Skip 15");
            var takeSkip = cities.Skip(5).Take(15).Skip(4).Take(3);
            PrintList(takeSkip, $"{counter++}. Skip/Take combo");
            var takeWhile = cities.TakeWhile(c => c.Length < 10); //take while the condition is true, skip when false
            var skipWhile = cities.SkipWhile(c => c.Length < 10); //skip while the condition is true, take when false
            PrintList(takeWhile, $"{counter++}. Take While");
            PrintList(skipWhile, $"{counter++}. Skip While");
        }
        static void GroupingOperator()
        {
            //GroupBy -> group item by criteria into type
            var q1 = from c in cities
                     orderby c
                     group c by c[0] into g
                     select g;
            //Group over the key and then extract values for each key 
            foreach (var group in q1)
            {
                PrintList(group.ToList(), $"{counter++}. Key {group.Key}");
            }
            var q2 = cities.GroupBy(g => g.Length).Select(c => c);
            foreach (var group in q2)
            {
                PrintList(group.ToList(), $"{counter++}. Key Length {group.Key}");
            }

        }
        static void AggregationOperators()
        {
            //SUM, MIN, MAX, AVERAGE, COUNT
            //var numbers = Enumerable.Range(1, 100).ToList();
            var count = cities.Count();
            var sum = cities.Sum(c => c.Length);
            var max = cities.Max(c => c.Length);
            var min = cities.Min(c => c.Length);
            var avg = cities.Average(c => c.Length);
            Console.WriteLine($"Numbers- Count=>{count},Sum=>{sum},Max=>{max},Min=>{min},Avg=>{avg}");
        }
        static void SortingOperators()
        {
            //OrderBy, OrderByDescending, ThenBy, ThenByDescending 
            //orderby <firstcolumn> [ascending|descending], ..... 
            var q1 = from c in cities
                     orderby c[0] descending, c[1] ascending
                     select c;
            PrintList(q1, $"{counter++}. Ordered by first and second letters");

            var q2 = cities
                .OrderBy(c => c[0])
                .ThenByDescending(c => c[1])
                .Select(c => c);
            PrintList(q2, $"{counter++}. Ordered by Method first and second letters");
        }

        static void RestrictionOperator()
        {
            //WHERE 
            var q1 = from c in cities
                     where c.Length > 9
                     select c;
            PrintList(q1, $"{counter++}. Where Length > 9");
            var q2 = cities
                .Where(c => c.Contains("gar") && c.Length < 15)
                .Select(c => c);
            PrintList(q2, $"{counter++}. Where Length < 15 and contains 'gar' ");
        }
        static void ProjectionOperators()
        {
            //SELECT, SelectMany 
            Action<IEnumerable<ProjectionType>, string> printList = (list, header) =>
            {
                WriteLine(line);
                WriteLine($"                 {header} ");
                WriteLine(line);
                WriteLine($"{"Length",-7}{"StartsWith",-12}{"Name"}");
                foreach (var item in list)
                    WriteLine($"{item.Length,-7}{item.InitialLetter,-12}{item.Name}");
                WriteLine($"{line}");
            };

            var q1 = from c in cities
                     select new ProjectionType
                     {
                         Name = c,
                         InitialLetter = c[0],
                         Length = c.Length
                     };
            printList(q1, $"{counter++}. Query based Projection ");
            var q2 = cities.Select(c => new ProjectionType
            {
                Name = c,
                InitialLetter = c[0],
                Length = c.Length
            });
            printList(q2, $"{counter++}. Method based Projection ");
        }
        static void BasicQuery()
        {
            var query = from city in cities
                        select city;
            PrintList(query, $"{counter++}. Query Syntax Style");
            //Method Syntax
            var query2 = cities.Select(c => c);
            PrintList(query2, $"{counter++}. Method Syntax Style");
        }



    }
}
