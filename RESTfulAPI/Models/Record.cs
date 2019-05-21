using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulAPI.Models
{
    public class Record
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Record(string lastName, string firstName, string gender, string favoriteColor, DateTime dob)
        {
            LastName = lastName;
            FirstName = firstName;
            Gender = gender;
            FavoriteColor = favoriteColor;
            DateOfBirth = dob;
        }

        public Record(IEnumerable<string> objStringData)
        {
            var list = objStringData.ToList();
            LastName = list[0];
            FirstName = list[1];
            Gender = list[2];
            FavoriteColor = list[3];
            DateOfBirth = ParseDateString(list[4]);
        }

        private DateTime ParseDateString(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime result);
            if (result == null || result == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }
            return result;
        }
    }
}