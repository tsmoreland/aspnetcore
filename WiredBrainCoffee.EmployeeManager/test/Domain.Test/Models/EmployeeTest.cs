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

namespace WiredBrainCoffee.EmployeeManager.Domain.Test.Models;

public sealed class EmployeeTest
{
    public enum ConstructorType
    {
        Default,
        IncludesId,
        IncludesDatabacking,
    }

    [Theory]
    [InlineData(ConstructorType.Default)]
    [InlineData(ConstructorType.IncludesId)]
    [InlineData(ConstructorType.IncludesDatabacking)]
    public void ConstructorShouldThrowWhenFirstNameIsNullOrEmpty(ConstructorType constructorType)
    {
        Department department = new(42, "department");

        Action constructor = BuildConstructorAction(constructorType, department, firstName: null!);
        ArgumentException ex = Assert.Throws<ArgumentException>(constructor);
        ex!.ParamName.Should().Be("firstName");
    }

    [Theory]
    [InlineData(ConstructorType.Default)]
    [InlineData(ConstructorType.IncludesId)]
    [InlineData(ConstructorType.IncludesDatabacking)]
    public void ConstructorShouldThrowWhenlastNameIsNullOrEmpty(ConstructorType constructorType)
    {
        Department department = new(42, "department");

        Action constructor = BuildConstructorAction(constructorType, department, lastName: null!);
        ArgumentException ex = Assert.Throws<ArgumentException>(constructor);
        ex!.ParamName.Should().Be("lastName");
    }

    private static Action BuildConstructorAction(ConstructorType constructorType, Department department, string firstName = "firstName", string lastName = "")
    {
        const int id = 4;
        object? databacking = new();

        Action constructor = constructorType switch
        {
            ConstructorType.IncludesDatabacking => () => _ = new Employee(id, firstName, lastName, true, department, databacking),
            ConstructorType.IncludesId => () => _ = new Employee(id, firstName, lastName, true, department),
            _ => () => _ = new Employee(firstName, lastName, true, department),
        };
        return constructor;
    }
}
