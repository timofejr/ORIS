using System.Text;
using Npgsql;

namespace MyORMLibrary;

public class ORMContext
{
    private readonly string _connectionString;
    
    public ORMContext(string connectionString)
    {
        _connectionString = connectionString;
    }
 
    public T Create<T>(T entity, string tableName) where T : class
    {
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        {
            var sqlQueryStringBuilder = new StringBuilder();
            sqlQueryStringBuilder.Append($"INSERT INTO {tableName.ToLower()} (");
            
            foreach (var prop in typeof(T).GetProperties().Skip(1))
            {
                sqlQueryStringBuilder.Append($"{prop.Name.ToLower()}," );
            }
            sqlQueryStringBuilder.Remove(sqlQueryStringBuilder.Length - 1, 1);
            sqlQueryStringBuilder.Append(") VALUES (");
            
            foreach (var prop in typeof(T).GetProperties().Skip(1))
            {
                sqlQueryStringBuilder.Append($"@{prop.Name.ToLower()},");
            }
            
            sqlQueryStringBuilder.Remove(sqlQueryStringBuilder.Length - 1, 1);
            sqlQueryStringBuilder.Append(")");
            
            var command = dataSource.CreateCommand(sqlQueryStringBuilder.ToString());
            foreach (var prop in typeof(T).GetProperties().Skip(1))
            {
                command.Parameters.AddWithValue($"{prop.Name.ToLower()}", prop.GetValue(entity));
            }

            command.ExecuteNonQuery();
            
            return entity;
        }
    }
 
    public T ReadById<T>(int id) where T : class, new()
    {
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        {
            string sqlQuery = $"SELECT * FROM {typeof(T).Name.ToLower()} WHERE Id = @id";
            var command = dataSource.CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@id", id);
            
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var obj = new T();
                    
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        var data = reader[prop.Name];
                        
                        prop.SetValue(obj, Convert.ChangeType(data, prop.PropertyType), null);
                    }

                    return obj;
                }
            }
        }
        return null;
    }
 
    public List<T> ReadByAll<T>() where T : class, new()
    {
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        {
            string sqlQuery = $"SELECT * FROM {typeof(T).Name.ToLower()}";
            var command = dataSource.CreateCommand(sqlQuery);

            var dataList = new List<T>();

            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new T();

                        foreach (var prop in typeof(T).GetProperties())
                        {
                            var data = reader[prop.Name];

                            prop.SetValue(obj, Convert.ChangeType(data, prop.PropertyType), null);
                        }

                        dataList.Add(obj);
                    }
                }

                return dataList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
 
    public void Update<T>(int id, T entity, string tableName)
    {
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        {
            var sqlQueryStringBuilder = new StringBuilder();
            sqlQueryStringBuilder.Append($"UPDATE {tableName.ToLower()} SET ");

            foreach (var prop in typeof(T).GetProperties().Skip(1))
            {
                sqlQueryStringBuilder.Append($"{prop.Name.ToLower()} = @{prop.Name.ToLower()},");
            }

            sqlQueryStringBuilder.Remove(sqlQueryStringBuilder.Length - 1, 1);
            sqlQueryStringBuilder.Append(" WHERE Id = @id");

            var command = dataSource.CreateCommand(sqlQueryStringBuilder.ToString());

            foreach (var prop in typeof(T).GetProperties().Skip(1))
            {
                command.Parameters.AddWithValue($"{prop.Name.ToLower()}", prop.GetValue(entity) ?? DBNull.Value);
            }

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
 
    public void Delete(int id, string tableName)
    {
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        {
            string sqlQuery = $"DELETE FROM {tableName.ToLower()} WHERE Id = @id";
            var command = dataSource.CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
}