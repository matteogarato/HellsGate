namespace HellsGate.Models.DatabaseModel
{
    public class NodeCreateModel : BaseModel
    {
        public string MacAddress { get; set; }

        public string Name { get; set; }

        public WellknownAuthorizationLevel AuthValue { get; set; }
    }
}