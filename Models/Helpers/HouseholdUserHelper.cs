using budgeter.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgeter.Models.Helpers
{
    public class HouseholdUserHelper
    {
        public class HouseholdUsersHelper
        {
            private ApplicationDbContext db;

            public HouseholdUsersHelper(ApplicationDbContext context)
            {
                this.db = context;
            }

            public bool IsUserOnHousehold(int householdId, string userId)
            {
                var household = db.Households.Find(householdId);
                var userCheck = household.Users.Any(p => p.Id == userId);
                return (userCheck);
            }

            public ICollection<ApplicationUser> ListAssignedUsers(int householdId)
            {
                Household household = db.Households.Find(householdId);
                var userList = household.Users.ToList();
                return (userList);
            }

            public bool AddUserToHousehold(int householdId, string userId)
            {
                Household household = db.Households.Find(householdId);
                ApplicationUser user = db.Users.Find(userId);

                household.Users.Add(user);

                try
                {
                    var userAdded = db.SaveChanges();

                    if (userAdded != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            public bool RemoveUserFromHousehold(int householdId, string userId)
            {
                Household household = db.Households.Find(householdId);
                ApplicationUser user = db.Users.Find(userId);

                var result = household.Users.Remove(user);

                try
                {
                    var userRemoved = db.SaveChanges();

                    if (userRemoved != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}