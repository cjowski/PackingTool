using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PackingTool.Core.Service.PackingList.Json
{
    public class PackingTask
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Done { get; set; }

        public PackingTask()
        {
            ID = 0;
            Name = string.Empty;
            Done = false;
        }

        [JsonConstructor]
        private PackingTask(
            int id,
            string name,
            bool done
        )
        {
            ID = id;
            Name = name;
            Done = done;
        }
    }
}
