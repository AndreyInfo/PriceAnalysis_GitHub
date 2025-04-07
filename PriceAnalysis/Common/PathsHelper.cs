namespace PriceAnalysis.Common;

public static class PathsHelper
{
    //путь к JS файлу, например, "../Pages/Projects/Edit.razor.js"
    public static string GetRazorJsPath( Type type )
        => $"..{type.FullName?.Replace( type.Assembly.FullName?.Split( ',' ) [0]!, "" ).Replace( '.', '/' )}.razor.js";

    //public static string SharedScriptsPath => "//shared.i-event.org/";
}
