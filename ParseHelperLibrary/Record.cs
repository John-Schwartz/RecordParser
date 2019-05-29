
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseHelperLibrary
{
    public class Record
    {
        public string LastName      { get; private set; }
        public string FirstName     { get; private set; }
        public string Gender        { get; private set; }
        public string FavoriteColor { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        private Record() { }

        private Record(IEnumerable<string> stringFields)
        {
                LastName = stringFields.ElementAt(0);
                FirstName = stringFields.ElementAt(1);
                Gender = stringFields.ElementAt(2);
                FavoriteColor = stringFields.ElementAt(3);
                DateTime.TryParse(stringFields.ElementAt(4), out DateTime parsedDate);
                DateOfBirth = parsedDate;            
        }

        public static Record CreateRecord(IEnumerable<string> stringFields)
        {
            return stringFields == null ? new Record() : new Record(stringFields);
        }

        public static Record CreateRecord(string lastName, string firstName, string gender, string favoriteColor, string dob)
        {
            return new Record(new string[5] { lastName, firstName, gender, favoriteColor, dob });
        }

        // C# 6+ string interpolation: 
        // Override object ToString to return nicely formatted string
        public override string ToString()
        {
            return $"{LastName}, {FirstName}, {Gender}, {FavoriteColor}, {DateOfBirth.ToString("M/d/yyyy")}";
        }
    }
}
