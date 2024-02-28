namespace API_CONDOMINIO_2.Models;

public class Block
{
    public short Id { get; set; }
    public string NameBlock { get; set; }
    public int QuantityeUnit { get; set; }
    public int QuantityFloor { get; set; }
    public List<Unit> Units { get; set; }

}

