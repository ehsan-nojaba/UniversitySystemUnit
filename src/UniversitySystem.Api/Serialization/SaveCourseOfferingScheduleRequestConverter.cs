using System.Text.Json.Serialization;
using System.Text.Json;
using UniversitySystem.Api.Contracts;
using UniversitySystem.Application.Features.CourseOfferings.Commands.CreateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.Commands.SaveCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Commands.UpdateCourseOffering;
using UniversitySystem.Application.Features.CourseOfferings.DTOs;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferingSchedule;
using UniversitySystem.Application.Features.CourseOfferings.Queries.GetCourseOfferings;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.AssignProfessor;
using UniversitySystem.Application.Features.TeachingAssignments.Commands.RemoveProfessorAssignment;
using UniversitySystem.Application.Features.TeachingAssignments.DTOs;
using UniversitySystem.Application.Features.TeachingAssignments.Queries.GetTeachingAssignments;

namespace UniversitySystem.Api.Serialization;

/// <summary>
/// تبدیل JSON ورودی زمان‌بندی؛ هر دو قالب آرایه و شیء دارای slots را به قرارداد یکسان تبدیل می‌کند.
/// </summary>
public class SaveCourseOfferingScheduleRequestConverter : JsonConverter<SaveCourseOfferingScheduleRequest>
{
    public override SaveCourseOfferingScheduleRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var slots = JsonSerializer.Deserialize<List<CourseOfferingScheduleSlotDto>>(ref reader, options) ?? [];
            return new SaveCourseOfferingScheduleRequest { Slots = slots };
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("slots", out var slotsProp) || root.TryGetProperty("Slots", out slotsProp))
            {
                var slots = JsonSerializer.Deserialize<List<CourseOfferingScheduleSlotDto>>(slotsProp.GetRawText(), options) ?? [];
                return new SaveCourseOfferingScheduleRequest { Slots = slots };
            }

            return new SaveCourseOfferingScheduleRequest();
        }

        throw new JsonException("Invalid JSON format for SaveCourseOfferingScheduleRequest. Expected array or object with 'slots'.");
    }

    public override void Write(Utf8JsonWriter writer, SaveCourseOfferingScheduleRequest value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("slots");
        JsonSerializer.Serialize(writer, value.Slots, options);
        writer.WriteEndObject();
    }
}
