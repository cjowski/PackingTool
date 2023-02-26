using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PackingTool.Core.Service.PackingList.Json
{
    public class PackingGroup
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Item.PackingItemType Type { get; set; }
        [Required]
        public int Sort { get; set; }
        [Required]
        public Item.PackingItem[] Items { get; set; }

        [JsonConstructor]
        private PackingGroup(
            int id,
            string name,
            Item.PackingItemType type,
            int sort,
            Item.PackingItem[] items
        )
        {
            ID = id;
            Name = name;
            Type = type;
            Sort = sort;
            Items = items;
        }

        public bool IsValid()
        {
            if (ID <= 0)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: ID is missing."
                );
            }

            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: name is missing."
                );
            }

            if (Type == Item.PackingItemType.Undefined)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: '{Name}' Type is undefined."
                );
            }

            if (Items == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingGroup)}: '{Name}' has null Items."
                );
            }

            return Items.All(i => i.IsValid());
        }
    }
}
