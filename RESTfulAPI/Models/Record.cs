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
    }
}