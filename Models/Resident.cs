namespace API_CONDOMINIO_2.Models;



public class Resident
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Unit Unit { get; set; }
    public int UnitId { get; set; }
    public string Image { get; set; }
    public bool Responsible { get; set; }
    public Sex Sex { get; set; }
    public short SexId { get; set; }
    public bool DisabledPerson { get; set; }
    public bool Excluded { get; set; }
    public DateTime ExclusionDate { get; set; }
    public DateTime CreationDate { get; set; }
    public string Observation { get; set; }
    public bool Defaulter { get; set; }
}

