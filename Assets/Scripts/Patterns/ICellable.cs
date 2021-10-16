namespace Society.Patterns
{
    /// <summary>
    /// наследники - слоты инвентарей
    /// </summary>
    internal interface ICellable
    {
        bool IsEmpty();
        void Clear();
    }
}