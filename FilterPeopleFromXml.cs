using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text.Json;

namespace PusulaTalentAcademy
{
    // JSON çıktısında kullanılacak model
    public class PersonReport
    {
        public List<string> Names { get; set; }
        public int TotalSalary { get; set; }
        public double AverageSalary { get; set; }
        public int MaxSalary { get; set; }
        public int Count { get; set; }
    }

    public class PeopleProcessor
    {
        public static string FilterPeopleFromXml(string xmlData)
        {
            // XML'i parse etme:
            var document = XDocument.Parse(xmlData);

            // filtreleme LINQ ile yaptık;
            var selectedPeople = document.Descendants("Person")
                .Where(person =>
                    int.Parse(person.Element("Age")?.Value ?? "0") > 30 &&
                    (person.Element("Department")?.Value ?? "") == "IT" &&
                    int.Parse(person.Element("Salary")?.Value ?? "0") > 5000 &&
                    DateTime.Parse(person.Element("HireDate")?.Value ?? DateTime.MinValue.ToString()).Year < 2019
                )
                .Select(person => new
                {
                    Name = person.Element("Name")?.Value ?? "",
                    Salary = int.Parse(person.Element("Salary")?.Value ?? "0")
                })
                .ToList();

            // eğer hiç kişi yoksa boş değerlerle dön
            if (!selectedPeople.Any())
            {
                return JsonSerializer.Serialize(new PersonReport
                {
                    Names = new List<string>(),
                    TotalSalary = 0,
                    AverageSalary = 0,
                    MaxSalary = 0,
                    Count = 0
                });
            }

            // isimleri alfabetik sırala:
            var names = selectedPeople.Select(p => p.Name).OrderBy(n => n).ToList();

            // maaş hesaplamaları:
            var totalSalary = selectedPeople.Sum(p => p.Salary);
            var avgSalary = selectedPeople.Average(p => p.Salary);
            var maxSalary = selectedPeople.Max(p => p.Salary);
            var count = selectedPeople.Count;

            // sonucu modele yaz
            var report = new PersonReport
            {
                Names = names,
                TotalSalary = totalSalary,
                AverageSalary = avgSalary,
                MaxSalary = maxSalary,
                Count = count
            };

            // JSON çıktısı döndür
            return JsonSerializer.Serialize(report);
        }

        
    }
}

