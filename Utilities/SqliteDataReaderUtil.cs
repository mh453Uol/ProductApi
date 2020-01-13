using Microsoft.Data.Sqlite;

namespace RefactorThis.Utilities
{
    public static class SqliteDataReaderUtil
    {
        
        public static T GetValueOrDefault<T>(this SqliteDataReader reader, string columnName)
        {
            // Get column index
            var columnIndex = reader.GetOrdinal(columnName);
            // If the column value is NULL return default nullable value for the column
            return reader.IsDBNull(columnIndex) ? default(T) : reader.GetFieldValue<T>(columnIndex);
        }
    }
}