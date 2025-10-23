namespace EduFlow.Web.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; } = false;

        // 👇 Add this back to fix Dashboard.razor errors
        public int Priority { get; set; } = 1; // 1 = Low, 2 = Medium, 3 = High
    }
}