using System.ComponentModel.DataAnnotations;

namespace API_CONDOMINIO_2.ViewModel
{
    public class UnitViewModel
    {

        [Required(ErrorMessage = "O número da unidade é obrigatorio")]
        public int NumberUnit { get; set; }
        [Required(ErrorMessage = " É obrigatorio informar se a unidade tem pessoas vivendo.")]
        public bool PeopleLiving { get; set; } = false;
        public string Observation { get; set; }
        [Required(ErrorMessage = " É obrigatorio informar se a unidade tem garagem.")]
        public bool HasGarage { get; set; } = false;
        [Required(ErrorMessage = " É obrigatorio informar o bloco do apartamento")]
        public int BlockId { get; set; }
    }
}



