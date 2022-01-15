namespace FileCabinetApp
{
    /// <summary>
    /// Interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Set value to nextHandler.
        /// </summary>
        /// <param name="commandHandler">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Perform action.
        /// </summary>
        /// <param name="appComandRequest">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
        public object Handle(AppComandRequest appComandRequest);
    }
}
