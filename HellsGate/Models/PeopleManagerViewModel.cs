using HellsGate.Models.DatabaseModel;
using System.Collections.Generic;
using System.Reflection;

namespace HellsGate.Models
{
    public class PeopleManagerViewModel
    {
        public List<PeopleAnagraphicModel> People { get; set; }

        public PropertyInfo[] Column => (new PeopleAnagraphicModel()).GetType().GetProperties();
    }
}