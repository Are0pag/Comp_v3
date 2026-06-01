using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite; // Необходим для работы с SqliteException

namespace Comp.Db;

public static class DbExceptionInterpreter
{
    public static string GetUserFriendlyMessage(Exception ex) {
        // 1. Проверяем, является ли ошибка следствием сбоя обновления в EF Core
        if (ex is DbUpdateException dbEx) {
            // Извлекаем внутреннее исключение провайдера данных (SQLite)
            var inner = dbEx.InnerException;

            if (inner is SqliteException sqliteEx) {
                // SqliteErrorCode.SqliteConstraint имеет числовое значение 19
                // Также проверяем текст ошибки, чтобы точно распознать FOREIGN KEY
                if (sqliteEx.SqliteErrorCode == 19 || sqliteEx.Message.Contains("FOREIGN KEY")) {
                    return "Не удалось удалить этот элемент, так как он используется в других частях программы (например, привязан к компоненту).\n\nСначала удалите или переназначьте связанные объекты.";
                }

                if (sqliteEx.Message.Contains("UNIQUE constraint failed")) {
                    return "Запись с такими данными уже существует в системе (нарушена уникальность поля).";
                }
            }
        }

        // 2. Если это наше кастомное исключение бизнес-логики
        if (ex is InvalidOperationException) {
            return ex.Message;
        }

        // 3. Для всех остальных непредвиденных системных ошибок
        return "Произошла непредвиденная ошибка при сохранении данных в локальную базу.";
    }
}