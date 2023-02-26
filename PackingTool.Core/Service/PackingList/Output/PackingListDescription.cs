using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.PackingList.Output
{
    public class PackingListDescription
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        public PackingListDescription(
            int id,
            string name
        )
        {
            ID = id;
            Name = name;
        }
    }
}
