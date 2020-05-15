using System.ComponentModel.DataAnnotations;


namespace cw10.DTOs
{
    public class DeleteStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }
    }
}