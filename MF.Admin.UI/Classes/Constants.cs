namespace MF.Admin.UI.Classes;

public static class Routes
{
    public static string Films { get; set; } = "Films";
    public static string Directors { get; set; } = "Directors";
    public static string Genres { get; set; } = "Genres";
    public static string SimilarFilms { get; set; } = "SimilarFilms";
    public static string Register { get; set; } = "Register";
}

public static class PageType
{
    public static string Index { get; set; } = "Index";
    public static string Create { get; set; } = "Create";
    public static string Edit { get; set; } = "Edit";
    public static string Delete { get; set; } = "Delete";
    public static string RemoveFilm { get; set; } = "Remove Film";
    public static string AddFilm { get; set; } = "Add Film";
}