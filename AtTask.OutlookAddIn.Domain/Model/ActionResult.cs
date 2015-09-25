namespace AtTask.OutlookAddIn.Domain.Model
{
    /// <summary>
    /// Contains 'result' from Stream API action
    /// </summary>
    public class ActionResult<T>
    {
        public T Result { get; set; }
    }
}