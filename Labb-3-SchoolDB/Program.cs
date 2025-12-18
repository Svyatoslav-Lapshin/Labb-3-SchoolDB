using DatabaseSchool.Menu;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


using System.Globalization;

namespace School
{

    internal class Program
    {
        static void Main(string[] args)
        {
            MainMenu.DisplayMenu();

        }

    }
}