using Application.Exceptions;

namespace Application.Shared;

public static class GuidExtensions
{
    public static Guid ValidateId(this string id)
    {
        if (string.IsNullOrEmpty(id) || !(Guid.TryParse(id, out Guid idGuid)))
            throw new FormatInvalidException("The id is invalid");
        return idGuid;
    }
}
