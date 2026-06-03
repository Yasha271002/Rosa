using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rosa.Data.Utilities;

public static class CollectionExtension
{
    public static async Task<T> RequestProcessingWithErrorHandling<T>(
        this ILogger logger,
        Func<Task<T>> action,
        string errorMessage = "Произошла ошибка при выполнении операции с БД")
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, errorMessage);
            throw;
        }
    }

    public static async Task RequestProcessingWithErrorHandling(
        this ILogger logger,
        Func<Task> action,
        string errorMessage = "Произошла ошибка при выполнении операции с БД")
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, errorMessage);
            throw;
        }
    }
}