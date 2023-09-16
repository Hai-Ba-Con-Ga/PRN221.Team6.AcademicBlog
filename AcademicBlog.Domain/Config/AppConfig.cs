namespace AcademicBlog.Domain.Config
{
    public class AppConfig
    {
        public static string ConnectionStrings { get; set; } = "Server=.;Database=AcademicBlog;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
