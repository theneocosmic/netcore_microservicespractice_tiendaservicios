using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libros.Test
{
	public class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
	{
        public AsyncEnumerable(Expression expression):base(expression){}
        public AsyncEnumerable(IEnumerable<T> enumerable):base(enumerable){}
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		IQueryProvider IQueryable.Provider { get { return new AsyncQueryProvider<T>(this); } }
	}
}
