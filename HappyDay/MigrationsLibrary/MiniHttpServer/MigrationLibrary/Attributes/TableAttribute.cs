namespace MigrationLibrary.Attributes;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TableAttribute: Attribute
{
    public string TableName { get; set; }
    
    public TableAttribute(string tableName)
    {
        TableName = tableName;
    }
}