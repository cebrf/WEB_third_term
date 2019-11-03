using System;
using System.Collections.Generic;
using System.Linq;


namespace _2_TaskTwo
{
    class Program
    {
        public class Diagnosis
        {
            public Diagnosis(int id, string title, int deathRate = 0)
            {
                this.id = id;
                this.title = title;
                this.deathRate = ((deathRate >= 10 && deathRate <= 90) ? GetRand() : deathRate);
            }

            private int GetRand()
            {
                Random random = new Random();
                return random.Next(10, 91);
            }

            public int id;
            internal string title;
            internal int deathRate;
        }
        
        class Patient
        {
            internal Patient(int id)
            {
                this.id = id;
                this.diagnosisId = -1;
                this.appointmentsNumber = 0;
            }
            internal int id;
            internal int diagnosisId;
            internal int appointmentsNumber;
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
            internal Therapist(int id) : base(id) { }
            internal int ExecuteMedicalExamination(Patient patient, int specialistNumber)
            {
                Random rnd = new Random();
                return rnd.Next(0, specialistNumber);
            }
        }

        class Specialist : Doctor
        {
            internal Specialist(int id, int therapyAreaId) : base(id)
            {
                this.therapyAreaId = therapyAreaId;
            }
            internal int therapyAreaId;
            internal int EstablishDiagnosis(Patient patient, int diagnosesNumber)
            {
                Random rnd = new Random();
                return rnd.Next(0, diagnosesNumber);
            }
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
                this.diagnoses = rhs.diagnoses;
                this.appointments = rhs.appointments;
                this.specialistsNumber = rhs.specialistsNumber;
            }
            internal void CreateNewModel()
            {
                therapyAreas = new Dictionary<int, Tuple<string, List<int>>>()
                {
                    { 1, new Tuple<string, List<int>>("Pulmonologist", new List<int>() { 0, 1, 2 } ) },
                    { 2, new Tuple<string, List<int>>("Surgeon", new List<int>() { } ) },
                    { 3, new Tuple<string, List<int>>("Rheumatologist", new List<int>() { } ) },
                    { 4, new Tuple<string, List<int>>("Neurologist", new List<int>() { } ) },
                };
                diagnoses = new Dictionary<int, Diagnosis>()
                {
                    { 0, new Diagnosis(0, "pneumonia", 50) },
                    { 1, new Diagnosis(1, "tuberculosis", 66) },
                    { 2, new Diagnosis(2, "lung cancer", 83) },
                };
                doctors = new Dictionary<int, Doctor>()
                {
                    { 0, new Therapist(0) },
                    { 1, new Specialist(1, 2) },
                    { 3, new Specialist(3, 4) },
                    { 5, new Therapist(5) },
                };
                specialistsNumber = 2;
                                
                firstDay = DateTime.Today;
                lastDay = DateTime.Today.AddDays(-1);
                for (int i = 0; i < 10; i++)
                {
                    addDay();
                }
            }
            protected void addDoctor(int id = -1, int therapyAreaId = -1)
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
                else
                {
                    Specialist newSpecialist = new Specialist(id, therapyAreaId);
                    doctors[id] = newSpecialist;
                    specialistsNumber++;
                }
            }
            void addDay()
            {
                lastDay = lastDay.AddDays(1);

                appointments[lastDay] = new Dictionary<int, Dictionary<int, int>>();
                foreach (KeyValuePair<int, Doctor> doc in doctors)
                {
                    appointments[lastDay][doc.Value.id] = new Dictionary<int, int>();
                }
            }
            internal void NextDay()
            {
                DateTime currentDay = firstDay;
                firstDay = firstDay.AddDays(1);
                addDay();
                foreach (KeyValuePair<int, Doctor> doc in doctors)
                {
                    foreach (KeyValuePair<int, int> appointment in appointments[currentDay][doc.Key])
                    {
                        int time = appointment.Key;
                        int patientId = appointment.Value;
                        if (doc.Value is Specialist)
                        {
                            int diagnosisId = ((Specialist)doc.Value).EstablishDiagnosis(patients[patientId], diagnoses.Count);
                            patients[patientId].diagnosisId = diagnoses.ElementAt(diagnosisId).Key;
                            
                            Console.WriteLine(patients[patientId].id + "  has  " + diagnoses[patients[patientId].diagnosisId].title + "  (doctor " + doc.Key + " " + therapyAreas[((Specialist)doc.Value).therapyAreaId].Item1 + ")");

                            patients[patientId].diagnosisId--;
                            if (patients[patientId].diagnosisId == 0)
                                patients.Remove(patientId);
                        }
                        else
                        {
                            int doctorId = ((Therapist)doc.Value).ExecuteMedicalExamination(patients[patientId], specialistsNumber);
                            int specialistId = -1;
                            foreach (KeyValuePair<int, Doctor> doct in doctors)
                            {
                                specialistId++;
                                if (doct.Value is Specialist)
                                {
                                    if (specialistId == doctorId)
                                    {
                                        specialistId = doct.Key;
                                        break;
                                    }   
                                }
                            }
                            specialistId = (specialistId == -1 ? 0 : specialistId);

                            Patient currentPatient = patients[patientId];
                            patients[patientId].diagnosisId--;
                            if (patients[patientId].diagnosisId == 0)
                                patients.Remove(patientId);
                            addAppointment(currentPatient, specialistId);
                        }
                    }
                }

                appointments.Remove(currentDay);
            }
            protected bool addAppointment(Patient patient, int doctorId)
            {
                DateTime currentDay = firstDay;
                while (currentDay != lastDay.AddDays(1))
                {
                    for (int i = 9; i < 19; i++)
                    {
                        if (!appointments[currentDay][doctorId].ContainsKey(i))
                        {
                            appointments[currentDay][doctorId][i] = patient.id;

                            if (!patients.ContainsKey(patient.id))
                            {
                                patients[patient.id] = patient;
                            }
                            patients[patient.id].appointmentsNumber++;
                            return true;
                        }
                    }
                }
                return false;
            }
            protected void addAppointment(Patient patient, int doctorId, DateTime dateTime)
            {
                if (appointments[dateTime.Date][doctorId].ContainsKey(dateTime.Hour))
                    throw new System.InvalidOperationException("this time is already occupied");

                appointments[dateTime.Date][doctorId][dateTime.Hour] = patient.id;
                if (!patients.ContainsKey(patient.id))
                {
                    patients[patient.id] = patient;
                }
                patients[patient.id].appointmentsNumber++;
            }

            protected DateTime firstDay = DateTime.Today;
            protected DateTime lastDay = DateTime.Today;
            protected Dictionary<int, Doctor> doctors = new Dictionary<int, Doctor>();
            protected Dictionary<int, Patient> patients = new Dictionary<int, Patient>();
            protected int specialistsNumber = 0;
            protected Dictionary<int, Tuple<string, List<int>>> therapyAreas = new Dictionary<int, Tuple<string, List<int>>>();
            public Dictionary<int, Tuple<string, List<int>>> TherapyAreas
            {
                get
                {
                    return therapyAreas;
                }
            }
            protected Dictionary<int, Diagnosis> diagnoses;

            protected Dictionary<DateTime, Dictionary<int, Dictionary<int, int>>> appointments = new Dictionary<DateTime, Dictionary<int, Dictionary<int, int>>>();
        }
        
        class ManagerOfDoctors : Reception
        {
            internal ManagerOfDoctors() { }
            internal ManagerOfDoctors(ref Reception rhs) : base(ref rhs) { }
            
            internal void AddDoctor(int id, int therapyAreaId = -1)
            {
                if (!doctors.ContainsKey(id))
                {
                    if (therapyAreas.ContainsKey(therapyAreaId) || therapyAreaId == -1)
                    {
                        this.addDoctor(id, therapyAreaId);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("therapyArea with such id doesn't exist");
                    }
                }
                else
                {
                    throw new System.InvalidOperationException("doctor with such id has already exist");
                }
            }
            internal void GetDoctorById(int id, ref Doctor outVal)
            {
                if (doctors.ContainsKey(id))
                {
                    outVal = doctors[id];
                }
                else
                {
                    throw new System.InvalidOperationException("no doctor with such id");
                }
            }
            internal void GetDoctorsByTherapyAreaId (ref List<Doctor> outVal, int id = -1)
            {
                if (therapyAreas.ContainsKey(id))
                {
                    foreach (KeyValuePair<int, Doctor> doc in doctors)
                    {
                        if (doc.Value is Therapist && id == -1)
                        {
                            outVal.Add(doc.Value);
                        }
                        else if (doc.Value is Specialist && ((Specialist)doc.Value).therapyAreaId == id)
                        {
                            outVal.Add(doc.Value);
                        }
                    }
                }
                else
                {
                    throw new System.InvalidOperationException("no area with such id");
                }
            }
            internal void DeleteDoctor (int id)
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
                    throw new System.InvalidOperationException("no doctor with such id");
                }
            }
        }

        class ManagerOfAppointments : Reception
        {
            internal ManagerOfAppointments() { }
            internal ManagerOfAppointments(ref Reception rhs) : base(ref rhs) { }

            internal void AddAppointment(Patient patient, int doctorId = -1, DateTime dateTime = new DateTime())
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
                    throw new System.InvalidOperationException("no Therapist in that hospital");
                }

                if (!doctors.ContainsKey(doctorId))
                {
                    throw new System.InvalidOperationException("no doctor with such id");
                }

                if (dateTime == new DateTime())
                {
                    // время записи не известно - записываем в первое попавшееся время. Полагаем, что пациенту все равно
                    if (!addAppointment(patient, doctorId))
                    {
                        throw new System.InvalidOperationException("no free time");
                    }
                }
                else
                {
                    if (dateTime.Date < firstDay.Date)
                        throw new System.InvalidOperationException("you can't get the appointment for past days");
                    if (dateTime.Date > lastDay.Date)
                        throw new System.InvalidOperationException("appointments are open only 10 days in advance. Choose another day");
                    if (dateTime.Hour < 9 || dateTime.Hour > 19)
                        throw new System.InvalidOperationException("clinic is closed at that time");

                    addAppointment(patient, doctorId, dateTime);
                }
            }
            internal void GetAppointmentsForDay(DateTime dateTime, ref Dictionary<int, Dictionary<int, int>> outVal)
            {
                if (dateTime.Date < firstDay.Date)
                    throw new System.InvalidOperationException("you can't get the appointment for past days");
                if (dateTime.Date > lastDay.Date)
                    throw new System.InvalidOperationException("appointments are open only 10 days in advance. Choose another day");

                if (appointments.ContainsKey(dateTime.Date))
                {
                    outVal = appointments[dateTime.Date];
                }
                else
                {
                    throw new System.InvalidOperationException("empty day");
                }
            }
            internal void GetAppointmentsOfDoctor(int id, ref Dictionary<DateTime, Dictionary<int, int>> outVal)
            {
                if (doctors.ContainsKey(id))
                {
                    foreach (KeyValuePair<DateTime, Dictionary<int, Dictionary<int, int>>> day in appointments)
                    {
                        outVal[day.Key] = day.Value[id];
                    }
                }
                else
                {
                    throw new System.InvalidOperationException("no doctor with such id");
                }
            }
            internal void GetPatient(DateTime dateTime, int doctorId, ref int outVal)
            {
                if (!doctors.ContainsKey(doctorId))
                    throw new System.InvalidOperationException("no such doctor");
                if (dateTime.Date < firstDay.Date)
                    throw new System.InvalidOperationException("you can't get the appointment for past days");
                if (dateTime.Date > lastDay.Date)
                    throw new System.InvalidOperationException("appointments are open only 10 days in advance. Choose another day");
                if (dateTime.Hour < 9 || dateTime.Hour > 19)
                    throw new System.InvalidOperationException("clinic is closed at that time");
                if (!appointments[dateTime.Date][doctorId].ContainsKey(dateTime.Hour))
                    throw new System.InvalidOperationException("no such appointment");

                outVal = appointments[dateTime.Date][doctorId][dateTime.Hour];
            }
            internal void DeleteAppointment(DateTime dateTime, int doctorId)
            {
                int patientId = 0;
                GetPatient(dateTime, doctorId, ref patientId);
                appointments[dateTime.Date][doctorId].Remove(dateTime.Hour);
                if (patients[patientId].diagnosisId == 1)
                {
                    patients.Remove(patientId);
                }
                else
                {
                    patients[patientId].diagnosisId--;
                }
            }
            internal void ChangeAppointment(DateTime prevDateTime, int doctorId, DateTime newDateTime)
            {
                if (prevDateTime != newDateTime)
                {
                    int patientId = -1;
                    GetPatient(prevDateTime, doctorId, ref patientId);
                    AddAppointment(patients[patientId], doctorId, newDateTime);
                    DeleteAppointment(prevDateTime, doctorId);
                }
            }
        }

        class ManagerOfTherapyAreas : Reception
        {
            internal ManagerOfTherapyAreas() { }
            internal ManagerOfTherapyAreas(ref Reception rhs) : base(ref rhs) { }
            
            internal void AddTherapyArea(int id, string title, List<int> diagnosesId)
            {
                if (!therapyAreas.ContainsKey(id))
                {
                    if (id == -1)
                    {
                        id = therapyAreas.Count;
                    }
                    foreach (int el in diagnosesId)
                    {
                        if (!diagnoses.ContainsKey(el))
                        {
                            throw new System.InvalidOperationException("no diagnosis with such id");
                        }
                    }
                    therapyAreas[id] = new Tuple<string, List<int>>(title, diagnosesId);
                }
                else
                {
                    throw new System.InvalidOperationException("therapy area with such is has already exist");
                }
            }
            internal void GetTherapyAreaById(int id, ref Tuple<string, List<int>> outVal)
            {
                if (therapyAreas.ContainsKey(id))
                {
                    outVal = therapyAreas[id];
                }
                else
                {
                    throw new System.InvalidOperationException("no therapyArea with such id");
                }
            }
        }
        
        class managerOfDiagnosis : Reception
        {
            internal managerOfDiagnosis() { }
            internal managerOfDiagnosis(ref Reception rhs) : base(ref rhs) { }

            internal void AddDiagnosis(int id, string title, int deathRate = 0)
            {
                if (!diagnoses.ContainsKey(id))
                {
                    diagnoses[id] = new Diagnosis(id, title, deathRate);
                }
                else
                {
                    throw new System.InvalidOperationException("diagnosis with such id has already exist");
                }
            }
            internal void GetDiagnosisById(int id, ref Diagnosis outVal)
            {
                if (diagnoses.ContainsKey(id))
                {
                    outVal = diagnoses[id];
                }
                else
                {
                    throw new System.InvalidOperationException("no diagnosis with such id");
                }
            }
            internal void GetDiagnosesByTherapyAreaId(int id, ref List<Diagnosis> outVal)
            {
                if (therapyAreas.ContainsKey(id))
                {
                    for (int i = 0; i < therapyAreas[id].Item2.Count; i++)
                        outVal.Add(diagnoses[therapyAreas[id].Item2[i]]);
                }
                else
                {
                    throw new System.InvalidOperationException("no therapyArea with such id");
                }
            }
            internal void GetDiagnosesByDeathRate(int deathRate, ref List<Diagnosis> outVal)
            {
                outVal.Clear();
                foreach (KeyValuePair<int, Diagnosis> pair in diagnoses)
                {
                    if (pair.Value.deathRate == deathRate)
                        outVal.Add(pair.Value);
                }
                if (outVal.Count == 0)
                {
                    throw new System.InvalidOperationException("no diagnoses with such death rate");
                }
            }
            internal void ChangeDeathRate(int id, int newDeathRate)
            {
                if (diagnoses.ContainsKey(id))
                {
                    diagnoses[id].deathRate = newDeathRate;
                }
                else
                {
                    throw new System.InvalidOperationException("no diagnosis with such id");
                }
            }
            internal void DeleteDiagnosis(int id)
            {
                if (diagnoses.ContainsKey(id))
                {
                    foreach (KeyValuePair<int, Tuple<string, List<int>>> area in therapyAreas)
                    {
                        area.Value.Item2.Remove(id);
                    }
                    diagnoses.Remove(id);
                }
                else
                {
                    throw new System.InvalidOperationException("no diagnosis with such id");
                }
            }
        }

        static internal void ConsoleMode()
        {
            Reception rep = new Reception();
            rep.CreateNewModel();
            while (true)
            {
                Console.WriteLine("You are in Console mod now\nChoose operating mode.\nManager of: doctors(1), therapyAreas(2), diagnosis(3), appointments(4)\n");
                string mode = Console.ReadLine();
                if (mode == "doctors" || mode == "1")
                {
                    managerOfDoctorsMode();
                    continue;
                }
                if (mode == "therapyAreas" || mode == "2")
                {
                    managerOfTherapyAreas();
                    continue;
                }
                if (mode == "diagnosis" || mode == "3")
                {
                    managerOfDiagnosis();
                    continue;
                }
                if (mode == "appointments" || mode == "4")
                {
                    managerOfAppointments();
                    continue;
                }
                if (mode == "exit")
                {
                    return;
                }
                Console.WriteLine(mode + " is not an internal or external command, executable program, or batch file. Choose another operation");
            }

            void managerOfDoctorsMode()
            {
                ManagerOfDoctors manager = new ManagerOfDoctors(ref rep);
                Console.WriteLine("You are in Manager of doctors mode now");

                while (true)
                {
                    Console.WriteLine("Choose operation");
                    string operation = Console.ReadLine();

                    try
                    {
                        if (operation == "nextDay")
                        {
                            nextDay(manager);
                            continue;
                        }
                        if (operation == "add")
                        {
                            Console.WriteLine("enter id of doctor and his therapyAreaId");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);

                            enter = Console.ReadLine();
                            int therapyAreaId;
                            if (enter == "-")
                            {
                                manager.AddDoctor(id);
                                Console.WriteLine("doctor was added");
                            }
                            else
                            {
                                therapyAreaId = int.Parse(enter);
                                manager.AddDoctor(id, therapyAreaId);
                                Console.WriteLine("doctor was added");
                            }
                            continue;
                        }
                        if (operation == "getById")
                        {
                            Console.WriteLine("enter id of doctor");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);

                            Doctor doc = new Doctor();
                            manager.GetDoctorById(id, ref doc);
                            if (doc is Therapist)
                                Console.WriteLine(((Therapist)doc).id + " therapist");
                            else
                                Console.WriteLine(((Specialist)doc).id + " " + manager.TherapyAreas[((Specialist)doc).therapyAreaId].Item1);
                            continue;
                        }
                        if (operation == "getByTherapyAreaId")
                        {
                            Console.WriteLine("enter id of therapyArea");
                            List<Doctor> docs = new List<Doctor>();
                            string enter = Console.ReadLine();
                            int id = -1;
                            if (enter == "-")
                            {
                                manager.GetDoctorsByTherapyAreaId(ref docs);
                            }
                            else
                            {
                                id = int.Parse(enter);
                                manager.GetDoctorsByTherapyAreaId(ref docs, id);

                                for (int i = 0; i < docs.Count; i++)
                                {
                                    if (docs[i] is Therapist)
                                        Console.WriteLine(((Therapist)docs[i]).id + " therapist");
                                    else
                                        Console.WriteLine(((Specialist)docs[i]).id + " " + manager.TherapyAreas[((Specialist)docs[i]).therapyAreaId].Item1);
                                }
                            }
                            continue;
                        }
                        if (operation == "delete")
                        {
                            Console.WriteLine("enter id of doctor");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            manager.DeleteDoctor(id);
                            Console.WriteLine("Doctor deleted");
                            continue;
                        }
                        if (operation == "goBack")
                        {
                            return;
                        }
                        Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            }

            void managerOfDiagnosis()
            {
                managerOfDiagnosis manager = new managerOfDiagnosis(ref rep);
                Console.WriteLine("You are in Manager of diagnosis mode now");

                while (true)
                {
                    Console.WriteLine("Choose operation");
                    string operation = Console.ReadLine();

                    try
                    {
                        if (operation == "nextDay")
                        {
                            nextDay(manager);
                            continue;
                        }
                        if (operation == "add")
                        {
                            Console.WriteLine("enter id of diagnosis, its title and deathRate");
                            string enter = Console.ReadLine();

                            int id = int.Parse(enter);
                            string title = Console.ReadLine();
                            enter = Console.ReadLine();
                            if (enter == "-")
                            {
                                manager.AddDiagnosis(id, title);
                                Console.WriteLine("diagnosis was added");
                                continue;
                            }
                            else
                            {
                                int deathRate = int.Parse(enter);
                                manager.AddDiagnosis(id, title, deathRate);
                                Console.WriteLine("diagnosis was added");
                                continue;
                            }
                        }
                        if (operation == "getById")
                        {
                            Console.WriteLine("enter id of diagnosis");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            Diagnosis diagnosis = new Diagnosis(0, "");

                            manager.GetDiagnosisById(id, ref diagnosis);
                            Console.WriteLine(diagnosis.id + " " + diagnosis.title + " " + diagnosis.deathRate);
                            continue;
                        }
                        if (operation == "getByTherapyAreaId")
                        {
                            Console.WriteLine("enter id of therapyArea");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            List<Diagnosis> diagnoses = new List<Diagnosis>();

                            manager.GetDiagnosesByTherapyAreaId(id, ref diagnoses);
                            for (int i = 0; i < diagnoses.Count; i++)
                            {
                                Console.WriteLine(diagnoses[i].id + " " + diagnoses[i].title + " " + diagnoses[i].deathRate);
                            }
                            continue;
                        }
                        if (operation == "getByDeathRate")
                        {
                            Console.WriteLine("enter deathRate");
                            string enter = Console.ReadLine();
                            int deathRate = int.Parse(enter);
                            List<Diagnosis> diagnoses = new List<Diagnosis>();

                            manager.GetDiagnosesByDeathRate(deathRate, ref diagnoses);
                            for (int i = 0; i < diagnoses.Count; i++)
                            {
                                Console.WriteLine(diagnoses[i].id + " " + diagnoses[i].title + " " + diagnoses[i].deathRate);
                            }
                            continue;
                        }
                        if (operation == "changeDeathRate")
                        {
                            Console.WriteLine("enter id of diagnosis and new deathRate");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            enter = Console.ReadLine();
                            int deathRate = int.Parse(enter);

                            manager.ChangeDeathRate(id, deathRate);
                            Console.WriteLine("deathRate was changed");
                            continue;
                        }
                        if (operation == "delete")
                        {
                            Console.WriteLine("enter id of diagnosis");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);

                            manager.DeleteDiagnosis(id);
                            Console.WriteLine("diagnosis was deleted");
                            continue;
                        }
                        if (operation == "goBack")
                        {
                            return;
                        }
                        Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            }

            void managerOfTherapyAreas()
            {
                ManagerOfTherapyAreas manager = new ManagerOfTherapyAreas(ref rep);
                Console.WriteLine("You are in Manager of therapyAreas mode now");

                while (true)
                {
                    Console.WriteLine("Choose operation");
                    string operation = Console.ReadLine();

                    try
                    {
                        if (operation == "nextDay")
                        {
                            nextDay(manager);
                            continue;
                        }
                        if (operation == "add")
                        {
                            Console.WriteLine("enter id of therapyAreas, its title and diagnosesId");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            string title = Console.ReadLine();
                            enter = Console.ReadLine();
                            string[] diagnosesId_ = enter.Split(" ");
                            List<int> diagnosesId = new List<int>();
                            for (int i = 0; i < diagnosesId_.Length; i++)
                            {
                                diagnosesId.Add(int.Parse(diagnosesId_[i]));
                            }

                            manager.AddTherapyArea(id, title, diagnosesId);
                            Console.WriteLine("therapyArea was added");
                            continue;
                        }
                        if (operation == "getById")
                        {
                            Console.WriteLine("enter id of therapyArea");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            Tuple<string, List<int>> therapyArea = new Tuple<string, List<int>>("", new List<int>());

                            manager.GetTherapyAreaById(id, ref therapyArea);
                            Console.Write(therapyArea.Item1 + "  ");
                            for (int i = 0; i < therapyArea.Item2.Count; i++)
                            {
                                Console.Write(therapyArea.Item2[i] + " ");
                            }
                            continue;
                        }
                        if (operation == "goBack")
                        {
                            return;
                        }
                        Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            }

            void managerOfAppointments()
            {
                ManagerOfAppointments manager = new ManagerOfAppointments(ref rep);
                Console.WriteLine("You are in Manager of appointments mode now");

                while (true)
                {
                    Console.WriteLine("Choose operation");
                    string operation = Console.ReadLine();

                    try
                    {
                        if (operation == "nextDay")
                        {
                            nextDay(manager);
                            continue;
                        }
                        if (operation == "add")
                        {
                            Console.WriteLine("enter id of patient");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            Patient patient = new Patient(id);

                            Console.WriteLine("enter id of doctor");
                            int doctorId = -1;
                            enter = Console.ReadLine();
                            if (enter != "-")
                                doctorId = int.Parse(enter);

                            Console.WriteLine("enter date (yy/mm/dd hh)");
                            enter = Console.ReadLine();
                            DateTime dateTime = new DateTime();
                            if (enter == "-")
                            {
                                manager.AddAppointment(patient, doctorId);
                            }
                            else
                            {
                                enter += ":00:00";
                                dateTime = DateTime.Parse(enter);
                                manager.AddAppointment(patient, doctorId, dateTime);
                            }
                            Console.WriteLine("new appointment was added");
                            continue;
                        }
                        if (operation == "getAppointmentsForDay")
                        {
                            Console.WriteLine("enter date (yy/mm/dd)");
                            string enter = Console.ReadLine();
                            DateTime dateTime = DateTime.Parse(enter);
                            Dictionary<int, Dictionary<int, int>> outVal = new Dictionary<int, Dictionary<int, int>>();

                            manager.GetAppointmentsForDay(dateTime, ref outVal);
                            foreach (KeyValuePair<int, Dictionary<int, int>> doctorAppoint in outVal)
                            {
                                Console.Write(doctorAppoint.Key + ":  ");
                                if (doctorAppoint.Value.Count == 0)
                                {
                                    Console.Write("empty");
                                }
                                else
                                {
                                    foreach (KeyValuePair<int, int> appoint in doctorAppoint.Value)
                                    {
                                        Console.Write(appoint.Key + ":00  -  " + appoint.Value);
                                    }
                                }
                                Console.WriteLine();
                            }
                            continue;
                        }
                        if (operation == "getAppointmentsOfDoctor")
                        {
                            Console.WriteLine("enter id of doctor");
                            string enter = Console.ReadLine();
                            int id = int.Parse(enter);
                            Dictionary<DateTime, Dictionary<int, int>> appointmentsOfDoctor = new Dictionary<DateTime, Dictionary<int, int>>();

                            manager.GetAppointmentsOfDoctor(id, ref appointmentsOfDoctor);
                            foreach (KeyValuePair<DateTime, Dictionary<int, int>> day in appointmentsOfDoctor)
                            {
                                Console.Write(day.Key.Date + ":  ");
                                if (day.Value.Count == 0)
                                    Console.Write("empty");
                                else
                                {
                                    foreach (KeyValuePair<int, int> el in day.Value)
                                    {
                                        Console.Write(el.Key + ":00 - " + el.Value + ", ");
                                    }
                                }
                                Console.WriteLine();
                            }
                            continue;
                        }
                        if (operation == "getPatient")
                        {
                            Console.WriteLine("enter date (yy/mm/dd hh)");
                            string enter = Console.ReadLine() + ":00:00";
                            DateTime dateTime = DateTime.Parse(enter);
                            Console.WriteLine("enter id of doctor");
                            enter = Console.ReadLine();
                            int doctorId = int.Parse(enter);

                            int patientId = 0;
                            manager.GetPatient(dateTime, doctorId, ref patientId);
                            Console.WriteLine("patient id is " + patientId);
                            continue;
                        }
                        if (operation == "delete")
                        {
                            Console.WriteLine("enter date (yy/mm/dd hh)");
                            string enter = Console.ReadLine() + ":00:00";
                            DateTime dateTime = DateTime.Parse(enter);
                            Console.WriteLine("enter id of doctor");
                            int doctorId = int.Parse(enter);

                            manager.DeleteAppointment(dateTime, doctorId);
                            Console.WriteLine("appointment was deleted");
                            continue;
                        }
                        if (operation == "changeDateTime")
                        {
                            Console.WriteLine("enter previous date (yy/mm/dd hh)");
                            string enter = Console.ReadLine() + ":00:00";
                            DateTime prevDateTime = DateTime.Parse(enter);
                            Console.WriteLine("enter id of doctor");
                            enter = Console.ReadLine();
                            int doctorId = int.Parse(enter);
                            Console.WriteLine("enter new date (yy/mm/dd hh)");
                            enter = Console.ReadLine() + ":00:00";
                            DateTime newDateTime = DateTime.Parse(enter);

                            manager.ChangeAppointment(prevDateTime, doctorId, newDateTime);
                            Console.WriteLine("appointment was changed");
                            continue;
                        }
                        if (operation == "goBack")
                        {
                            return;
                        }
                        Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            }
        
            void nextDay(Reception reception)
            {
                reception.NextDay();
            }
        }

        static void Main(string[] args)
        {
            ConsoleMode();
        }
    }
}
