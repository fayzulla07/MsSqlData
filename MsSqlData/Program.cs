using MyXaN.Service.Models;
using SqlData;
using SqlData.SqlGenerator;
using System;

namespace MsSqlData
{
    class Program
    {
        static void Main(string[] args)
        {
            Cs.CsStr = "Data Source=DESKTOP-8ETS1SJ;Initial Catalog=XAN;Integrated Security=True";
            ISqlData data = new SqlGenerator();
            var result = data.GetAll<Users>();
            foreach (var item in result)
            {
                Console.WriteLine(item.FIO);
            }
            Console.ReadKey();
        }
    }
}
