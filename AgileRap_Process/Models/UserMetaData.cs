using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgileRap_Process.Models
{
    public class UserMetaData
    {
    }
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        [DataType(DataType.Password)]
        [Required]
        [NotMapped]
        public string? ConfirmPassword { get; set; }
        [NotMapped]
        public bool? isLogin { get; set; } = false;
    }
}
