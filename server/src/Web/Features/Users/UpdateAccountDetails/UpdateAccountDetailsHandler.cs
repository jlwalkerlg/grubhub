﻿using System.Threading;
using System.Threading.Tasks;
using Web.Domain;
using Web.Services.Authentication;

namespace Web.Features.Users.UpdateAccountDetails
{
    public class UpdateAccountDetailsHandler : IRequestHandler<UpdateAccountDetailsCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticator authenticator;

        public UpdateAccountDetailsHandler(IUnitOfWork unitOfWork, IAuthenticator authenticator)
        {
            this.unitOfWork = unitOfWork;
            this.authenticator = authenticator;
        }

        public async Task<Result> Handle(UpdateAccountDetailsCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetById(authenticator.UserId);

            if (user is null) return Error.NotFound("User not found.");

            user.Rename(command.FirstName, command.LastName);
            user.MobileNumber = new MobileNumber(command.MobileNumber);

            await unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
