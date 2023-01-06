namespace ToDo.MinimalApi
{
    public interface IToDoService
    {
        void Create(ToDoModel ToDoModel);
        void Delete(Guid id);
        List<ToDoModel> GetAll();
        ToDoModel GetById(Guid id);
        void Update(ToDoModel ToDoModel);
    }

    public class ToDoService : IToDoService
    {
        private readonly Dictionary<Guid, ToDoModel> _ToDoModels = new();
        public ToDoService()
        {
            var sampleToDoModel = new ToDoModel { Value = "Learn MinimalApi" };
            _ToDoModels[sampleToDoModel.Id] = sampleToDoModel;
        }

        public ToDoModel GetById(Guid id) => _ToDoModels.GetValueOrDefault(id);

        public List<ToDoModel> GetAll() => _ToDoModels.Values.ToList();

        public void Create(ToDoModel ToDoModel)
        {
            if (ToDoModel is null)
                return;

            _ToDoModels[ToDoModel.Id] = ToDoModel;
        }

        public void Update(ToDoModel ToDoModel)
        {
            if (ToDoModel is null)
                return;

            _ToDoModels[ToDoModel.Id] = ToDoModel;
        }

        public void Delete(Guid id)
        {
            _ToDoModels.Remove(id);
        }
    }
}
