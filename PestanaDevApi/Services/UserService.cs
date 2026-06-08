using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using System.Net;
using PestanaDevApi.Utils;
using Org.BouncyCastle.Asn1.Ocsp;
using PestanaDevApi.Extensions.Dtos.Requests;

namespace PestanaDevApi.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ISignUpRepository _signUpRepository;
        private readonly IConfirmedEmailsRepository _confirmedEmailsRepository;

        public UserService(IUserRepository repository, ISignUpRepository signUpRepository, IConfirmedEmailsRepository confirmedEmailsRepository)
        {
            _repository = repository;
            _signUpRepository = signUpRepository;
            _confirmedEmailsRepository = confirmedEmailsRepository;
        }

        public async Task<GetUserResponseDto> GetUser(Guid userId)
        {
            User? user = await _repository.GetUser(userId);

            if (user == null)
                return new(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);

            return new(user);
        }

        public async Task<ChangeUserDataResponseDto> ChangeUserData(ChangeUserDataRequestDto dto, Guid userId)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessages.RequestDontHaveAnyChangedData);

            User? currentUserData = await _repository.GetUser(userId);

            if (currentUserData == null)
                return new(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);

            if (dto.UserEmailWasUpdated(currentUserEmail: currentUserData.UserEmail))
            {
                if (!ApiLib.IsEmailValid(dto.Email!))
                    return new(ErrorMessages.InvalidEmailFormat);

                if (await _signUpRepository.IsEmailBeingUsed(dto.Email!))
                    return new(ErrorMessages.EmailAlreadyBeingUsed);

                if (!await _confirmedEmailsRepository.IsEmailConfirmed(dto.Email!))
                    return new(ErrorMessages.EmailNotConfirmed);
            }

            if (!await _repository.UpdateUserData(dto, currentUserData, userId))
                return new(HttpStatusCode.InternalServerError, ErrorMessages.UserNotUpdated);

            return new();
        }

        public async Task<DeleteUserResponseDto> DeleteUser(Guid userId)
        {
            User? user = await _repository.GetUser(userId);

            if (user == null)
                return new(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);

            if (!await _confirmedEmailsRepository.IsEmailConfirmed(user.UserEmail))
                return new(ErrorMessages.UserEmailWasNotConfirmed);

            bool wasUserDeleted = await _repository.DeleteUserData(userId);

            await _confirmedEmailsRepository.DeleteEmailConfirmation(user.UserEmail);

            if (!wasUserDeleted)
                return new(HttpStatusCode.InternalServerError, ErrorMessages.UserNotDeleted);

            return new();
        }
    }
}
