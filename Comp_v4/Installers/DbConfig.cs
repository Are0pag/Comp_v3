using System.IO;

namespace Comp_v4.Installers;

public static class DbConfig
{
    public const string DB_FILE_NAME = "comp.db";
    public static string DbFolderPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    
    public static string ConnectionString { get; } =  $"data source={Path.Combine(DbFolderPath, DB_FILE_NAME)}";
}