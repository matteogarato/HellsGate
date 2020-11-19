using HellsGate.Models.DatabaseModel;
using System.Collections.Generic;
using System.Reflection;

namespace HellsGate.Models
{
    public class PeopleManagerViewModel
    {
        public PropertyInfo[] Column => (new PersonModel()).GetType().GetProperties();
        public List<PersonModel> People { get; set; }
    }
}