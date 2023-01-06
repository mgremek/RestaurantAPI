namespace ToDo.MinimalApi
{
    public class ToDoModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Value { get; set; }
        public bool IsCompleted { get; set; }
            
    }
}
