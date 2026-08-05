using AutoMapper;
using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.DTOs.Employees;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using FluentValidation;

namespace EmployeeManagement.Application.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDepartmentDto> _createValidator;
        private readonly IValidator<UpdateDepartmentDto> _updateValidator;
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper,IValidator<CreateDepartmentDto>createValidator,
                                 IValidator<UpdateDepartmentDto> updateValidator)
        {
            _unitOfWork = unitOfWork;       
            _mapper = mapper;
            _createValidator = createValidator;
             _updateValidator = updateValidator;
        }
        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            // Check if Department already exists

            if (await _unitOfWork.Departments.ExistsByNameAsync(dto.Name))
            {
                throw new DuplicateRecordException("Department already exists.");
            }

            // Map DTO to Entity
           var department = _mapper.Map<Department>(dto);
            
            // Add Department
            await _unitOfWork.Departments.AddAsync(department);

            // save changes
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<PagedResult<DepartmentDto>> GetDepartmentsAsync(DepartmentQueryParameters queryParameters)
        {
            // Get paginated employees from repository
            var result = await _unitOfWork.Departments.GetDepartmentsAsync(queryParameters);

            // Map entities to DTOs
           var departmnetDto = _mapper.Map<IEnumerable<DepartmentDto>>(result.Items);

            return new PagedResult<DepartmentDto>
            {
                Items = departmnetDto,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<DepartmentDetailDto?> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
           
            if (department == null)
            {
                throw new NotFoundException("Department not Found.");
            }
            return _mapper.Map<DepartmentDetailDto>(department);
        }

        public async Task UpdateAsync(UpdateDepartmentDto dto)
        {
            var validationresult = await _updateValidator.ValidateAsync(dto);
            if (!validationresult.IsValid)
            {
                throw new ValidationException(validationresult.Errors);
            }
            // Check if department exists
            var department = await _unitOfWork.Departments.GetByIdAsync(dto.Id);
            if (department == null)
            {
                throw new NotFoundException("Department not Found.");
            }
           
            // Check duplicate name
            if (await _unitOfWork.Departments.ExistsByNameExcludingIdAsync(dto.Name, dto.Id)) 
            {
                throw new DuplicateRecordException("Department already exists.");
            }

            // Map updated values onto existing entity
            _mapper.Map(dto, department);

            await _unitOfWork.SaveChangesAsync();
            
        }
        public async Task DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                throw new NotFoundException("Department Not Found.");
            }
            department.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            
        }
    }
    

}
