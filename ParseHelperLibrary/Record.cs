
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseHelperLibrary
{
    public class Record
    {
        public string LastName      { get; set; }
        public string FirstName     { get; set; }
        public string Gender        { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime DateOfBirth { get; set; }

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

        /// <summary>
        /// Static factory method for creating a Record object with a valid string data enumerable
        /// </summary>
        /// <param name="stringFields">Enumerable of strings containing record information</param>
        public static Record CreateRecord(IEnumerable<string> stringFields)
        {
            return stringFields == null ? new Record() : new Record(stringFields);
        }

        /// <summary>
        /// Static factory method overload for creating a Record object with separate parameters rather than an enumerable
        /// </summary>
        /// <param name="lastName">Last Name</param>
        /// <param name="firstName">First Name</param>
        /// <param name="gender">Gender</param>
        /// <param name="favoriteColor">Favorite Color</param>
        /// <param name="dob">Date of Birth</param>
        public static Record CreateRecord(string lastName, string firstName, string gender, string favoriteColor, string dob)
        {
            return new Record(new string[5] { lastName, firstName, gender, favoriteColor, dob });
        }

        /// <summary>
        /// Override of ToString to return nicely formatted string of record data. C# 6 interpolation.
        /// </summary>
        public override string ToString()
        {
            return $"{LastName}, {FirstName}, {Gender}, {FavoriteColor}, {DateOfBirth.ToString("M/d/yyyy")}";
        }
    }
}
