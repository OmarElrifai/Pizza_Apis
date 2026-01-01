namespace WebApi_Basic.Records;

public record class ToDo
{
    public required int id {get; set;}
    public required string name {get; set;}

    public required DateTime dueTime {get; set;}

    public required bool isCompleted {get; set;}

}
