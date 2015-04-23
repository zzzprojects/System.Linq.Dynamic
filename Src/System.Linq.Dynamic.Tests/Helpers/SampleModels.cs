using FluentValidationNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.Tests.Helpers
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public int Income { get; set; }

        public int? NullableAge { get; set; }

        public UserProfile Profile { get; set; }

        public List<Role> Roles { get; set; }

        public static IList<User> GenerateSampleModels(int total, bool allowNullableProfiles = false)
        {
            Validate.Argument(total).IsInRange(x => total >= 0).Check();

            var list = new List<User>();

            for (int i = 0; i < total; i++)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = "User" + i.ToString(),
                    Income = ((i) % 15) * 100
                };

                if (i % 3 > 0) user.NullableAge = i % 50;

                if (!allowNullableProfiles || (i % 8) != 5)
                {
                    user.Profile = new UserProfile()
                    {
                        FirstName = "FirstName" + i,
                        LastName = "LastName" + i,
                        Age = (i % 50) + 18
                    };
                }

                user.Roles = new List<Role>(Role.StandardRoles);

                list.Add(user);
            }

            return list.ToArray();
        }
    }

    public class UserProfile
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }
    }

    public class Role
    {
        public static readonly Role[] StandardRoles = new Role[] {
            new Role() { Name="Admin"},
            new Role() { Name="User"},
            new Role() { Name="Guest"},
            new Role() { Name="G"},
            new Role() { Name="J"},
            new Role() { Name="A"},
        };

        public Role()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class SimpleValuesModel
    {
        public float FloatValue { get; set; }

        public decimal DecimalValue { get; set; }
    }
}
