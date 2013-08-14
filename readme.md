# RIKTIG

This is a sample application that demonstrates the various way to use MassTransit, Automatonymous, Topshelf, and related projects to create applications.

## Getting Started

### Building the Application

To build the solution, open the ```Riktig.sln``` file in the ```src``` folder. NuGet package restore is enabled, so the packages should restore and the solution should build.

### Running the Application

The application consists of multiple services and a web site, so to run them all it is easiest to right-click on the solution, select properties, and check the multiple startup option. The _Riktig.CoordinationService_, _Riktig.ImageRetrievalService_, and the _Riktig.Web_ projects should be changed to _Start_. Once they set specified, press Control+F5 (or just F5 if you want to debug) to run the application.

## Examples

### Image Request

__Uses:__ _MassTransit, Topshelf, Automatonymous, Autofac, ASP.NET MVC 4_

One example has a web form where the URI of an image can be entered. The URI is then sent to a coordinator (built using Automatonymous) that
dispatches the request to the image retrieval service which ultimately retrieves the image from the server. Once retrieved, the service publishes
an event that is observed by the state machine to complete the request.

Clicking on the Image Request link on the web page will open a page where a URI can be entered. There are also some Ajax links for Cat, Dog, etc. that will submit requests for images on imgur.




