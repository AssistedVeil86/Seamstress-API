using System.Text.Json.Serialization;

namespace Seamstress.VSA.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpenseType
{
    SUPPLIES,
    EMPLOYEES
}
