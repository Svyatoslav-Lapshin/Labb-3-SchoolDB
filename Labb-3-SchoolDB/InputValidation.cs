using System;
using System.Collections.Generic;
using System.Text;

namespace Labb_3_SchoolDB
{
    internal class InputValidation
    {

        public static int ValidateIntInRange(string question, int min, int max)
        {
            Console.Write(question);
            bool check = int.TryParse(Console.ReadLine(), out int result);
            while (!check || result < min || result > max)
            {
                Console.Write($"Invalid input. Please enter a valid integer between {min} and {max}:");
                check = int.TryParse(Console.ReadLine(), out result);
            }
            return result;

        }
        public static int ValidateIntInput(string question)
        {
            Console.Write(question);
            bool check = int.TryParse(Console.ReadLine(), out int result);

            while (!check || result < 0)
            {
                Console.Write("Invalid input. Please enter a valid positive integer:");
                check = int.TryParse(Console.ReadLine(), out result);
            }
            return result;
        }

        //String validation
        public static string ValidateStringInput(string question)
        {
            Console.Write(question);
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Input cannot be empty. Please enter a valid value:");
                input = Console.ReadLine();
            }
            return input;
        }



    }
}