using MigrationLibrary.Attributes;

namespace MiniHttpServer.Models;

[Table("users")]
class User {
    [PrimaryKey]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("surname")]
    public string Surname { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
}