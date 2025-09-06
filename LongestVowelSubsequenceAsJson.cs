using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PusulaTalentAcademy
{
    // her kelimenin sonucunu saklamak için küçük bir model sınıfı, sadece gerekliler:
    public class Vowel
    {
        public string Word { get; set; }     // kelimenin kendisi
        public string Sequence { get; set; } // en uzun sesli harf dizisi
        public int Length { get; set; }      // dizi uzunluk
    }

    public class Program
    {
        // örnek çıktılardaki gibi sadece ingilizce sesli harfleri (küçük,büyük harf dahil) listeledim, hashset içine dahil ettim:
        private static readonly HashSet<char> Vowels = new HashSet<char>
        {
            'a','e','i','o','u',
            'A','E','I','O','U'
        };

        // kelimeleri alır, her biri için sonucu hesaplar, JSON döndürür
        public static string LongestVowelSubsequenceAsJson(List<string> words)
        {
            var results = new List<Vowel>();

            foreach (var word in words)
            {
                // her kelimenin içinden en uzun sesli zinciri çıkartıyoruz, zincir kopmadan:
                string longestSeq = ExtractLongestVowelChain(word);

                results.Add(new Vowel
                {
                    Word = word,
                    Sequence = longestSeq,
                    Length = longestSeq.Length
                });
            }

            // JSON formatına dönüştürüp geri verdik:
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = false });
        }

        // tek bir kelimede en uzun sesli harf zincirini bulan metod
        private static string ExtractLongestVowelChain(string word)
        {
            string current = "";  // Şu anki zincir
            string longest = "";  // En uzun zincir

            foreach (char c in word)
            {
                if (Vowels.Contains(c))
                {
                    // sesli harfse zincire ekle:
                    current += c;

                    // daha uzun sesli zincir bulunursa güncelle:
                    if (current.Length > longest.Length)
                        longest = current;
                }
                else
                {
                    // sessiz harf geldiğinde zinciri sıfırla
                    current = "";
                }
            }

            return longest;
        }
    }
}
