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
            internal string therapyAreas;
        }



        class Reception
        {
            internal Reception() { }
            internal Reception(ref Reception rhs)
            {
                this.firstDay = rhs.firstDay;
                this.lastDay = rhs.lastDay;
                this.doctors = rhs.doctors;
                this.patients = rhs.patients;
                this.therapyAreas = rhs.therapyAreas;
                this.appointments = rhs.appointments;
            }
            internal bool CreateNewModel()
            {
                therapyAreas = new List<string>()
                {
                    "Pulmonologist",
                    "Surgeon",
                    "Rheumatologist",
                    "Neurologist",
                };

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
                    doctors[id] = newTherapist;
                }
                else if (therapyAreaId >= 0 && therapyAreaId < therapyAreas.Count)
                {
                    Specialist newSpecialist = new Specialist(id, therapyAreas[therapyAreaId]);
                    doctors[id] = newSpecialist;
                }
                else
                {
                    // TODO throw exception
                }
            }

            void addDay(DateTime newDay)
            {
                lastDay = lastDay.AddDays(1);

                appointments[newDay] = new Dictionary<int, Dictionary<int, int>>();
                foreach (KeyValuePair<int, Doctor> doc in doctors)
                {
                    appointments[newDay][doc.Value.id] = new Dictionary<int, int>();  //TODO сделать doctors сетом
                }
            }

            internal bool addAppointment_(ref string error, Patient patient, int doctorId = -1, DateTime date = new DateTime(), int time = 0)
            {
                if (doctorId == -1)
                {
                    foreach (KeyValuePair<int, Doctor> doc in doctors)
                    {
                        if (doc.Value is Therapist)
                            doctorId = doc.Value.id;
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

            protected bool addAppointment(Patient patient, int doctorId)
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
                return false;
            }

            protected bool addAppointment(Patient patient, int doctorId, DateTime dateTime)
            {
                appointments[dateTime.Date][doctorId][dateTime.Hour] = patient.id;
                patients.Add(patient);
                return true;
            }

            protected DateTime firstDay = DateTime.Today;
            protected DateTime lastDay = DateTime.Today;
            protected Dictionary<int, Doctor> doctors = new Dictionary<int, Doctor>();
            protected List<Patient> patients = new List<Patient>();
            protected List<string> therapyAreas;


            //день: { { id доктора, словарь записей на прием (время приема, id пациента) }, { ... }, { ... }, ... }
            //время приема целое число часов, с 9 до 19 в формате чч:00
            protected Dictionary<DateTime, Dictionary<int, Dictionary<int, int>>> appointments = new Dictionary<DateTime, Dictionary<int, Dictionary<int, int>>>();
        }

        class ManagerOfDoctors : Reception
        {
            internal ManagerOfDoctors() { }
            internal ManagerOfDoctors(ref Reception rhs) : base(ref rhs) { }
            internal bool AddDoctor(int id, int therapyAreaId = -1)
            {
                if (!doctors.ContainsKey(id))
                {
                    this.addDoctor(id, therapyAreaId);
                    return true;
                }
                else
                {
                    //TODO exception
                    return false;
                }
            }

            internal bool GetDoctorById(int id, ref Doctor outVal)
            {
                if (id >= 0 && id < doctors.Count)
                {
                    if (doctors.ContainsKey(id))
                    {
                        outVal = doctors[id];
                        return true;
                    }
                    else
                    {
                        //TODO exception
                        return false;
                    }
                }
                else
                {
                    //TODO throw exception no such doctorId
                    return false;
                }
            }

            internal bool GetDoctorsByTherapyAreaId (ref List<Doctor> outVal, int id = -1)
            {
                if (id >= 0 && id < therapyAreas.Count)
                {
                    for (int i = 0; i < doctors.Count; i++)
                    {
                        if (doctors[i] is Therapist && id == -1)
                        {
                            outVal.Add(doctors[i]);
                        }
                        else if (doctors[i] is Specialist && ((Specialist)doctors[i]).therapyAreas == therapyAreas[id])
                        {
                            outVal.Add(doctors[i]);
                        }
                    }
                }
                else
                {
                    //TODO exception: no such area
                    return false;
                }
                return true;
            }

            internal bool DeleteDoctor (int id)
            {                
                if (doctors.ContainsKey(id))
                {
                    //полагаем, что при удалении врача, все пациенты, которые к нему записаны, удаляются
                    foreach (KeyValuePair<DateTime, Dictionary<int, Dictionary<int, int>>> day in appointments)
                    {
                        day.Value.Remove(id);
                    }
                    doctors.Remove(id);
                }
                else
                {
                    //TODO exceprion: no such doctor
                    return false;
                }

                return true;
            }
        }

        class ManagerOfAppointments : Reception
        {
            internal ManagerOfAppointments() { }
            internal ManagerOfAppointments(ref Reception rhs) : base(ref rhs) { }

            internal bool AddAppointment(Patient patient, int doctorId = -1, DateTime dateTime = new DateTime())
            {
                if (!doctors.ContainsKey(doctorId))
                {
                    //TODO throw exception: no such doctor
                    return false;
                }

                if (doctorId == -1)
                {
                    foreach (KeyValuePair<int, Doctor> doc in doctors)
                    {
                        if (doc.Value is Therapist)
                            doctorId = doc.Value.id;
                    }
                }
                if (doctorId == -1)
                {
                    //TODO throw exception: no Therapist in that hospital
                    return false;
                }

                if (dateTime == new DateTime())
                {
                    // время записи не известно - записываем в первое попавшееся время. Полагаем, что пациенту все равно
                    if (!addAppointment(patient, doctorId))
                    {
                        //TODO exception "no free time";
                        return false;
                    }
                }
                else
                {
                    if (dateTime.Date >= firstDay.Date && dateTime.Date <= lastDay.Date && dateTime.Hour >= 9 && dateTime.Hour <= 19)
                    {
                        if (addAppointment(patient, doctorId, dateTime))
                        {
                            return true;
                        }
                        else
                        {
                            //TODO запись занята
                            return false;
                        }
                    }
                    else
                    {
                        //TODO exception "incorrect date";
                        return false;
                    }
                }
                return true;
            }
        
            internal bool GetAppointmentsForDay(DateTime dateTime, ref Dictionary<int, Dictionary<int, int>> outVal)
            {
                if (appointments.ContainsKey(dateTime.Date))
                {
                    outVal = appointments[dateTime.Date];
                    return true;
                }
                else
                {
                    return false;
                }
            }

            internal bool GetAppointmentsForDoctor(int id, ref Dictionary<DateTime, Dictionary<int, int>> outVal)
            {
                if (doctors.ContainsKey(id))
                {
                    foreach (KeyValuePair<DateTime, Dictionary<int, Dictionary<int, int>>> day in appointments)
                    {
                        outVal[day.Key] = day.Value[id];
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            internal bool GetPatient(DateTime dateTime, int doctorId, ref int outVal)
            {
                if (!doctors.ContainsKey(doctorId))
                {
                    //TODO error: no such doctor
                    return false;
                }
                if (!(dateTime.Date >= firstDay.Date && dateTime.Date <= lastDay.Date && dateTime.Hour >= 9 && dateTime.Hour <= 19))
                {
                    //TODO incorrect dateTime
                    return false;
                }

                if (appointments[dateTime.Date][doctorId].ContainsKey(dateTime.Hour))
                {
                    outVal = appointments[dateTime.Date][doctorId][dateTime.Hour];
                    return true;
                }
                else
                {
                    //TODO no such appointment
                    return false;
                }
            }

            internal bool DeleteAppointment(DateTime dateTime, int doctorId)
            {
                int patientId = 0;
                if (GetPatient(dateTime, doctorId, ref patientId))
                {
                    appointments[dateTime.Date][doctorId].Remove(dateTime.Hour);
                    patients.RemoveAt(patientId);
                    return true;
                }
                else
                {
                    //TODO no such Appointment
                    return false;
                }
            }
        
            internal bool ChangeAppointment(DateTime prevDateTime, int doctorId, DateTime newDateTime)
            {
                int patientId = -1;
                if (GetPatient(prevDateTime, doctorId, ref patientId))
                {
                    if (DeleteAppointment(prevDateTime, doctorId))
                    {
                        if (AddAppointment(patients[patientId], doctorId, newDateTime))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        static void Main(string[] args)
        {
            Reception rep = new Reception();
            if (rep.CreateNewModel())
                Console.WriteLine("new model created");

            Patient leaf = new Patient(1);
            Patient leafi = new Patient(0);
            /*string error = "";
            if (!rep.addAppointment(ref error, leaf, 1))
                Console.WriteLine("Not ok 1");

            Patient leafi = new Patient(0);
            if (!rep.addAppointment(ref error, leafi, 1, new DateTime(2019, 11, 1), 16))
                Console.WriteLine("Not ok 2");*/

            ManagerOfAppointments appointments = new ManagerOfAppointments(ref rep);
            if (!appointments.AddAppointment(leaf, 1))
                Console.WriteLine("Not ok 1");

            if (!appointments.AddAppointment(leafi, 1, new DateTime(2019, 11, 1, 16, 0, 0)))
                Console.WriteLine("Not ok 2");


            ManagerOfDoctors manager = new ManagerOfDoctors(ref rep);
            Doctor doc = new Doctor();
            if (manager.GetDoctorById(0, ref doc))
                Console.WriteLine(doc.id);
            else
                Console.WriteLine("Not ok 3");

            List<Doctor> docs = new List<Doctor>();
            if (manager.GetDoctorsByTherapyAreaId(ref docs, 1))
                for (int i = 0; i < docs.Count; i++)
                    Console.WriteLine(docs[i].id);
            else
                Console.WriteLine("Not ok 4");

            if (!manager.DeleteDoctor(0))
                Console.WriteLine("Not ok 5");

            if (manager.GetDoctorById(0, ref doc))
                Console.WriteLine("Not ok 6");

            Console.WriteLine("End");
        }
    }
}
