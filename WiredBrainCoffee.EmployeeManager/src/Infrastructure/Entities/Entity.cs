﻿//
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


namespace WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;

public abstract class Entity<TId> : Entity, IEquatable<Entity<TId>>
    where TId : notnull, IEquatable<TId> 
{

    protected Entity()
    {
    }

    public TId Id { get; set; } = default!;

    /// <inheritdoc />
    public bool Equals(Entity<TId>? other)
    {
        return other is not null && other.GetType() == GetType() && other.Id.Equals(Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Equals(entity);
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Id.GetHashCode();
}

public abstract class Entity 
{

    protected Entity()
    {
    }

    public DateTimeOffset LastModifiedTime { get; set; } = DateTimeOffset.MinValue;
    public ulong Version { get; set; } 
}
