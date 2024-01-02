namespace TaskProject.Models
{
    public class Task
    {
    public int Id { get; set; }
    public string Description { get; set; }
    public bool status{ get; set; }
    public string Deadline { get; set; }

     public long UserId {get;set;}

    
    }
}




