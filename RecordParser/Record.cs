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

        public Record(string lastName, string firstName, string gender, string favoriteColor, string dob)
        {
            LastName = lastName;
            FirstName = firstName;
            Gender = gender;
            FavoriteColor = favoriteColor;
            DateOfBirth = DateTime.Parse(dob);
        }

        public Record(IEnumerable<string> stringFields)
        {
            LastName = stringFields.ElementAt(0);
            FirstName = stringFields.ElementAt(1);
            Gender = stringFields.ElementAt(2);
            FavoriteColor = stringFields.ElementAt(3);
            DateOfBirth = DateTime.Parse(stringFields.ElementAt(4));
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}, {Gender}, {FavoriteColor}, {DateOfBirth.ToString("M/d/yyyy")}";
        }
    }
}
