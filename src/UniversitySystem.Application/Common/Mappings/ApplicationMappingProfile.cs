using AutoMapper;

namespace UniversitySystem.Application.Common.Mappings;

/// <summary>
/// Marker profile registered with AutoMapper during application startup.
/// All domain-to-DTO mapping configurations will be added to this profile
/// or to dedicated feature profiles in the Features/ folder in future tasks.
///
/// Each mapping follows the convention:
///   CreateMap&lt;SourceEntity, DestinationDto&gt;();
/// </summary>
public sealed class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        // Feature-specific mappings will be added here in future tasks.
        // Example:
        //   CreateMap<Student, StudentDto>();
    }
}
