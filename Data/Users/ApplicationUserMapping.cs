using AutoMapper;

namespace BlazorServerTest.Data.Users;

public class ApplicationUserMapping : Profile {
    public ApplicationUserMapping() {
        CreateMap<ApplicationUser, SafeApplicationUser>();
    }
}