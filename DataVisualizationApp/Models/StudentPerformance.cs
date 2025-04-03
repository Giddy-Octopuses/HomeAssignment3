namespace DataVisualizationApp.Models
{
    public class StudentPerformance
    {
        public int Hours_Studied { get; set; }
        public int Attendance { get; set; }
        public required string Parental_Involvement { get; set; } = "Unknown";
        public required string Access_to_Resources { get; set; } = "Unknown";
        public required string Extracurricular_Activities { get; set; } = "Unknown";
        public int Sleep_Hours { get; set; }
        public int Previous_Scores { get; set; }
        public required string Motivation_Level { get; set; } = "Unknown";
        public required string Internet_Access { get; set; } = "Unknown";
        public int Tutoring_Sessions { get; set; }
        public required string Family_Income { get; set; } = "Unknown";
        public required string Teacher_Quality { get; set; } = "Unknown";
        public required string School_Type { get; set; } = "Unknown";
        public required string Peer_Influence { get; set; } = "Unknown";
        public int Physical_Activity { get; set; }
        public required string Learning_Disabilities { get; set; } = "Unknown";
        public required string Parental_Education_Level { get; set; } = "Unknown";
        public required string Distance_from_Home { get; set; } = "Unknown";
        public required string Gender { get; set; } = "Unknown";
        public int Exam_Score { get; set; }
    }
}