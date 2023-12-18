using MauiEFStress.Entity;
using MauiEFStress.Service;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;

namespace MauiEFStress.Model
{
    public class MainModel : INotifyPropertyChanged
    {
        //PeopleContext db = new PeopleContext();

        public MainModel()
        {
            Task.Run(async () =>
            {
                //await LoadList();
                await DeleteAllData();
            });
        }

        private List<Person> list;
        public List<Person> List
        {
            get { return list; }
            set
            {
                list = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(List)));
            }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }

        private string insertMilisecs = "0";
        public string InsertMilisecs
        {
            get => insertMilisecs;
            set
            {
                insertMilisecs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InsertMilisecs)));
            }
        }

        private string insertOrReplaceMilisecs = "0";
        public string InsertOrReplaceMilisecs
        {
            get => insertOrReplaceMilisecs;
            set
            {
                insertOrReplaceMilisecs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InsertOrReplaceMilisecs)));
            }
        }

        private string bulkInsertMilisecs = "0";
        public string BulkInsertMilisecs
        {
            get => bulkInsertMilisecs;
            set
            {
                bulkInsertMilisecs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BulkInsertMilisecs)));
            }
        }

        private string listLoadMilisecs = "0";
        public string ListLoadMilisecs
        {
            get => listLoadMilisecs;
            set
            {
                listLoadMilisecs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListLoadMilisecs)));
            }
        }

        private bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task RunInsertTask()
        {
            using var db = new PeopleContext();
            //db.Database.EnsureCreated();
            isRunning = true;
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            try
            {
                for (int i = 1; i <= 50000; i++)
                {
                    //var data = new Person { ID = i + 1, Name = $"Person_{i}" };
                    Person data = GeneratePerson(i);
                    //await db.AttemptAndRetry(() => db.AddPerson(data));

                    //db.Add(data);
                    //db.Entry(data).State = EntityState.Detached;
                    db.People.Add(data);
                    db.Entry(data).State = EntityState.Added;
                    db.SaveChanges();

                    db.Entry(data).State = EntityState.Detached;
                }

                await LoadList();
                Count = db.People.Count();
                //Count = await db.GetPeopleAmount();
                stopwatch.Stop();
                InsertMilisecs = stopwatch.ElapsedMilliseconds.ToString();
                await App.Current.MainPage.DisplayAlert("Data Insertion", "Data inserted successfully in " + InsertMilisecs + "ms", "OK"); ;
            }
            catch (Exception ex)
            {
                await LoadList();
                Debug.WriteLine(ex);
            }
            isRunning = false;


        }


        public async Task RunInsertOrReplaceTask()
        {
            using var db = new PeopleContext();
            //db.Database.EnsureCreated();
            isRunning = true;
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            try
            {
                for (int i = 1; i <= 50000; i++)
                {
                    //var data = new Person { ID = i + 1, Name = $"Person_{i}" };
                    Person data = GeneratePerson(i);
                    //await db.AttemptAndRetry(() => db.AddOrReplacePerson(data));
                    //db.People.Attach(data);
                    var existingData = db.People.Find(data.ID);
                    if (existingData != null)
                    {
                        existingData.Name = data.Name;
                        existingData.Salary = data.Salary;
                        existingData.Weight = data.Weight;
                        existingData.DateOfBirth = data.DateOfBirth;


                        //db.Entry(existingData).CurrentValues.SetValues(data);
                        //db.Entry(data).State = EntityState.Modified;
                        db.People.Update(existingData);
                        db.Entry(existingData).State = EntityState.Modified;
                        //db.Entry(data).State = EntityState.Detached;
                        db.SaveChanges();

                        db.Entry(existingData).State = EntityState.Detached;

                    }
                    else
                    {
                        //db.People.Add(data);

                        db.People.Add(data);
                        db.Entry(data).State = EntityState.Added;
                        db.SaveChanges();

                        db.Entry(data).State = EntityState.Detached;
                    }

                }


                await LoadList();
                Count = db.People.Count();
                //Count = await db.GetPeopleAmount();
                stopwatch.Stop();
                InsertOrReplaceMilisecs = stopwatch.ElapsedMilliseconds.ToString();
                await App.Current.MainPage.DisplayAlert("Data Insertion", "Data inserted successfully in " + InsertOrReplaceMilisecs + "ms", "OK"); ;
            }
            catch (Exception ex)
            {
                await LoadList();
                Debug.WriteLine(ex);
            }
            isRunning = false;


        }

        public async Task RunBulkInsertTask()
        {
            using var db = new PeopleContext();
            //db.Database.EnsureCreated();
            isRunning = true;
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            try
            {
                //Person data;
                List<Person> list = new List<Person>();
                for (int i = 1; i <= 50000; i++)
                {
                    //data = new Person { ID = i + 1, Name = $"Person_{i}" };
                    Person data = GeneratePerson(i);
                    list.Add(data);
                }

                //await db.AttemptAndRetry(() => db.AddPeopleBulk(list));
                //db.People.AttachRange(list);
                db.People.AddRange(list);
                db.SaveChanges();

                //db.Entry(list).State = EntityState.Detached;

                await LoadList();

                //Count = await db.GetPeopleAmount();
                Count = db.People.Count();
                stopwatch.Stop();
                BulkInsertMilisecs = stopwatch.ElapsedMilliseconds.ToString();
                await App.Current.MainPage.DisplayAlert("Data Insertion", "Data inserted successfully in " + BulkInsertMilisecs + "ms", "OK"); ;
            }
            catch (Exception ex)
            {
                await LoadList();
                Debug.WriteLine(ex);
            }
            isRunning = false;


        }

        public async Task DeleteAllData()
        {
            using var db = new PeopleContext();
            //await db.DeletePeople();
            //db.RemoveRange(List);
            db.People.ExecuteDelete();
            db.SaveChanges();
            await LoadList();
            //Count = await db.GetPeopleAmount();
            Count = db.People.Count();

            await App.Current.MainPage.DisplayAlert("Data Deleted", "Data deleted successfully!", "OK");
        }

        // TODO: Load list using Select * From sql query
        private async Task LoadList()
        {
            using var db = new PeopleContext();
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                //var res = db.GetDatabaseConnection<Person>().Result.Query<Person>("SELECT * FROM person");
                //List = db.GetPeople().Result.ToList();
                //List<Person> res = db.People.ToList();
                List<Person> res = db.People.FromSqlRaw("SELECT * FROM Person").ToList();
                stopwatch.Stop();
                ListLoadMilisecs = stopwatch.ElapsedMilliseconds.ToString();
                List = res;
                //Count = await db.GetPeopleAmount();
                Count = res.Count;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Problem with the person list addition: " + e.Message);
            }


        }

        private static Person GeneratePerson(int Id)
        {
            Person person = new Person();
            // ID
            person.ID = Id;

            // Name
            Random rnd = new Random();
            int nameLen = rnd.Next(2, 7);
            person.Name = GenerateName(nameLen);

            // Salary
            person.Salary = (decimal)rnd.Next(1000, 10000);

            // Weight
            person.Weight = (decimal)rnd.Next(40, 120);

            // Date Of Birth
            DateTime start = new DateTime(1950, 1, 1);
            int range = (DateTime.Today - start).Days;
            person.DateOfBirth = start.AddDays(rnd.Next(range));

            return person;
        }

        private static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
    }
}
