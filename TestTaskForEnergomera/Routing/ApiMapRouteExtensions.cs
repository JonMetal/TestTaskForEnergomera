using TestTaskForEnergomera.Models;
using TestTaskForEnergomera.Services;

namespace TestTaskForEnergomera.Routing
{
    public static class ApiMapRouteExtensions
    {
        public static void MapRoute(this RouteGroupBuilder routeGroupBuilder, IFieldService fieldService)
        {
            routeGroupBuilder.MapGet("/fields", () =>
            {
                return Results.Ok(fieldService.GetAllFields());
            });

            routeGroupBuilder.MapGet("/fields/size/{id}", (int id) =>
            {
                try
                {
                    return Results.Ok(fieldService.GetFieldSize(id));
                }
                catch (ArgumentNullException)
                {
                    return Results.NotFound($"Field with Id {id} not found");
                }
                catch
                {
                    return Results.BadRequest("Bad Request");
                }
            });

            routeGroupBuilder.MapGet("/fields/{lat}-{lon}-{id}", (double lat, double lon, int id) =>
            {
                try
                {
                    return Results.Ok(fieldService.Distance(lat, lon, id));
                }
                catch (ArgumentNullException)
                {
                    return Results.NotFound($"Field with Id {id} not found");
                }
                catch
                {
                    return Results.BadRequest("Bad Request");
                }
            });

            routeGroupBuilder.MapGet("/fields/point-in-polygon/{lat}-{lon}", (double lat, double lon) =>
            {
                try
                {
                    var result = fieldService.TryFindFieldByPoint(lat, lon);

                    if (result.HasValue)
                    {
                        return Results.Ok(new FieldDetail()
                        {
                            Id = result.Value.Id,
                            Name = result.Value.Name
                        });
                    }
                    else
                    {
                        return Results.Ok(false);
                    }
                }
                catch
                {
                    return Results.BadRequest();
                }
            });
        }
    }
}
