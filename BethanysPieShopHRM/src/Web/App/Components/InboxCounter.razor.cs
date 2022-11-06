using System.Security.Cryptography;
using BethanysPieShopHRM.Web.App.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Components;

public partial class InboxCounter
{
    private readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

    public int MessageCount { get; set; }

    [Inject]
    public ApplicationState ApplicationState { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        _randomNumberGenerator.GetBytes(bytes);
        uint value = BitConverter.ToUInt32(bytes);
        MessageCount = Math.Abs(Convert.ToInt32(value % 10u)) ;

        ApplicationState.NumberOfMessages = MessageCount;
    }
}
