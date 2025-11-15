namespace MigrationLibrary.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute: Attribute
{
    public string ColumnName { get; set; }
    
    public ColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}