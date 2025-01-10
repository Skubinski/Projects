using System.ComponentModel.DataAnnotations;
public class Reservation
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string SecondName { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public DateTime Arriving { get; set; }

    [Required]
    public DateTime Leaving { get; set; }

    [Required]
    public string Room { get; set; }

    [Required]
    public int Adults { get; set; }

    public int Children { get; set; }

    public string Message { get; set; }
}
