using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoMapper;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Features.Users.Queries;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser,Guid>,IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IAmazonSQS _sqsClient;
        private  string _emailQueueUrl;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, IAmazonSQS sqsClient,
            IConfiguration configuration,IMapper mapper)
            :base(context)
        {
            _userManager = userManager;
            _context = context;
            _sqsClient = sqsClient;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Guid> CreateUserAsync(string password, Guid employeeId, Guid roleId)
        {
            var employee = await _context.Staffs
                            .AsNoTracking()
                            .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new Exception("Invalid Employee ID. No employee found.");


            var user = new ApplicationUser
            {
               // Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                UserName = employee.EmployeeName,    
                Email = employee.Email,
                PhoneNumber = employee.Phone,
                RoleId = roleId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }

            var emailMsg = new EmailMessage
            {
                ToEmail = user.Email,
                Subject = "Welcome to Inventory",
                Body = $"Hello {user.UserName}, your account has been created successfully.",
                ProcessorName = "TanvirSakil"
            };

            var messageBody = Newtonsoft.Json.JsonConvert.SerializeObject(emailMsg);

            var sendRequest = new SendMessageRequest
            {
                QueueUrl = _configuration["AWS:SQSQueueUrl"],
                MessageBody = messageBody
            };

            await _sqsClient.SendMessageAsync(sendRequest);

            return user.Id;
        }

        public async Task<(IList<UserDto> Data, int Total, int TotalDisplay)> GetPagedUserAsync(IGetUserListQuery request)
        {


            Expression<Func<ApplicationUser, bool>> filter = c => true;

            //if (!string.IsNullOrWhiteSpace(request.Name))
            //{
            //    filter = filter.AndAlso(c => c.Name.Contains(request.Name));
            //}


            //if (!string.IsNullOrWhiteSpace(request.Search.Value))
            //{
            //    string searchTerm = request.Search.Value.Trim();

            //    filter = filter.AndAlso(c =>
            //        c.Name.Contains(searchTerm)
            //    //  c.IsActive.ToString().Contains(searchTerm) ||
            //    // c.CreatedDate.ToString().Contains(searchTerm)
            //    );
            //}
            var (data,total,totalDisplay) =  await GetDynamicAsync(
                filter,
                request.OrderBy,
                q => q.Include(x => x.Role),
                request.PageIndex,
                request.PageSize,
                isTrackingOff: true   
            );

            var userDtos = data.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleName = user.Role != null ? user.Role.Name : null,
                CompanyName = null, // you can fetch from Employee.Company if needed
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            }).ToList();

            return (userDtos, total, totalDisplay);
        }
        //public async Task<(IList<UserDto> Data, int Total, int TotalDisplay)> GetPagedUserAsync(IGetUserListQuery request)
        //{
        //    var query = _context.Users
        //        .AsNoTracking()
        //        .Include(u => u.Employee) 
        //        .Include(u => u.Role)
        //        .AsQueryable();

        //    //if (!string.IsNullOrWhiteSpace(request.SearchText))
        //    //{
        //    //    var search = request.SearchText.Trim().ToLower();

        //    //    query = query.Where(u =>
        //    //        u.UserName.ToLower().Contains(search) ||
        //    //        u.Email.ToLower().Contains(search) ||
        //    //        u.PhoneNumber.ToLower().Contains(search)
        //    //    );
        //    //}

        //    var total = await query.CountAsync();
        //    query = query.OrderByDescending(u => u.CreatedDate);

        //    // Paging
        //    var users = await query
        //        //.Skip(request.PageIndex * request.PageSize)
        //        //.Take(request.PageSize)
        //        .Select(u => new UserDto
        //        {
        //            Id = u.Id,
        //            UserName = u.UserName,
        //            CompanyName = u.Role.CompanyName,
        //            Email = u.Email,
        //            RoleName = u.Role.Name,
        //            //PhoneNumber = u.PhoneNumber,
        //            CreatedDate = u.CreatedDate,
        //            IsActive = u.IsActive,
        //            //Status = u.IsActive ? "Active" : "Inactive"
        //        })
        //        .ToListAsync();

        //    return (users, total, total);
        //}

        public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user =  await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            var userDto = new UserDto
            {
                UserName = user.UserName,
                RoleName = user.Role.Name
            };
            return userDto;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var entity = await _context
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (entity == null) return null;

            return new User(entity.Id ,entity.EmployeeId ,entity.RoleId, entity.UserName, entity.IsActive);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(r => r.Id == user.Id);
            if (entity == null) return false;

            entity.UserName = user.UserName;
            entity.IsActive = user.IsActive;
            if (user.RoleId != Guid.Empty && user.RoleId != entity.RoleId)
            {
                entity.RoleId = user.RoleId;
            }
            return true;
        }

    }


}
