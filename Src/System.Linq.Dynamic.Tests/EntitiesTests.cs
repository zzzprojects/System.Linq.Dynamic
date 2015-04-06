using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic.Tests.Helpers.Entities;
using System.Data.Entity;
using System.Linq.Dynamic.Tests.Helpers;

namespace System.Linq.Dynamic.Tests
{
    /// <summary>
    /// Summary description for EntitiesTests
    /// </summary>
    [TestClass]
    public class EntitiesTests
    {
        BlogContext _context;


        public TestContext TestContext { get; set; }

        #region Entities Test Support
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //

        static readonly Random Rnd = new Random(1);

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            var connectionString = String.Format(@"Data Source=(localdb)\v11.0;Initial Catalog=DynamicLinqTestDb_{0};Integrated Security=True", TestContext.TestName);

            _context = new BlogContext(connectionString);
            _context.Database.Delete();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            _context.Database.Delete();

            _context.Dispose();
            _context = null;
        }

        void PopulateTestData(int blogCount = 25, int postCount = 10)
        {
            for (int i = 0; i < blogCount; i++)
            {
                var blog = new Blog() { Name = "Blog" + (i + 1) };

                _context.Blogs.Add(blog);

                for (int j = 0; j < postCount; j++)
                {
                    var post = new Post()
                    {
                        Blog = blog,
                        Title = String.Format("Blog {0} - Post {1}", i + 1, j + 1),
                        Content = "My Content",
                        PostDate = DateTime.Today.AddDays(-Rnd.Next(0,100)).AddSeconds(Rnd.Next(0, 30000)),
                        NumberOfReads = Rnd.Next(0, 5000)
                    };

                    _context.Posts.Add(post);
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region Select Tests

        [TestMethod]
        public void Entities_Select_SingleColumn()
        {
            //Arrange
            PopulateTestData(5, 0);

            var expected = _context.Blogs.Select(x => x.BlogId).ToArray();

            //Act
            var test = _context.Blogs.Select("BlogId").ToDynamicArray();

            //Assert
            CollectionAssert.AreEqual(expected, test);
        }

        [TestMethod]
        public void Entities_Select_MultipleColumn()
        {
            //Arrange
            PopulateTestData(5, 0);

            var expected = _context.Blogs.Select(x => new { x.BlogId, x.Name }).ToArray();

            //Act
            var test = _context.Blogs.Select("new (BlogId, Name)").ToDynamicArray();


            //Assert
            CollectionAssert.AreEqual(
                expected,
                test.Select(x => new { BlogId = (int)x.BlogId, Name = (string)x.Name }).ToArray() //convert to same anomymous type used by expected so they can be found equal
                );
        }

        [TestMethod]
        public void Entities_Select_BlogPosts()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Blogs.Where(x => x.BlogId == 1).SelectMany(x => x.Posts).Select(x => x.PostId).ToArray();

            //Act
            var test = _context.Blogs.Where(x => x.BlogId == 1).SelectMany("Posts").Select("PostId").ToDynamicArray();

            //Assert
            CollectionAssert.AreEqual(expected, test);
        }

        [TestMethod]
        public void Entities_Select_BlogAndPosts()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Blogs.Select(x => new { x.BlogId, x.Name, x.Posts }).ToArray();

            //Act
            var test = _context.Blogs.Select("new (BlogId, Name, Posts)").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];
                var testRow = test[i];

                Assert.AreEqual(expectedRow.BlogId, testRow.BlogId);
                Assert.AreEqual(expectedRow.Name, testRow.Name);

                CollectionAssert.AreEqual(expectedRow.Posts, testRow.Posts);
            }
        }

        #endregion

        #region GroupBy Tests

        [TestMethod]
        public void Entities_GroupBy_SingleKey()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Posts.GroupBy(x => x.BlogId).ToArray();

            //Act
            var test = _context.Posts.GroupBy("BlogId").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];

                //For some reason, the DynamicBinder doesn't allow us to access values of the Group object, so we have to cast first
                var testRow = (IGrouping<int,Post>)test[i];

                Assert.AreEqual(expectedRow.Key, testRow.Key);
                CollectionAssert.AreEqual(expectedRow.ToArray(), testRow.ToArray());
            }
        }

        [TestMethod]
        public void Entities_GroupBy_MultiKey()
        {
            //Arrange
            PopulateTestData(5, 15);

            var expected = _context.Posts.GroupBy(x => new { x.BlogId, x.PostDate }).ToArray();

            //Act
            var test = _context.Posts.GroupBy("new (BlogId, PostDate)").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];

                //For some reason, the DynamicBinder doesn't allow us to access values of the Group object, so we have to cast first
                var testRow = (IGrouping<DynamicClass, Post>)test[i];

                Assert.AreEqual(expectedRow.Key.BlogId, ((dynamic)testRow.Key).BlogId);
                Assert.AreEqual(expectedRow.Key.PostDate, ((dynamic)testRow.Key).PostDate);
                CollectionAssert.AreEqual(expectedRow.ToArray(), testRow.ToArray());
            }
        }

        [TestMethod]
        public void Entities_GroupBy_SingleKey_SingleResult()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Posts.GroupBy(x => x.PostDate, x => x.Title).ToArray();

            //Act
            var test = _context.Posts.GroupBy("PostDate", "Title").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];

                //For some reason, the DynamicBinder doesn't allow us to access values of the Group object, so we have to cast first
                var testRow = (IGrouping<DateTime, String>)test[i];

                Assert.AreEqual(expectedRow.Key, testRow.Key);
                CollectionAssert.AreEqual(expectedRow.ToArray(), testRow.ToArray());
            }
        }

        [TestMethod]
        public void Entities_GroupBy_SingleKey_MultiResult()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Posts.GroupBy(x => x.PostDate, x => new { x.Title, x.Content }).ToArray();

            //Act
            var test = _context.Posts.GroupBy("PostDate", "new (Title, Content)").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];

                //For some reason, the DynamicBinder doesn't allow us to access values of the Group object, so we have to cast first
                var testRow = (IGrouping<DateTime, DynamicClass>)test[i];

                Assert.AreEqual(expectedRow.Key, testRow.Key);
                CollectionAssert.AreEqual(
                    expectedRow.ToArray(),
                    testRow.Cast<dynamic>().Select(x => new { Title = (string)x.Title, Content = (string)x.Content }).ToArray());
            }
        }

        [TestMethod]
        public void Entities_GroupBy_SingleKey_Count()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Posts.GroupBy(x => x.PostDate).Select( x => new { x.Key, Count = x.Count()}).ToArray();

            //Act
            var test = _context.Posts.GroupBy("PostDate").Select( "new(Key, Count() AS Count)").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];
                var testRow = test[i];

                Assert.AreEqual(expectedRow.Key, testRow.Key);
                Assert.AreEqual(expectedRow.Count, testRow.Count);
            }
        }

        [TestMethod]
        public void Entities_GroupBy_SingleKey_Sum()
        {
            //Arrange
            PopulateTestData(5, 5);

            var expected = _context.Posts.GroupBy(x => x.PostDate).Select(x => new { x.Key, Reads = x.Sum(y => y.NumberOfReads) }).ToArray();

            //Act
            var test = _context.Posts.GroupBy("PostDate").Select("new(Key, Sum(NumberOfReads) AS Reads)").ToDynamicArray();

            //Assert
            Assert.AreEqual(expected.Length, test.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                var expectedRow = expected[i];
                var testRow = test[i];

                Assert.AreEqual(expectedRow.Key, testRow.Key);
                Assert.AreEqual(expectedRow.Reads, testRow.Reads);
            }
        }

        #endregion

        #region Executor Tests

        [TestMethod]
        public void FirstOrDefault_AsStringExpressions()
        {
            //Arrange
            PopulateTestData();

            //remove all posts from first record (to allow Defaults case to validate)
            _context.Posts.RemoveRange(_context.Blogs.OrderBy(x => x.BlogId).First<Blog>().Posts);
            _context.SaveChanges();

            
            //Act
            var firstExpected = _context.Blogs.OrderBy(x => x.Posts.OrderBy(y => y.PostDate).FirstOrDefault().PostDate).Select(x => x.BlogId);
            var firstTest = _context.Blogs.OrderBy("Posts.OrderBy(PostDate).FirstOrDefault().PostDate").Select("BlogId");

            //Assert
            CollectionAssert.AreEqual(firstExpected.ToArray(), firstTest.ToDynamicArray());
        }

        #endregion

        #region Entitites Helper Function Tests

        [TestMethod]
        public void Entities_Helper_Function_Tests()
        {
            System.Linq.Dynamic.GlobalConfig.CustomTypeProvider = new CustomTypeProvider();

            //Arrange
            PopulateTestData(5, 0);

            var expected = _context.Blogs.Select(x => DbFunctions.Reverse(x.Name)).ToArray();

            //Act
            var test = _context.Blogs.Select("DbFunctions.Reverse(Name)").ToDynamicArray();

            //Assert
            CollectionAssert.AreEqual(expected, test);
        }

        [TestMethod]
        public void Entities_Helper_Function_Exceptions()
        {
            System.Linq.Dynamic.GlobalConfig.CustomTypeProvider = null;

            //Arrange
            PopulateTestData(5, 0);

            //Act
            Helper.ExpectException<ParseException>(() => _context.Blogs.Select("DbFunctions.Reverse(Name)").ToDynamicArray());
        }

        class CustomTypeProvider : DefaultDynamicLinqCustomTypeProvider
        {

            public override HashSet<Type> GetCustomTypes()
            {
                var result = base.GetCustomTypes();

                result.Add(typeof(DbFunctions));

                return result;
            }
        }

        #endregion
    }
}
