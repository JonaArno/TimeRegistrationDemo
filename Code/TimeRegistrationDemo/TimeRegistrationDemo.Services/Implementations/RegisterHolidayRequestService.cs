﻿using FluentValidation;
using TimeRegistrationDemo.Data.Entities;
using TimeRegistrationDemo.Repositories.Interfaces;
using TimeRegistrationDemo.Services.Dtos;
using TimeRegistrationDemo.Services.Dtos.RegisterHolidayRequest;
using TimeRegistrationDemo.Services.Interfaces;
using TimeRegistrationDemo.Services.Validation.ValidationResult;

namespace TimeRegistrationDemo.Services.Implementations
{
    public class RegisterHolidayRequestService : IRegisterHolidayRequestService
    {
        private readonly IHolidayRequestRepository HolidayRequestRepository;
        private readonly IHolidayTypeRepository HolidayTypeRepository;
        private readonly IUserRepository UserRepository;
        private readonly IValidator<HolidayRequestEntity> HolidayRequestEntityValidator;
        private readonly IValidator<RegisterHolidayRequestInputDto> RegisterHolidayRequestInputDtoValidator;

        public RegisterHolidayRequestService(
            IHolidayRequestRepository holidayRequestRepository,
            IHolidayTypeRepository holidayTypeRepository,
            IUserRepository userRepository,
            IValidator<HolidayRequestEntity> holidayRequestEntityValidator,
            IValidator<RegisterHolidayRequestInputDto> registerHolidayRequestInputDtoValidator)
        {
            HolidayRequestRepository = holidayRequestRepository;
            HolidayTypeRepository = holidayTypeRepository;
            UserRepository = userRepository;
            HolidayRequestEntityValidator = holidayRequestEntityValidator;
            RegisterHolidayRequestInputDtoValidator = registerHolidayRequestInputDtoValidator;
        }

        public RegisterHolidayRequestOutputDto Register(RegisterHolidayRequestInputDto request)
        {
            // validate dto
            var dtoValidationResult = RegisterHolidayRequestInputDtoValidator.Validate(request).ToTRValidationResult();
            if (!dtoValidationResult.IsValid)
                return new RegisterHolidayRequestOutputDto(dtoValidationResult);

            // get referential data
            var holidayType = HolidayTypeRepository.GetByKey(request.HolidayType);
            var user = UserRepository.GetByKey(request.UserId);

            // create entity
            var holidayRequestEntity = new HolidayRequestEntity()
            {
                From = request.From,
                To = request.To,
                Remarks = request.Remarks,
                HolidayType = holidayType,
                User = user
            };

            //validate entity
            var entityValidationResult = HolidayRequestEntityValidator.Validate(holidayRequestEntity).ToTRValidationResult();
            if (!entityValidationResult.IsValid)
                return new RegisterHolidayRequestOutputDto(entityValidationResult);

            //save entity
            if (entityValidationResult.IsValid)
                HolidayRequestRepository.Register(holidayRequestEntity);

            return new RegisterHolidayRequestOutputDto(holidayRequestEntity.ToDto());
        }
    }
}

//todo dbcontext must be threadsafe + in transaction