using System;
using System.Linq;

namespace Task_2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (T_6Context db = new T_6Context())
            {
                var Patients = db.Patients.ToList();
                Console.WriteLine("Patients list:");
                foreach (Patient u in Patients)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age} - {u.Diagnosis}");
                }
            }
        }
    }
}
