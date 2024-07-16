using Azure;
using MicroShop.Integrations.MessageBus.Abstractions;
using MicroShop.Services.Auth.AuthApiApp.Models;
using MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;
using MicroShop.Services.Auth.AuthApiApp.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroShop.Services.Auth.AuthApiApp.Api;

internal static class AuthApiRouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapAuthApi(this RouteGroupBuilder builder)
    {
        return builder
            .MapRegister()
            .MapLogin();
    }

    public static RouteGroupBuilder MapRegister(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("register", Handler)
            .WithName("Register")
            .WithOpenApi();

        return builder;

        static async Task<Results<Ok<ResponseDto<UserDto>>, BadRequest<ResponseDto<UserDto>>>> Handler(
            [FromBody] RegistrationRequestDto model,
            [FromServices] IAuthService authService,
            [FromServices] IRabbitAuthSender messageSender,
            [FromServices] IOptions<RabbitSettings> options)
        {
            // TODO: validate using FluentValidation as there isn't an available validator by default yet
            ResponseDto<UserDto> response = await authService.Register(model.Email, model.Name, model.PhoneNumber, model.Password);

            if (!response.Success)
            {
                return TypedResults.BadRequest(ResponseDto.Error<UserDto>("Unable to register, one or more fields are invalid."));  // intentionally vague;
            }
            // auth service should log underlying errors as we won't be specific with the failure here to prevent attacks attempting to identify existing users
            if (!options.Value.TryGetQueueByName("RegisterUser", out RabbitQueue? queue))
            {
                // should return 500 series error
                return TypedResults.BadRequest<ResponseDto<UserDto>>(ResponseDto.Error<UserDto>("configuration error"));
            }
            messageSender.SendMessage(model.Email, queue.QueueName);
            return TypedResults.Ok(response);
        }
    }

    public static RouteGroupBuilder MapLogin(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("login", Handler)
            .WithName("Login")
            .WithOpenApi();

        return builder;

        static async Task<Results<Ok<ResponseDto<LoginResponseDto>>, BadRequest<ResponseDto<LoginResponseDto>>>> Handler(LoginRequestDto model, [FromServices] IAuthService authService)
        {
            ResponseDto<LoginResponseDto> response = await authService.Login(model.UserName, model.Password);
            return response.Success
                ? TypedResults.Ok(response)
                : TypedResults.BadRequest(ResponseDto.Error<LoginResponseDto>("Username or password is incorrect.")); // intentionally vague
        }
    }

    public static RouteGroupBuilder MapPostRoute(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("role", Handler)
            .WithName("AssignRole")
            .WithOpenApi();

        return builder;

        static async Task<Results<Ok<ResponseDto>, NotFound<ResponseDto>>> Handler([FromBody] ChangeRoleDto model, [FromServices] IAuthService authService)
        {
            // TODO: validate model
            return await authService.AssignRole(model.Email, model.RoleName)
                ? TypedResults.Ok(ResponseDto.Ok())
                : TypedResults.NotFound(ResponseDto.Error("User not found or unable to assign role"));
        }
    }
}
