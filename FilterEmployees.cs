using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaTalentAcademy
{
    // JSON çıktısı için sınıf:
    public class EmployeeSummary
    {
        public List<string> Name { get; set; }          
        public decimal TotalSalary { get; set; }        // toplam maaş
        public decimal AverageSalary { get; set; }      // ortalama maaş
        public decimal MinSalary { get; set; }          // en düşük maaş
        public decimal MaxSalary { get; set; }          // en yüksek maaş
        public int Count { get; set; }                  // toplam kişi sayısı
    }

    public static class EmployeeProcessor
    {
        // verilen çalışan listesi üzerinde filtreleme ve hesaplamaları yapar, JSON format döner,
        public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
        {
            if (employees == null)
                return JsonSerializer.Serialize(EmptySummary());

            // filtreleme yapıyoruz: yaş, departman, maaş aralığı ve işe giriş yılı
            var filteredEmployees = employees
                .Where(emp =>
                    emp.Age >= 25 && emp.Age <= 40 &&
                    (emp.Department == "IT" || emp.Department == "Finance") &&
                    emp.Salary >= 5000 && emp.Salary <= 9000 &&
                    emp.HireDate.Year > 2017
                )
                .ToList();

            // filtreleme sonrası hiç çalışan yoksa, boş JSON:
            if (!filteredEmployees.Any())
                return JsonSerializer.Serialize(EmptySummary());

            // isimleri önce uzunluklarına göre azalan, sonra alfabetik sırala:
            var sortedNames = filteredEmployees
                .OrderByDescending(emp => emp.Name.Length)
                .ThenBy(emp => emp.Name)
                .Select(emp => emp.Name)
                .ToList();

            // maaş bilgilerini hesapla
            var totalSalary = filteredEmployees.Sum(emp => emp.Salary);
            var avgSalary = filteredEmployees.Average(emp => emp.Salary);
            var minSalary = filteredEmployees.Min(emp => emp.Salary);
            var maxSalary = filteredEmployees.Max(emp => emp.Salary);
            var count = filteredEmployees.Count;

            // sonuçları modele yaz
            var summary = new EmployeeSummary
            {
                Name = sortedNames,
                TotalSalary = totalSalary,
                AverageSalary = avgSalary,
                MinSalary = minSalary,
                MaxSalary = maxSalary,
                Count = count
            };

            // JSON olarak döndür
            return JsonSerializer.Serialize(summary);
        }

        // boş sonuç döndürmek için yardımcı metod,
        private static EmployeeSummary EmptySummary()
        {
            return new EmployeeSummary
            {
                Name = new List<string>(),
                TotalSalary = 0,
                AverageSalary = 0,
                MinSalary = 0,
                MaxSalary = 0,
                Count = 0
            };
        }
    }
}
