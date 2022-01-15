using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Works with records.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates new record and return id.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to insert data.</param>
        /// <returns>Record id.</returns>
        public int CreateRecord(FileCabinetRecord fileCabinetRecord);

        /// <summary>
        /// Deletes record.
        /// </summary>
        /// <param name="name">Parameter to find parameter to delete record.</param>
        /// <param name="value">Parameter to delete record.</param>
        public void Delete(string name, string value);

        /// <summary>
        /// Updates record.
        /// </summary>
        /// <param name="parameters">Parameter update record.</param>
        public void Update(string parameters);

        /// <summary>
        /// Selects record.
        /// </summary>
        /// <param name="parameters">Parameter select record.</param>
        public void Select(string parameters);

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <returns>Record count.</returns>
        public (int, int) GetStat();

        /// <summary>
        /// Creates snapshot.
        /// </summary>
        /// <param name="snapshot">Parameter to restore data.</param>
        /// <returns>Snapshot.</returns>
        public ReadOnlyCollection<FileCabinetRecord> Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Purges file from deleted records.
        /// </summary>
        /// <returns>Result of action.</returns>
        public int Purge();

        /// <summary>
        /// Inserts new record and return id.
        /// </summary>
        /// <param name="fileCabinetRecord">Parameter to insert data.</param>
        /// <returns>Record id.</returns>
        public int Insert(FileCabinetRecord fileCabinetRecord);
    }
}
