using System.Linq;
using AwawaTech.Mecanaut.API.IAM.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.IAM.Domain.Repositories;
using AwawaTech.Mecanaut.API.IAM.Domain.Services;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;
using AwawaTech.Mecanaut.API.IAM.Interfaces.ACL;

namespace AwawaTech.Mecanaut.API.IAM.Application.Internal.CommandServices;

/**
 * <summary>
 *     The user command service
 * </summary>
 * <remarks>
 *     This class is used to handle user commands
 * </remarks>
 */
public class UserCommandService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ITenantRepository tenantRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    ISubscriptionPlanAcl _subscriptionPlanAcl,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = tokenService.GenerateToken(user);
        

        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task<User> Handle(SignUpCommand command)
    {
        if (!await _subscriptionPlanAcl.ExistsByIdAsync(command.SubscriptionPlanId))
            throw new InvalidOperationException("El plan de suscripción especificado no existe.");

        // 1) Crear Tenant
        var tenant = new Tenant(
            command.Ruc,
            command.LegalName,
            command.CommercialName,
            command.Address,
            command.City,
            command.Country,
            command.TenantPhone,
            command.TenantEmail,
            command.Website,
            command.SubscriptionPlanId
            );

        await tenantRepository.AddAsync(tenant);
        // Persistimos el tenant para obtener su Id generado antes de crear el usuario admin
        await unitOfWork.CompleteAsync();

        // 2) Establecer contexto multitenant para la creación del usuario
        TenantContext.SetTenantId(tenant.Id);
        try
        {
            // Verificar username único dentro del nuevo tenant
            if (userRepository.ExistsByUsername(command.Username))
                throw new Exception($"Username {command.Username} is already taken");

            var hashedPassword = hashingService.HashPassword(command.Password);
            var user = new User(command.Username, hashedPassword)
                .UpdatePersonalInfo(command.FirstName, command.LastName, command.Email)
                .SetTenant(tenant.Id);

            var requestedRole = ParseRequestedRole(command.Role);
            var roleEntity = await roleRepository.FindByNameAsync(requestedRole.ToString()) ?? new Role(requestedRole);
            if (roleEntity.Id == 0)
                await roleRepository.AddAsync(roleEntity);

            user.AddRole(roleEntity);

            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();

            return user;
        }
        finally
        {
            // Limpiar el contexto para no afectar otras operaciones
            TenantContext.Clear();
        }
    }

    public async Task<User> Handle(CreateUserCommand command)
    {
        var tenantId = TenantContext.CurrentTenantId;
        
        var canAddUser = await CanAddAdminUser(tenantId);
        if (!canAddUser)
            throw new Exception("No puedes agregar más usuarios administradores, se ha alcanzado el límite del plan de suscripción.");
        
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");

        var hashed = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashed)
            .UpdatePersonalInfo(command.FirstName, command.LastName, new EmailAddress(command.Email))
            .SetTenant(TenantContext.CurrentTenantId);
        
        
        var rolesToAssign = new List<Role>();
        if (command.Roles != null)
        {
            foreach (var name in command.Roles)
            {
                var role = await roleRepository.FindByNameAsync(name);
                if (role == null)
                {
                    // create role on the fly if does not exist
                    role = new Role(Enum.Parse<Roles>(name, true));
                    await roleRepository.AddAsync(role);
                }
                rolesToAssign.Add(role);
            }
        }
        user.AddRoles(rolesToAssign);
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();
        return user;
    }

    public async Task<User> Handle(UpdateUserCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId) ?? throw new Exception("User not found");
        user = user.UpdatePersonalInfo(command.FirstName, command.LastName, new EmailAddress(command.Email));
        if (command.Roles.Any())
        {
            var roles = new List<Role>();
            foreach (var name in command.Roles)
            {
                var role = await roleRepository.FindByNameAsync(name);
                if (role == null)
                {
                    role = new Role(Enum.Parse<Roles>(name, true));
                    await roleRepository.AddAsync(role);
                }
                roles.Add(role);
            }
            user.AddRoles(roles);
        }
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
        return user;
    }

    public async Task<User> Handle(DeleteUserCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId) ?? throw new Exception("User not found");
        user.Deactivate();
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
        return user;
    }

    private static Roles ParseRequestedRole(string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return Roles.RoleAdmin;

        var normalized = role.Trim();
        if (Enum.TryParse<Roles>(normalized, ignoreCase: true, out var parsedRole))
            return parsedRole;

        return normalized.ToLowerInvariant() switch
        {
            "admin" or "administrator" or "administrador" => Roles.RoleAdmin,
            "technical" or "technician" or "tecnico" or "técnico" => Roles.RoleTechnical,
            _ => throw new InvalidOperationException($"Invalid role specified: {role}")
        };
    }
    
    public async Task<bool> CanAddAdminUser(long tenantId)
    {
        // Llamar a la capa ACL para obtener el número máximo de usuarios
        var maxUsers = await _subscriptionPlanAcl.GetMaxUsersByTenantId(tenantId);

        // Obtener la cantidad de usuarios admin actuales
        var currentAdminUserCount = await userRepository.GetAdminUserCountByTenantId(tenantId);

        // Compara si se puede agregar un nuevo usuario admin
        return currentAdminUserCount < maxUsers;
    }

    
}