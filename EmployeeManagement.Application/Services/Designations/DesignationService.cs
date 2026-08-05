using AutoMapper;
using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Designation;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using FluentValidation;

namespace EmployeeManagement.Application.Services.Designations
{
    public class DesignationService : IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDesignationDto> _createValidator;
        private readonly IValidator<UpdateDesignationDto> _updateValidator;
        public DesignationService(IUnitOfWork unitOfWork, IMapper mapper, IValidator <CreateDesignationDto> createValidator,
                                   IValidator<UpdateDesignationDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        public async Task<DesignationDto> CreateAsync(CreateDesignationDto dto)
        {
            // Validation
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid) 
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Check if Department already exists
            dto.Name = dto.Name.Trim();
            if (await _unitOfWork.Designations.ExistsByNameAsync(dto.Name))
            {
                throw new DuplicateRecordException("Designation already exists.");
            }

            // Map DTO to Entity
            var designation = _mapper.Map<Designation>(dto);

            // Add Designation
             await _unitOfWork.Designations.AddAsync(designation);

            // save changes

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DesignationDto>(designation);


        }
        public async Task<PagedResult<DesignationDto>> GetPagedDesignationAsync(DesignationQueryParameters queryParameters)
        {
            // Get paginated designation from repository
            var result = await _unitOfWork.Designations.GetPagedDesignationsAsync(queryParameters);

            // Map entities to DTOs
            var designationDtos = _mapper.Map<IEnumerable<DesignationDto>>(result.Items);
            return new PagedResult<DesignationDto>
            {
                Items = designationDtos,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }
        public async Task<DesignationDto> GetByIdAsync(int id)
        {
            var designations = await _unitOfWork.Designations.GetByIdAsync(id);
            if(designations == null) 
             {
                throw new NotFoundException("Designation Not Found.");

                }

            return _mapper.Map<DesignationDto>(designations);

        }
        public async Task UpdateAsync(UpdateDesignationDto dto)
        {
            var validationresult = await _updateValidator.ValidateAsync(dto);
            if (!validationresult.IsValid)
            {
                throw new ValidationException(validationresult.Errors);
            }

            var designations = await _unitOfWork.Designations.GetByIdAsync(dto.Id);
            // Check if department exists
            if (designations == null)
            {
                throw new NotFoundException("Designation Not Found.");
            }
            // Check duplicate name
            if (await _unitOfWork.Designations.ExistsByNameExcludingIdAsync(dto.Name, dto.Id))
            {
                throw new DuplicateRecordException("Designation already exists.");
            }
            // Map updated values onto existing entity
            _mapper.Map(dto, designations);

            await _unitOfWork.SaveChangesAsync();

        }
       public async Task DeleteAsync(int id)
        {
            var designations = await _unitOfWork.Designations.GetByIdAsync(id);
            if(designations == null)
            {
                throw new NotFoundException("Designation not Found.");
            }

            if (await _unitOfWork.Designations.HasEmployeesAsync(id))
            {
                throw new BadRequestException(
                    "Cannot delete designation because it is assigned to one or more employees.");
            }
            designations.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
