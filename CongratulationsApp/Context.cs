using System;

namespace CongratulationsApp
{
    internal class Context
    {
        public int id { get; set; }
        public DateTime birthday { get; set; }
        public string description { get; set; }
        public override string ToString()
        {
            return $"| ID: {id} | Дата: {birthday.Day}/{birthday.Month}/{birthday.Year} | Описание: {description} |";
        }
    }
}
