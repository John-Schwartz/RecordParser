using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser
{
    public class Record
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Record() { }

        public Record(string lastName, string firstName, string gender, string favoriteColor, DateTime dob)
        {
            LastName = lastName;
            FirstName = firstName;
            Gender = gender;
            FavoriteColor = favoriteColor;
            DateOfBirth = dob;
        }

        public Record(IEnumerable<string> stringFields)
        {
            LastName = stringFields.ElementAt(0);
            FirstName = stringFields.ElementAt(1);
            Gender = stringFields.ElementAt(2);
            FavoriteColor = stringFields.ElementAt(3);
            DateOfBirth = ParseDateString(stringFields.ElementAt(4));
        }

        public DateTime ParseDateString(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime result);
            if (result == null || result == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }
            return result;
        }

        public string GetFormattedString()
        {
            return ("Name: {0,-30} | Gender: {1,-7} | Favorite Color: {2,-15} | DOB: {3,-10}",
                   $"{LastName}, {FirstName}", Gender, FavoriteColor, DateOfBirth.ToString("M/d/yyyy")).ToString();
        }
    }
}
