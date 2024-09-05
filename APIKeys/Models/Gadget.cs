namespace APIKeys.Models
{
    public class Gadget
    {
        public int Id { get; set; }

        public string GadgetType { get; set; } = string.Empty;

        public string UsageInstructions { get; set; } = string.Empty;
    }
}
