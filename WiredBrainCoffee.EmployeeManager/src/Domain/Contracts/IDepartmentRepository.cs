//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using WiredBrainCoffee.EmployeeManager.Domain.Models;

namespace WiredBrainCoffee.EmployeeManager.Domain.Contracts;

public interface IDepartmentRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Returns <see cref="Department"/> matching <paramref name="id"/> if found.
    /// </summary>
    /// <param name="id">id of <see cref="Department"/> to find</param>
    /// <param name="track">if <see langword="true"/> then the departments will be tracked so that changes made can be applied</param>
    /// <param name="includeEmployees">if <see langword="true"/> then Employees will be include in <see cref="Department"/></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>
    /// An asynchronous task which upon completion will store either the matching <see cref="Department"/> or
    /// <see langword="null"/> if not found.
    /// </returns>
    Task<Department?> FindByIdAsync(int id, bool includeEmployees, bool track, CancellationToken cancellationToken);

    /// <summary>
    /// Finds All <see cref="Department"/> in pages of size <paramref name="pageSize"/>
    /// </summary>
    /// <param name="pageNumber">page number, used to determine how many records to skip</param>
    /// <param name="pageSize">maximum number of <see cref="Department"/> objects to return</param>
    /// <param name="includeEmployees">if <see langword="true"/> then Employees will be include in <see cref="Department"/></param>
    /// <param name="ascending">sort directory, results will be orded by name</param>
    /// <param name="track">if <see langword="true"/> then the departments will be tracked so that changes made can be applied</param>
    /// <param name="cancellationToken">A cancelation token.</param>
    /// <returns><see cref="IAsyncEnumerable{Department}"/> containing at most <paramref name="pageSize"/> objects.</returns>
    IAsyncEnumerable<Department> FindPageAsync(int pageNumber, int pageSize,
        bool includeEmployees, bool ascending, bool track,
        CancellationToken cancellationToken);
}
