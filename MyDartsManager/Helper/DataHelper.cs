using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDartsManager.Entity;

namespace MyDartsManager.Helper
{
    public static class DataHelper
    {
        /// <summary>
        /// Used to populate the database with all the possible throw combinations on the database creation.
        /// </summary>
        /// <param name="dbContext"></param>
        public static void SeedThrowCombinations(DartsDbContext dbContext)
        {
            if (!dbContext.ThrowCombinations.Any())
            {
                List<ThrowCombination> throwCombinations = new List<ThrowCombination>();

                //invalid throw
                throwCombinations.Add(new ThrowCombination(-1, 1));
                //zero throw
                throwCombinations.Add(new ThrowCombination(0, 1));

                for (int i = 1; i < 21;i++)
                {
                    throwCombinations.Add(new ThrowCombination(i, 1));
                    throwCombinations.Add(new ThrowCombination(i, 2));
                    throwCombinations.Add(new ThrowCombination(i, 3));
                }

                //center point throws
                throwCombinations.Add(new ThrowCombination(25, 1));
                throwCombinations.Add(new ThrowCombination(25, 2));



                dbContext.ThrowCombinations.AddRange(throwCombinations);
                dbContext.SaveChanges();
            }
        }
    }
}
