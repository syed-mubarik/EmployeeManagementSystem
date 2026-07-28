using AutoMapper;
using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Employees;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using FluentValidation;

namespace EmployeeManagement.Application.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEmployeeDto> _createValidator;
        private readonly IValidator<UpdateEmployeeDto> _updateValidator;
        public EmployeeService(IUnitOfWork unitOfWork,IMapper mapper,
            IValidator<CreateEmployeeDto> createValidator, IValidator<UpdateEmployeeDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _unitOfWork.Employees.GetAllEmployeesWithDetailsAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDetailDto?> GetByIdAsync(int id)
        {
            // Get employee from repository
            var employee = await _unitOfWork.Employees.GetEmployeeWithDetailsAsync(id);

            // Check if employee exists
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            // Convert Entity to DTO
            return _mapper.Map<EmployeeDetailDto>(employee);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            // Validate
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Convert DTO to Entity
            var employee = _mapper.Map<Employee>(dto);

           // var employee = _mapper.Map<EmployeeManagement.Domain.Entities.Employee>(dto);
           
            // Track the new entity
            await _unitOfWork.Employees.AddAsync(employee);

            // Save (Id generated)
            await _unitOfWork.SaveChangesAsync();
            
            // Generate EmployeeCode
            employee.EmployeeCode = $"EMP{employee.Id:D5}";

            // Later, when we add authentication
            // employee.EmployeeCode = EmployeeCodeGenerator.Generate(employee.Id);

            // Save again
            await _unitOfWork.SaveChangesAsync();

            // Load navigation properties for EmployeeDto(related Department and Designation).

            var createdEmployee = await _unitOfWork.Employees.GetEmployeeWithDetailsAsync(employee.Id);
            
            
            // Convert Entity → DTO (Returning the DTO)
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task UpdateAsync(UpdateEmployeeDto dto)
        {
            var validationresult = await _updateValidator.ValidateAsync(dto);
            if (!validationresult.IsValid)
            {
                throw new ValidationException(validationresult.Errors);
            }
            // Retrieve employee from DB
            var employee = await _unitOfWork.Employees.GetByIdAsync(dto.Id);
           
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            //if (employee.IsDeleted)
            //{
            //    throw new BusinessException("Employee has been deleted.");
            //}

            // copies values into the tracked entity.
            _mapper.Map(dto, employee);
            // Save to database
            await _unitOfWork.SaveChangesAsync();  
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
           
            if(employee == null) {
                throw new KeyNotFoundException("Employee Not Found");
        }
            if (employee.Status == EmployeeStatus.Inactive)
            {
                throw new InvalidOperationException("Employee is already Inactive");
            }
            employee.IsDeleted = true;
            employee.Status = EmployeeStatus.Inactive;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedResult<EmployeeDto>> GetEmployeesAsync(EmployeeQueryParameters queryParameters)
        {
            // Get paginated employees from repository
            var employees = await _unitOfWork.Employees.GetEmployeesAsync(queryParameters);

            // Map entities to DTOs
            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees.Items);

            // Return paged result
            var result = new PagedResult<EmployeeDto>
            {
                Items = employeeDtos,
                TotalCount = employees.TotalCount,
                PageNumber = employees.PageNumber,
                PageSize = employees.PageSize
            };
            return result;
        }
    }
}