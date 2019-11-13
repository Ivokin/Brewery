using System;
using System.Collections.Generic;

namespace Brewery.Core.Helpers
{
    public class Randomizer
    {
        public List<int> ReturnListOfUniqueRandomNumbers(int maxNumber, int countOfNumbers)
        {
            if (maxNumber < countOfNumbers)
            {
                countOfNumbers = maxNumber;
            }

            List<int> result = new List<int>();
            Random random = new Random();
            while (countOfNumbers > 0)
            {
                int randomNumber = random.Next(maxNumber);
                if (!result.Contains(randomNumber))
                {
                    result.Add(randomNumber);
                    countOfNumbers--;
                }
            }

            return result;
        }
    }
}
