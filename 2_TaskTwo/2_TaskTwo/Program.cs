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

            internal bool CreateNewModel()
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

                return true;
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
            
            internal bool NextDay()
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
                            
                            Console.WriteLine(patients[patientId].id + "  has  " + diagnoses[patients[patientId].diagnosisId].title);
                            //TODO выводим информацию в консоль

                            patients[patientId].diagnosisId--;
                            if (patients[patientId].diagnosisId == 0)
                                patients.Remove(patientId);
                            //TODO добавить пациенту поле Кол-во записей и удалять пациента только, если записей больше нет
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
                return true;
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

            protected bool addAppointment(Patient patient, int doctorId, DateTime dateTime)
            {
                appointments[dateTime.Date][doctorId][dateTime.Hour] = patient.id;
                if (!patients.ContainsKey(patient.id))
                {
                    patients[patient.id] = patient;
                }
                patients[patient.id].appointmentsNumber++;
                return true;
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
            //день: { { id доктора, словарь записей на прием (время приема, id пациента) }, { ... }, { ... }, ... }
            //время приема целое число часов, с 9 до 19 в формате чч:00
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
                if (id >= 0 && id < therapyAreas.Count)
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

            internal bool AddAppointment(ref string error, Patient patient, int doctorId = -1, DateTime dateTime = new DateTime())
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

                if (!doctors.ContainsKey(doctorId))
                {
                    error = "no doctor with such id";
                    return false;
                }

                if (dateTime == new DateTime())
                {
                    // время записи не известно - записываем в первое попавшееся время. Полагаем, что пациенту все равно
                    if (!addAppointment(patient, doctorId))
                    {
                        error = "no free time";
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
                            error = "запись занята";
                            return false;
                        }
                    }
                    else
                    {
                        error = "incorrect date";
                        return false;
                    }
                }
                return true;
            }
        
            internal bool GetAppointmentsForDay(ref string error, DateTime dateTime, ref Dictionary<int, Dictionary<int, int>> outVal)
            {
                if (appointments.ContainsKey(dateTime.Date))
                {
                    outVal = appointments[dateTime.Date];
                    return true;
                }
                else
                {
                    error = "empty appointment";
                    return false;
                }
            }

            internal bool GetAppointmentsOfDoctor(ref string error, int id, ref Dictionary<DateTime, Dictionary<int, int>> outVal)
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
                    error = "no doctor with such id";
                    return false;
                }
            }

            internal bool GetPatient(ref string error, DateTime dateTime, int doctorId, ref int outVal)
            {
                if (!doctors.ContainsKey(doctorId))
                {
                    error = "no such doctor";
                    return false;
                }
                if (!(dateTime.Date >= firstDay.Date && dateTime.Date <= lastDay.Date && dateTime.Hour >= 9 && dateTime.Hour <= 19))
                {
                    error = "incorrect dateTime";
                    return false;
                }

                if (appointments[dateTime.Date][doctorId].ContainsKey(dateTime.Hour))
                {
                    outVal = appointments[dateTime.Date][doctorId][dateTime.Hour];
                    return true;
                }
                else
                {
                    error = "no such appointment";
                    return false;
                }
            }

            internal bool DeleteAppointment(ref string error, DateTime dateTime, int doctorId)
            {
                int patientId = 0;
                if (GetPatient(ref error, dateTime, doctorId, ref patientId))
                {
                    appointments[dateTime.Date][doctorId].Remove(dateTime.Hour);
                    if (patients[patientId].diagnosisId == 1)
                    {
                        patients.Remove(patientId);
                    }
                    else
                    {
                        patients[patientId].diagnosisId--;
                    }
                    return true;
                }
                else
                {
                    error = "no such Appointment";
                    return false;
                }
            }
        
            internal bool ChangeAppointment(ref string error, DateTime prevDateTime, int doctorId, DateTime newDateTime)
            {
                int patientId = -1;
                if (GetPatient(ref error, prevDateTime, doctorId, ref patientId))
                {
                    Patient patient = patients[patientId]; 
                    if (DeleteAppointment(ref error, prevDateTime, doctorId))
                    {
                        if (AddAppointment(ref error, patient, doctorId, newDateTime))
                        {
                            return true;
                        }
                    }
                }
                error = "no such appointment";
                return false;
            }
        }

        class ManagerOfTherapyAreas : Reception
        {
            internal ManagerOfTherapyAreas() { }
            internal ManagerOfTherapyAreas(ref Reception rhs) : base(ref rhs) { }
            internal bool AddTherapyArea(ref string error, int id, string title, List<int> diagnosesId)
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
                            error = "no diagnoses with such id";
                            return false;
                        }
                    }
                    therapyAreas[id] = new Tuple<string, List<int>>(title, diagnosesId);
                    return true;
                }
                else
                {
                    error = "therapy area with such is has already exist";
                    return false;
                }
            }
        
            internal bool GetTherapyAreaById(ref string error, int id, ref Tuple<string, List<int>> outVal)
            {
                if (therapyAreas.ContainsKey(id))
                {
                    outVal = therapyAreas[id];
                    return true;
                }
                else
                {
                    error = "no therapyArea with such id";
                    return false;
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
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
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
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                }
            }

            void managerOfTherapyAreas()
            {
                ManagerOfTherapyAreas manager = new ManagerOfTherapyAreas(ref rep);
                string error = "";
                Console.WriteLine("You are in Manager of therapyAreas mode now");

                while (true)
                {
                    Console.WriteLine("Choose operation");
                    string operation = Console.ReadLine();
                    if (operation == "add")
                    {
                        Console.WriteLine("enter id of therapyAreas, its title and diagnosesId");
                        string enter = Console.ReadLine();
                        int id;
                        if (!int.TryParse(enter, out id))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }
                        string title = Console.ReadLine();
                        enter = Console.ReadLine();
                        string[] diagnosesId_ = enter.Split(" ");
                        List<int> diagnosesId = new List<int>();
                        for (int i = 0; i < diagnosesId_.Length; i++)
                        {
                            int buf = 0;
                            if (!int.TryParse(diagnosesId_[i], out buf))
                            {
                                Console.WriteLine("incorrect input");
                                continue;
                            }
                            else
                            {
                                diagnosesId.Add(buf);
                            }
                            continue;
                        }
                        if (manager.AddTherapyArea(ref error, id, title, diagnosesId))
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("therapyArea was added");
                            continue;
                        }
                    }
                    if (operation == "getById")
                    {
                        //GetTherapyAreaById(ref string error, int id, ref Tuple<string, List<int>> outVal)
                        Console.WriteLine("enter id of therapyArea");
                        string enter = Console.ReadLine();
                        int id = -1;
                        if (!int.TryParse(enter, out id))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }
                        Tuple<string, List<int>> therapyArea = new Tuple<string, List<int>>("", new List<int>());
                        if (manager.GetTherapyAreaById(ref error, id, ref therapyArea))
                        {
                            Console.Write(therapyArea.Item1 + "  ");
                            for (int i = 0; i < therapyArea.Item2.Count; i++)
                            {
                                Console.Write(therapyArea.Item2[i] + " ");
                            }
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                    }
                    if (operation == "goBack")
                    {
                        return;
                    }
                    Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                }
            }

            void managerOfAppointments()
            {
                ManagerOfAppointments manager = new ManagerOfAppointments(ref rep);
                string error = "";
                Console.WriteLine("You are in Manager of appointments mode now");

                while (true)
                {
                    Console.WriteLine("Choose operation");
                    string operation = Console.ReadLine();
                    if (operation == "add")
                    {
                        Console.WriteLine("enter id of patient");
                        string enter = Console.ReadLine();
                        int id = -1;
                        if (!int.TryParse(enter, out id))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }
                        Patient patient = new Patient(id);

                        Console.WriteLine("enter id of doctor");
                        int doctorId = -1;
                        enter = Console.ReadLine();
                        if (enter != "-")
                        {
                            if (!int.TryParse(enter, out doctorId))
                            {
                                Console.WriteLine("incorrect input");
                                continue;
                            }
                        }

                        Console.WriteLine("enter date (yy/mm/dd hh)");
                        enter = Console.ReadLine() + ":00:00";
                        DateTime dateTime = new DateTime();
                        if (enter == "-")
                        {
                            if (manager.AddAppointment(ref error, patient, doctorId))
                            {
                                Console.WriteLine("new appointment was added");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine(error);
                                continue;
                            }
                        }
                        else
                        {
                            if (!DateTime.TryParse(enter, out dateTime))
                            {
                                Console.WriteLine("incorrect input");
                                continue;
                            }

                            if (manager.AddAppointment(ref error, patient, doctorId, dateTime))
                            {
                                Console.WriteLine("new appointment was added");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine(error);
                                continue;
                            }
                        }
                    }
                    if (operation == "getAppointmentsForDay")
                    {
                        Console.WriteLine("enter date (dd-mm-yy)");
                        string enter = Console.ReadLine();
                        DateTime dateTime = new DateTime();
                        if (!DateTime.TryParse(enter, out dateTime))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }
                        Dictionary<int, Dictionary<int, int>> outVal = new Dictionary<int, Dictionary<int, int>>();
                        if (manager.GetAppointmentsForDay(ref error, dateTime, ref outVal))
                        {
                            foreach(KeyValuePair<int, Dictionary<int, int>> doctorAppoint in outVal)
                            {
                                Console.Write(doctorAppoint.Key + "  ");
                                foreach(KeyValuePair<int, int> appoint in doctorAppoint.Value)
                                {
                                    Console.Write(appoint.Key + " " + appoint.Value);
                                }
                                Console.WriteLine();
                            }
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                    }
                    if (operation == "getAppointmentsOfDoctor")
                    {
                        Console.WriteLine("enter id of doctor");
                        string enter = Console.ReadLine();
                        int id = -1;
                        if (!int.TryParse(enter, out id))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }
                        Dictionary<DateTime, Dictionary<int, int>> appointmentsOfDoctor = new Dictionary<DateTime, Dictionary<int, int>>();
                        if (manager.GetAppointmentsOfDoctor(ref error, id, ref appointmentsOfDoctor))
                        {
                            foreach (KeyValuePair<DateTime, Dictionary<int, int>> day in appointmentsOfDoctor)
                            {
                                Console.Write(day.Key + ":  ");
                                foreach (KeyValuePair<int, int> el in day.Value)
                                {
                                    Console.Write(el.Key + "-" + el.Value + "; ");
                                }
                                Console.WriteLine();
                            }
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                    }
                    if (operation == "GetPatient")
                    {
                        Console.WriteLine("enter date (yy/mm/dd hh)");
                        string enter = Console.ReadLine() + ":00:00";
                        DateTime dateTime = new DateTime();
                        if (!DateTime.TryParse(enter, out dateTime))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }

                        Console.WriteLine("enter id of doctor");
                        int doctorId = -1;
                        enter = Console.ReadLine();
                        if (enter != "-")
                        {
                            if (!int.TryParse(enter, out doctorId))
                            {
                                Console.WriteLine("incorrect input");
                                continue;
                            }
                        }

                        int patientId = 0;
                        if (manager.GetPatient(ref error, dateTime, doctorId, ref patientId))
                        {
                            Console.WriteLine("patient id is " + patientId);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                    }
                    if (operation == "delete")
                    {
                        Console.WriteLine("enter date (yy/mm/dd hh)");
                        string enter = Console.ReadLine() + ":00:00";
                        DateTime dateTime = new DateTime();
                        if (!DateTime.TryParse(enter, out dateTime))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }

                        Console.WriteLine("enter id of doctor");
                        int doctorId = -1;
                        enter = Console.ReadLine();
                        if (enter != "-")
                        {
                            if (!int.TryParse(enter, out doctorId))
                            {
                                Console.WriteLine("incorrect input");
                                continue;
                            }
                        }

                        if (manager.DeleteAppointment(ref error, dateTime, doctorId))
                        {
                            Console.WriteLine("appointment was deleted");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                    }
                    if (operation == "changeDateTime")
                    {
                        Console.WriteLine("enter previous date (yy/mm/dd hh)");
                        string enter = Console.ReadLine() + ":00:00";
                        DateTime prevDateTime = new DateTime();
                        if (!DateTime.TryParse(enter, out prevDateTime))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }

                        Console.WriteLine("enter id of doctor");
                        int doctorId = -1;
                        enter = Console.ReadLine();
                        if (enter != "-")
                        {
                            if (!int.TryParse(enter, out doctorId))
                            {
                                Console.WriteLine("incorrect input");
                                continue;
                            }
                        }

                        Console.WriteLine("enter new date (yy/mm/dd hh)");
                        enter = Console.ReadLine() + ":00:00";
                        DateTime newDateTime = new DateTime();
                        if (!DateTime.TryParse(enter, out newDateTime))
                        {
                            Console.WriteLine("incorrect input");
                            continue;
                        }

                        if (manager.ChangeAppointment(ref error, prevDateTime, doctorId, newDateTime))
                        {
                            Console.WriteLine("appointment was changed");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            continue;
                        }
                    }
                    if (operation == "goBack")
                    {
                        return;
                    }
                    Console.WriteLine(operation + " is not an internal or external command, executable program, or batch file. Choose another operation");
                }
            }
        }

        static void Main(string[] args)
        {
            ConsoleMode();

            try
            {
                int a = 0;
                if (a == 0)
                {
                    throw new System.AccessViolationException("Logfile cannot be read-only");
                }
                Console.WriteLine("bad news");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("I am still alive!");











            /*Reception rep = new Reception();
            if (rep.CreateNewModel())
                Console.WriteLine("new model created");

            Patient leaf = new Patient(1);
            Patient leafi = new Patient(0);

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

            Diagnosis diagnosis = new Diagnosis(0, "bebebe");
            managerOfDiagnosis managDiagn = new managerOfDiagnosis(ref rep);
            if (managDiagn.GetDiagnosisById(0, ref diagnosis))
                Console.WriteLine(diagnosis.title);
            else
                Console.WriteLine("Not ok 7");

            if (!managDiagn.DeleteDiagnosis(0))
                Console.WriteLine("Not ok 8");

            if (managDiagn.GetDiagnosisById(0, ref diagnosis))
                Console.WriteLine("Not ok 9");



            appointments.AddAppointment(new Patient(93));
            appointments.AddAppointment(new Patient(95));
            appointments.AddAppointment(new Patient(97), 1);
            appointments.AddAppointment(new Patient(93), 3, new DateTime(2019, 11, 2, 16, 0, 0));

            appointments.NextDay();
            Console.WriteLine("End");*/
        }
    }
}
