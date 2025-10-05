namespace Application.Statics.Configurations
{
    public static class DbSettings
    {
        public static string DefaultConnection { get; set; } = string.Empty;
        public static int TimeoutInMinutes { get; set; }
    };
}
