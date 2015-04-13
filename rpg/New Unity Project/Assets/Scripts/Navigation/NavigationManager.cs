
using System.Collections.Generic;
public static class NavigationManager
{
    public struct Route
    {
        public string RouteDescription;
        public bool CanTravel;
    }


    public static Dictionary<string, Route> RouteInformation = new Dictionary<string, Route>() {
        { "World", new Route { RouteDescription = "The big bad world", CanTravel = true}},
        { "Cave", new Route {RouteDescription = "The deep dark cave", CanTravel = false}},
    };

    public static string GetRouteInfo(string destination)
    {
        return RouteInformation.ContainsKey(destination) ? RouteInformation[destination].RouteDescription : null;
    }

    public static bool CanNavigate(string destination)
    {
        return RouteInformation.ContainsKey(destination) ? RouteInformation[destination].CanTravel : false;
    }

    public static void NavigateTo(string destination)
    {
        // The following line is commented out for now 
        // as we have nowhere to go :D
        //Application.LoadLevel(destination);
    }
}
