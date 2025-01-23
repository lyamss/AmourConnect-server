using API.Services;
using MediatR;

namespace API.Features.Authentification.Register;

public record CommandRegister(string Pseudo, string Description, DateTime? Date_of_birth, string Sex, string City) : IRequest<ApiResponseDto>;