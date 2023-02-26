using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PackingTool.Core.Service.PackingList.Json
{
    public class PackingList
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public PackingListContent Content { get; set; }

        public PackingList()
        {
            ID = 0;
            Name = string.Empty;
            Content = new PackingListContent();
        }

        [JsonConstructor]
        private PackingList(
            int id,
            string name,
            PackingListContent content
        )
        {
            ID = id;
            Name = name;
            Content = content;
        }

        public static PackingList FromJson(
            string listName,
            string jsonList
        )
        {
            var content = PackingListContent.FromJson(jsonList);
            return new PackingList(
                id: 0,
                name: listName,
                content: content
            );
        }

        public static PackingList FromDatabase(
            Repository.PackingList.Output.PackingListDb packingListDb
        )
        {
            var content = PackingListContent.FromJson(packingListDb.Json);
            return new PackingList(
                id: packingListDb.ID,
                name: packingListDb.Name,
                content: content
            );
        }

        
    }
}
