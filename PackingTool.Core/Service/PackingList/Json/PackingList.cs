using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PackingTool.Core.Service.PackingList.Json
{
    public class PackingList
    {
        public int ID { get; set; } //TODO do not serialize
        [Required]
        public string Name { get; set; } //TODO do not serialize
        public int GridColumnCount { get; set; }
        [Required]
        public PackingGroup[] Groups { get; set; }

        [JsonConstructor]
        private PackingList(
            int id,
            string name,
            int gridColumnCount,
            PackingGroup[] groups
        )
        {
            ID = id;
            Name = name;
            GridColumnCount = gridColumnCount;
            Groups = groups;
        }

        public static PackingList FromJson(
            string jsonList
        )
        {
            var list = JsonConvert.DeserializeObject<PackingList>(jsonList);

            if (list == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingList)}: unable to deserialize JSON."
                );
            }

            list.IsValid(false);
            list.PrettifySorts();
            return list;
        }

        public static PackingList FromDatabase(
            int listID,
            string jsonList
        )
        {
            var list = JsonConvert.DeserializeObject<PackingList>(jsonList);

            if (list == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingList)}: unable to deserialize JSON."
                );
            }

            list.ID = listID;
            list.IsValid(true);
            list.PrettifySorts();
            return list;
        }

        public bool IsValid(
            bool fromDatabase
        )
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidDataException(
                    $"{nameof(PackingList)}: name is missing."
                );
            }

            if (fromDatabase && ID <= 0)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingList)}: '{Name}' invalid ID: {ID}."
                );
            }

            if (GridColumnCount < 1)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingList)}: '{Name}' invalid GridColumnCount: {GridColumnCount}."
                );
            }

            if (Groups == null)
            {
                throw new InvalidDataException(
                    $"{nameof(PackingList)}: '{Name}' has null Groups."
                );
            }

            return Groups.All(g => g.IsValid());
        }

        private void PrettifySorts()
        {
            Groups.ToList().Sort((x, y) => x.Sort.CompareTo(y.Sort));

            var groupSort = 1;
            foreach(var group in Groups)
            {
                group.Sort = groupSort++;
                group.Items.ToList().Sort((x, y) => x.Sort.CompareTo(y.Sort));

                var itemSort = 1;
                foreach(var item in group.Items)
                {
                    item.Sort = itemSort++;
                }
            }
        }
    }
}
