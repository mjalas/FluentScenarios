namespace FluentScenarios
{
    public class Step<TAction>
    {
        public string Name { get; set; }
        public TAction Action { get; set; }
    }
}