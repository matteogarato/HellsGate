namespace HellsGate.Models.DatabaseModel
{
    public class NodeCreateModel : BaseModel
    {
        public WellknownAuthorizationLevel AuthValue { get; set; }
        public string MacAddress { get; set; }

        public string Name { get; set; }
    }
}