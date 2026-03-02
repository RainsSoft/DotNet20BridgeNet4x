using System;
using System.Threading.Tasks;

namespace Concurrency
{
    public class Dispose : Example
    {
        public override int Id => 51;
        public override async Task RunAsync()
        {
            PrintWithThread("Main method");

            var customer = await GetCustomerAsync();
        }

        private async Task<Customer> GetCustomerAsync()
        {
            using (var connection = new DatabaseConnection())
            {
                return await connection.GetCustomerAsync();
            }
        }
    }

    public class Customer { }

    public class DatabaseConnection : IDisposable
    {
        private bool _isDisposed = false;

        public async Task<Customer> GetCustomerAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(DatabaseConnection));

            await Task.Delay(5000);

            if (_isDisposed)
                throw new ObjectDisposedException(nameof(DatabaseConnection));

            return new Customer();
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
