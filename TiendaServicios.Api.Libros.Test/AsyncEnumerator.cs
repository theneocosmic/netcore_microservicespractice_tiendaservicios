using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libros.Test
{
	//Esta clase evalua el arreglo que devuelve el entityFramework
	public class AsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> enumerator;
		public T Current => enumerator.Current;

        public AsyncEnumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

        public async ValueTask DisposeAsync()
		{
			await Task.CompletedTask;
		}

		public async ValueTask<bool> MoveNextAsync()
		{
			return await Task.FromResult(enumerator.MoveNext());
		}
	}
}
