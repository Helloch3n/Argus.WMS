using System.ComponentModel.DataAnnotations;

namespace Argus.WMS.Putaway.Dtos
{
    public class CompletePutawayTaskInput
    {
        [Required]
        public string TargetLocationCode { get; set; }
    }
}