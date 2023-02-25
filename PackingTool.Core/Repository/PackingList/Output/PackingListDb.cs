namespace PackingTool.Core.Repository.PackingList.Output
{
    public class PackingListDb
    {
        public int ID { get; }
        public string Name { get; }
        public string Json { get; }

        public PackingListDb(
            int id,
            string name,
            string json
        )
        {
            ID = id;
            Name = name;
            Json = json;
        }
    }
}
