using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using PusulaTalentAcademy;

namespace PusulaTalentAcademy
{
    public static class MaxIncreasingSubArrayProcessor
    {
        public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
        {

            if (numbers == null || numbers.Count == 0)
                return "[]";

            List<int> runningList = new List<int>(); // var olan dizi
            List<int> resultList = new List<int>(); // sonuç olacak dizi
            int runningListSum = 0; // zincirdeki toplamı hesap etmek için; başlangıç değeri 0.
            long resultListSum = long.MinValue; // sonuç zincirindeki toplam, negatif sayılar sebebiyle MinValue atandı,



            foreach (var num in numbers)
            {
                if (runningList.Count == 0 || num > runningList.Last())
                {
                    runningList.Add(num);
                    runningListSum += num;
                }
                else
                {
                    // artış bozuldu; önce current'u değerlendir
                    if (runningListSum > resultListSum)
                    {
                        resultListSum = runningListSum;
                        resultList = new List<int>(runningList);
                    }

                    // yeni alt dizi başlat
                    runningList = new List<int> { num };
                    runningListSum = num;
                }
            }

            // döngü bittikten sonra son current'u kontrol et,
            if (runningListSum >= resultListSum)
            {
                resultList = new List<int>(runningList);
            }

            return JsonSerializer.Serialize(resultList);
        
        }
    }

}