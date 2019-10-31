using System;
using System.Collections.Generic;

namespace _2_TaskTwo
{
    class Program
    {
        class Patient
        {
            internal Patient(int id)
            {
                this.id = id;
            }
            internal int id;
        }

        class Doctor
        {
            internal Doctor(int id = -1)
            {
                this.id = id;
            }
            internal int id;
        }

        class Therapist : Doctor
        {
            internal Therapist(int id) : base(id)
            {

            }
        }

        class Specialist : Doctor
        {
            internal Specialist(int id, string therapyAreas) : base(id)
            {
                this.therapyAreas = therapyAreas;
            }
            string therapyAreas;
        }



        class Reception
        {
            internal Reception() { }
            internal Reception(ref Reception rhs)
            {
                this.firstDay = rhs.firstDay;
                this.lastDay = rhs.lastDay;
                this.doctorsNumber = rhs.doctorsNumber;
                this.doctors = rhs.doctors;
                this.patients = rhs.patients;
                this.therapyAreas = rhs.therapyAreas;
                this.appointments = rhs.appointments;
            }
            internal bool CreateNewModel()
            {
                for (int i = 0; i < 4; i++)
                {
                    addDoctor(i, i % therapyAreas.Count);
                }

                DateTime currentDay = DateTime.Today;
                for (int i = 0; i < 10; i++)
                {
                    addDay(currentDay);
                    currentDay = currentDay.AddDays(1);
                }

                return true;
            }

            protected void addDoctor(int id = -1, int therapyAreaId = -1) // TODO принимает тип Doctor
            {
                if (id == -1)
                {
                    id = doctors.Count;
                }
                if (therapyAreaId == -1)
                {
                    Therapist newTherapist = new Therapist(id);
                    doctors.Add(newTherapist);
                }
                else if (therapyAreaId >= 0 && therapyAreaId < therapyAreas.Count)
                {
                    Specialist newSpecialist = new Specialist(id, therapyAreas[therapyAreaId]);
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

            internal bool addAppointment(ref string error, Patient patient, int doctorId = -1, DateTime date = new DateTime(), int time = 0)
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
            protected int doctorsNumber = 0;
            protected List<Doctor> doctors = new List<Doctor>();
            List<Patient> patients = new List<Patient>();
            protected List<string> therapyAreas = new List<string>()
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

        class ManagerOfDoctors : Reception
        {
            internal ManagerOfDoctors() { }
            internal ManagerOfDoctors(ref Reception rhs) : base(ref rhs) { }
            internal void AddDoctor(int id, int therapyAreaId = -1)
            {
                ;
            }

            internal bool GetDoctorById(int id, ref Doctor outVal)
            {
                return false;
            }

            internal bool GetDoctorsByTherapyAreaId (int id, ref List<Doctor> outVal)
            {
                return true;
            }

            internal bool DeleteDoctor (int id)
            {
                return true;
            }

            internal bool ChangeDoctor (int id /* smth */)
            {
                return true;
            }
        }




        /*internal class A
        {
            internal A() { }
            internal A(ref A rhs)
            {
                this.a = rhs.a;
            }
            internal void changeA()
            {
                a = 13;
            }
            protected int a = 0;

            internal class C
            {
                internal C(int a)
                {
                    this.a = a;
                }
                int a;
                internal void returnA()
                {

                    Console.WriteLine(a);
                }
            }
        }

        internal class B : A
        {
            internal B() { }
            internal B(ref A rhs) : base(ref rhs) { }
            internal void returnA()
            {

                Console.WriteLine(a);
            }
        }*/


        static void Main(string[] args)
        {
            /*A mai = new A();
            mai.changeA();
            B unluck = new B();
            B luck = new B(ref mai);

            unluck.returnA();
            luck.returnA();*/



            Reception rep = new Reception();
            if (rep.CreateNewModel())
                Console.WriteLine("new model created");

            Patient leaf = new Patient(1);
            string error = "";
            if (!rep.addAppointment(ref error, leaf, 1))
                Console.WriteLine("Not ok 1");

            Patient leafi = new Patient(0);
            if (!rep.addAppointment(ref error, leafi, 1, new DateTime(2019, 11, 1), 16))
                Console.WriteLine("Not ok 2");

            
            ManagerOfDoctors manager = new ManagerOfDoctors(ref rep);
            Doctor doc = new Doctor();
            if (manager.GetDoctorById(0, ref doc))
                Console.WriteLine(doc.id);
            else
                Console.WriteLine("Not ok 3");

            Console.WriteLine("End");
        }
    }
}
