using System;
using System.IO;
using System.Linq;

namespace CSVVerarbeitungMitArrays
{
    class Person
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public int Alter { get; set; }
        public string Beruf { get; set; }

        public Person(string vorname, string nachname, int alter, string beruf)
        {
            Vorname = vorname;
            Nachname = nachname;
            Alter = alter;
            Beruf = beruf;
        }

        public override string ToString()
        {
            return $"{Vorname};{Nachname};{Alter};{Beruf}";
        }
    }

    class Program
    {
        static Person[] LesePersonenAusCsv(string dateiname)
        {
            Person[] personen = new Person[0];
            try
            {
                if (File.Exists(dateiname))
                {
                    string[] zeilen = File.ReadAllLines(dateiname);
                    if (zeilen.Length > 1)
                    {
                        personen = new Person[zeilen.Length - 1]; // Array ohne Kopfzeile
                        for (int i = 1; i < zeilen.Length; i++)
                        {
                            string[] teile = zeilen[i].Split(';');
                            if (teile.Length == 4)
                            {
                                if (int.TryParse(teile[2], out int alter))
                                {
                                    personen[i - 1] = new Person(teile[0], teile[1], alter, teile[3]);
                                }
                                else
                                {
                                    Console.WriteLine($"Ungültiges Alter in Zeile: {zeilen[i]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Ungültige Zeile in CSV: {zeilen[i]}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Die Datei '{dateiname}' ist leer oder enthält nur eine Kopfzeile.");
                    }
                }
                else
                {
                    Console.WriteLine($"Die Datei '{dateiname}' wurde nicht gefunden.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Lesen der Datei: {ex.Message}");
            }
            return personen;
        }

        static Person ErfasseNeuePerson()
        {
            Console.Write("Geben Sie den Vornamen der neuen Person ein: ");
            string vorname = Console.ReadLine();

            Console.Write("Geben Sie den Nachnamen der neuen Person ein: ");
            string nachname = Console.ReadLine();

            int alter;
            while (true)
            {
                Console.Write("Geben Sie das Alter der neuen Person ein: ");
                if (int.TryParse(Console.ReadLine(), out alter))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe. Bitte geben Sie eine Zahl für das Alter ein.");
                }
            }

            Console.Write("Geben Sie den Beruf der neuen Person ein: ");
            string beruf = Console.ReadLine();

            return new Person(vorname, nachname, alter, beruf);
        }

        static void SpeicherePersonenAlsCsv(string dateiname, Person[] personen)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(dateiname, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("Vorname;Nachname;Alter;Beruf"); // Kopfzeile schreiben
                    foreach (Person person in personen)
                    {
                        writer.WriteLine(person.ToString());
                    }
                }
                Console.WriteLine($"Die Daten wurden erfolgreich in '{dateiname}' gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Datei: {ex.Message}");
            }
        }

        static void Main(string[] args)
        {
            // 3. CSV-Daten einlesen und in ein Array speichern
            Person[] personen = LesePersonenAusCsv("personen.csv");
            Console.WriteLine("Aktuelle Personen:");
            foreach (Person person in personen)
            {
                Console.WriteLine(person);
            }

            // 4. Neue Person erfassen
            Console.WriteLine("\nGeben Sie die Daten für eine neue Person ein:");
            Person neuePerson = ErfasseNeuePerson();

            // Erstellen eines neuen Arrays mit Platz für die zusätzliche Person
            Person[] erweitertePersonen = new Person[personen.Length + 1];
            // Kopieren der vorhandenen Personen in das neue Array
            Array.Copy(personen, erweitertePersonen, personen.Length);
            // Hinzufügen der neuen Person am Ende des Arrays
            erweitertePersonen[personen.Length] = neuePerson;

            Console.WriteLine("\nPersonen nach dem Hinzufügen:");
            foreach (Person person in erweitertePersonen)
            {
                Console.WriteLine(person);
            }

            // 5. Erweiterte Daten in ein neues Array speichern und in die Datei schreiben
            SpeicherePersonenAlsCsv("personen.csv", erweitertePersonen);

            Console.WriteLine("\nDrücken Sie eine beliebige Taste, um das Programm zu beenden.");
            Console.ReadKey();

        }
    }
}