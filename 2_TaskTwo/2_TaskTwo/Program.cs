using System;
using System.Collections.Generic;

namespace _2_TaskTwo
{
    class Program
    {
        public class Patient
        {
            public Patient(int id)
            {
                this.id = id;
            }
            public int id;
        }

        public class Doctor
        {
            public Doctor(int id)
            {
                this.id = id;
            }
            public int id;
        }

        public class Therapist : Doctor
        {
            public Therapist(int id) : base(id)
            {

            }
        }

        public class Specialist : Doctor
        {
            public Specialist(int id, string therapyArea) : base(id)
            {
                this.therapyArea = therapyArea;
            }
            string therapyArea;
        }


        public class Reception
        {
            public Reception()
            {
                for (int i = 0; i < 4; i++)
                {
                    addDoctor(i, i % therapyArea.Count);
                }

                DateTime currentDay = DateTime.Today;
                for (int i = 0; i < 10; i++)
                {
                    addDay(currentDay);
                    currentDay = currentDay.AddDays(1);
                }
            }

            void addDoctor(int id, int therapyAreaId = -1)
            {
                if (therapyAreaId == -1)
                {
                    Therapist newTherapist = new Therapist(id);
                    doctors.Add(newTherapist);
                }
                else if (therapyAreaId >= 0 && therapyAreaId < therapyArea.Count)
                {
                    Specialist newSpecialist = new Specialist(id, therapyArea[therapyAreaId]);
                    doctors.Add(newSpecialist);
                }
                else
                {
                    // TODO throw exception
                }

                doctorsNumber++;
            }

            void addDay(DateTime newDay)
            {
                lastDay = lastDay.AddDays(1);

                appointments[newDay] = new List<Dictionary<int, int>>();
                for (int i = 0; i < doctorsNumber; i++)
                {
                    appointments[newDay].Add(new Dictionary<int, int>());
                }
            }

            public bool addAppointment(ref string error, Patient patient, int doctorId = -1, DateTime date = new DateTime(), int time = 0)
            {
                /*if ((date.Day == 0 || date.Month == 0 || time == 0) && !(date.Day == 0 && date.Month == 0 && time == 0))
                {
                    error = "you must enter date.Day and time or nothing of them";
                    return false;
                }*/

                if (doctorId == -1)
                {
                    foreach (Doctor doc in doctors)
                    {
                        if (doc is Therapist)
                            doctorId = doc.id;
                    }
                }
                if (doctorId == -1)
                {
                    error = "no Therapist in that hospital";
                    return false;
                }

                if (date.Day != 0 && time != 0 && date.Month != 0)
                {
                    if (date < firstDay || date > lastDay || time < 9 || time > 19)
                    {
                        error = "incorrect date";
                        return false;
                    }

                    if (!appointments[date][doctorId].ContainsKey(time))
                    {
                        appointments[date][doctorId][time] = patient.id;

                        patients.Add(patient);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (date == new DateTime())
                {
                    DateTime currentDay = firstDay;
                    while (currentDay != lastDay)
                    {
                        for (int i = 9; i < 19; i++)
                        {
                            if (!appointments[currentDay][doctorId].ContainsKey(i))
                            {
                                appointments[currentDay][doctorId][i] = patient.id;
                                
                                patients.Add(patient);
                                return true;
                            }
                        }
                    }
                    error = "no free time";
                    return false;
                }
                return false;
            }

            DateTime firstDay = DateTime.Today;
            DateTime lastDay = DateTime.Today;
            int doctorsNumber = 0;
            List<Doctor> doctors = new List<Doctor>();
            List<Patient> patients = new List<Patient>();
            List<string> therapyArea = new List<string>()
            {
                "Pulmonologist",
                "Surgeon",
                "Rheumatologist",
                "Neurologist",
            };


            //день: { { id доктора, словарь записей на прием (время приема, id пациента) }, { ... }, { ... }, ... }
            //время приема целое число часов, с 9 до 19 в формате чч:00
            Dictionary<DateTime, List<Dictionary<int, int>>> appointments = new Dictionary<DateTime, List<Dictionary<int, int>>>();
        }



        static void Main(string[] args)
        {
            Reception rep = new Reception();
            Console.WriteLine("uru uru");

            Patient leaf = new Patient(1);
            string error = "";
            if (!rep.addAppointment(ref error, leaf, 1))
                Console.WriteLine("Not ok 1");

            Patient leafi = new Patient(0);
            if (!rep.addAppointment(ref error, leafi, 1, new DateTime(2019, 11, 1), 16))
                Console.WriteLine("Not ok 2");

            Console.WriteLine("End");
        }
    }
}
