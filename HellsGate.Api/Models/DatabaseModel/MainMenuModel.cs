namespace HellsGate.Models.DatabaseModel
{
    public class MainMenuModel : BaseModel
    {
        public string Action { get; set; }
        public WellknownAuthorizationLevel AuthLevel { get; set; }
        public string Controller { get; set; }
        public int Id { get; set; }

        public int? ParentMenu { get; set; }
        public string Text { get; set; }
    }
}