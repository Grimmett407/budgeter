using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgeter.Models.Helpers
{
    public class Utility
    {
        // Generates a random alphanumeric code with 6 characters
        public static string GenerateSecretCode()
        {
            return GenerateSecretCode(6);
        }

        public static string GenerateSecretCode(int length)
        {
            string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            // Selects the random characters from the set and then the array of characters are passed into the string constructor to make an alpahnumeric string.
            string randomCode = new string(
                Enumerable.Repeat(characterSet, length)
                .Select(set => set[random.Next(set.Length)])
                .ToArray());
            return randomCode;
        }
    }
}