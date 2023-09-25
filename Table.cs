namespace ToDoList
{
    public class Table
    {
        public enum Status
        {
            New,
            InProgress,
            Completed
        }

        public enum Category
        {
            Work,
            Home,
            School,
            Entertainment
        }

        public class Item
        {
            public string TaskId { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public DateTime DueDate { get; set; }

            public Status TaskStatus { get; set; }

            public Category TaskCategory { get; set; }

            public string CreatedBy { get; set; }
        }

    }
}